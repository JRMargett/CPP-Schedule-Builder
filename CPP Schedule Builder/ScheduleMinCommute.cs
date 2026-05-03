using System;
using System.Collections.Generic;
using System.Linq;

namespace CPP_Schedule_Builder
{
    internal class ScheduleMinCommute : Schedule
    {
        private sealed class CourseCandidateGroup
        {
            public CourseCandidateGroup(string classCode, int courseOrder, List<Lecture> candidates)
            {
                ClassCode = classCode;
                CourseOrder = courseOrder;
                Candidates = candidates;
            }

            public string ClassCode { get; }
            public int CourseOrder { get; }
            public List<Lecture> Candidates { get; }
        }

        public ScheduleMinCommute()
        {
        }

        public ScheduleMinCommute(IEnumerable<Lecture> selectedLectures)
        {
            foreach (Lecture lecture in selectedLectures)
            {
                AddLecture(lecture);
            }
        }

        public override bool TryBuildSchedule()
        {
            scheduleNotes.Clear();
            scheduleLectures.Clear();

            List<CourseCandidateGroup> candidateGroups = BuildCandidateGroups();
            List<Lecture> currentSchedule = new List<Lecture>();
            List<Lecture> bestSchedule = new List<Lecture>();

            if (candidateGroups.Count == 0)
            {
                scheduleNotes.Add("No classes have been added yet.");
                return false;
            }

            FindBestSchedule(candidateGroups, 0, currentSchedule, ref bestSchedule);

            scheduleLectures.AddRange(
                bestSchedule
                    .OrderBy(lecture => candidateGroups.First(group => group.ClassCode == lecture.ClassCode).CourseOrder));

            BuildNotes(candidateGroups);
            return scheduleLectures.Count > 0;
        }

        private List<CourseCandidateGroup> BuildCandidateGroups()
        {
            return Lectures
                .Select((lecture, index) => new { Lecture = lecture, OriginalOrder = index })
                .GroupBy(item => item.Lecture.ClassCode)
                .Select((group, courseIndex) => new CourseCandidateGroup(
                    group.Key,
                    courseIndex,
                    group
                        .OrderBy(item => CountMeetingDays(item.Lecture))
                        .ThenBy(item => ConvertTo24Hour(item.Lecture.StartTime, item.Lecture.StartAM_PM))
                        .ThenBy(item => item.OriginalOrder)
                        .Select(item => item.Lecture)
                        .ToList()))
                .ToList();
        }

        private void FindBestSchedule(
            List<CourseCandidateGroup> candidateGroups,
            int courseIndex,
            List<Lecture> currentSchedule,
            ref List<Lecture> bestSchedule)
        {
            if (currentSchedule.Count + candidateGroups.Count - courseIndex < bestSchedule.Count)
            {
                return;
            }

            if (courseIndex == candidateGroups.Count)
            {
                if (IsBetterSchedule(currentSchedule, bestSchedule))
                {
                    bestSchedule = new List<Lecture>(currentSchedule);
                }

                return;
            }

            foreach (Lecture candidate in candidateGroups[courseIndex].Candidates)
            {
                bool hasConflict = currentSchedule.Any(selected => AreConflicting(candidate, selected));

                if (hasConflict)
                {
                    continue;
                }

                currentSchedule.Add(candidate);
                FindBestSchedule(candidateGroups, courseIndex + 1, currentSchedule, ref bestSchedule);
                currentSchedule.RemoveAt(currentSchedule.Count - 1);
            }

            FindBestSchedule(candidateGroups, courseIndex + 1, currentSchedule, ref bestSchedule);
        }

        private bool IsBetterSchedule(List<Lecture> candidateSchedule, List<Lecture> bestSchedule)
        {
            if (candidateSchedule.Count != bestSchedule.Count)
            {
                return candidateSchedule.Count > bestSchedule.Count;
            }

            int candidateCampusDays = CountCampusDays(candidateSchedule);
            int bestCampusDays = CountCampusDays(bestSchedule);
            if (candidateCampusDays != bestCampusDays)
            {
                return candidateCampusDays < bestCampusDays;
            }

            int candidateMeetingDays = candidateSchedule.Sum(CountMeetingDays);
            int bestMeetingDays = bestSchedule.Sum(CountMeetingDays);
            if (candidateMeetingDays != bestMeetingDays)
            {
                return candidateMeetingDays < bestMeetingDays;
            }

            long candidateStartTotal = candidateSchedule.Sum(lecture =>
                ConvertTo24Hour(lecture.StartTime, lecture.StartAM_PM).Ticks);
            long bestStartTotal = bestSchedule.Sum(lecture =>
                ConvertTo24Hour(lecture.StartTime, lecture.StartAM_PM).Ticks);
            return candidateStartTotal < bestStartTotal;
        }

        private void BuildNotes(List<CourseCandidateGroup> candidateGroups)
        {
            if (scheduleLectures.Count == 0)
            {
                scheduleNotes.Add("No non-overlapping class could be found at all.");
                return;
            }

            int campusDays = CountCampusDays(scheduleLectures);
            string dayText = FormatCampusDays(scheduleLectures);
            string campusDayLabel = campusDays == 1 ? "campus day" : "campus days";
            scheduleNotes.Add($"Min commute schedule uses {campusDays} {campusDayLabel}: {dayText}.");

            foreach (CourseCandidateGroup group in candidateGroups)
            {
                bool courseScheduled = scheduleLectures.Any(lecture => lecture.ClassCode == group.ClassCode);
                if (courseScheduled)
                {
                    continue;
                }

                Lecture lecture = group.Candidates[0];
                scheduleNotes.Add(
                    $"No non-overlapping section could be scheduled for {lecture.ClassCode} - {lecture.ClassName}.");
            }
        }

        private static int CountCampusDays(IEnumerable<Lecture> schedule)
        {
            return schedule
                .SelectMany(lecture => lecture.Days)
                .Distinct()
                .Count();
        }

        private static int CountMeetingDays(Lecture lecture)
        {
            return lecture.Days.Distinct().Count();
        }

        private static string FormatCampusDays(IEnumerable<Lecture> schedule)
        {
            string dayText = ScheduleDisplayHelper.FormatDays(
                schedule
                    .SelectMany(lecture => lecture.Days)
                    .Distinct());

            return string.IsNullOrWhiteSpace(dayText) ? "none" : dayText;
        }
    }
}
