namespace CPP_Schedule_Builder
{
    internal abstract class ScheduleOptimizerBase : IScheduleOptimizer
    {
        public abstract string Name { get; }

        public ScheduleOptimizationResult Optimize(IEnumerable<Lecture> candidates)
        {
            List<List<Lecture>> courseGroups = candidates
                .Where(IsValidCandidate)
                .GroupBy(GetCourseKey)
                .OrderBy(group => group.Count())
                .Select(group => OrderCandidates(group).ToList())
                .ToList();

            if (courseGroups.Count == 0)
            {
                return new ScheduleOptimizationResult(
                    Array.Empty<Lecture>(),
                    new[] { "Add at least one class before building a schedule." });
            }

            List<Lecture> bestSchedule = new();
            ExploreSchedules(courseGroups, 0, new List<Lecture>(), ref bestSchedule);

            List<string> errors = courseGroups
                .Where(group => !bestSchedule.Any(lecture => GetCourseKey(lecture) == GetCourseKey(group[0])))
                .Select(group => $"Could not schedule {GetCourseKey(group[0])} without overlapping another selected class.")
                .ToList();

            return new ScheduleOptimizationResult(bestSchedule, errors);
        }

        protected virtual IEnumerable<Lecture> OrderCandidates(IEnumerable<Lecture> candidates)
        {
            return candidates;
        }

        protected abstract double GetOptimizationScore(IReadOnlyList<Lecture> schedule);

        protected static string GetCourseKey(Lecture lecture)
        {
            return $"{lecture.Subject} {lecture.ClassCode}".Trim();
        }

        protected static TimeSpan GetStartTime(Lecture lecture)
        {
            return ToTimeOfDay(lecture.StartTime, lecture.StartAM_PM);
        }

        protected static TimeSpan GetEndTime(Lecture lecture)
        {
            return ToTimeOfDay(lecture.EndTime, lecture.EndAM_PM);
        }

        protected static TimeSpan ToTimeOfDay(TimeSpan time, string amPm)
        {
            int hour = time.Hours;

            if (string.Equals(amPm, "PM", StringComparison.OrdinalIgnoreCase) && hour != 12)
            {
                hour += 12;
            }
            else if (string.Equals(amPm, "AM", StringComparison.OrdinalIgnoreCase) && hour == 12)
            {
                hour = 0;
            }

            return new TimeSpan(hour, time.Minutes, 0);
        }

        private void ExploreSchedules(
            IReadOnlyList<List<Lecture>> courseGroups,
            int groupIndex,
            List<Lecture> currentSchedule,
            ref List<Lecture> bestSchedule)
        {
            if (groupIndex == courseGroups.Count)
            {
                if (IsBetterSchedule(currentSchedule, bestSchedule))
                {
                    bestSchedule = new List<Lecture>(currentSchedule);
                }

                return;
            }

            foreach (Lecture candidate in courseGroups[groupIndex])
            {
                if (ConflictsWithSchedule(candidate, currentSchedule))
                {
                    continue;
                }

                currentSchedule.Add(candidate);
                ExploreSchedules(courseGroups, groupIndex + 1, currentSchedule, ref bestSchedule);
                currentSchedule.RemoveAt(currentSchedule.Count - 1);
            }

            ExploreSchedules(courseGroups, groupIndex + 1, currentSchedule, ref bestSchedule);
        }

        private bool IsBetterSchedule(IReadOnlyList<Lecture> currentSchedule, IReadOnlyList<Lecture> bestSchedule)
        {
            if (currentSchedule.Count != bestSchedule.Count)
            {
                return currentSchedule.Count > bestSchedule.Count;
            }

            double currentScore = GetOptimizationScore(currentSchedule);
            double bestScore = GetOptimizationScore(bestSchedule);

            if (Math.Abs(currentScore - bestScore) > 0.0001)
            {
                return currentScore > bestScore;
            }

            return currentSchedule.Sum(lecture => GetStartTime(lecture).TotalMinutes) <
                   bestSchedule.Sum(lecture => GetStartTime(lecture).TotalMinutes);
        }

        private static bool ConflictsWithSchedule(Lecture candidate, IEnumerable<Lecture> schedule)
        {
            return schedule.Any(existingLecture => ClassesOverlap(candidate, existingLecture));
        }

        private static bool ClassesOverlap(Lecture first, Lecture second)
        {
            if (!first.Days.Intersect(second.Days).Any())
            {
                return false;
            }

            TimeSpan firstStart = GetStartTime(first);
            TimeSpan firstEnd = GetEndTime(first);
            TimeSpan secondStart = GetStartTime(second);
            TimeSpan secondEnd = GetEndTime(second);

            return firstStart < secondEnd && secondStart < firstEnd;
        }

        private static bool IsValidCandidate(Lecture lecture)
        {
            return !string.IsNullOrWhiteSpace(lecture.Subject) &&
                   !string.IsNullOrWhiteSpace(lecture.ClassCode) &&
                   lecture.Days.Count > 0 &&
                   GetEndTime(lecture) > GetStartTime(lecture);
        }
    }
}
