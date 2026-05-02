using System;
using System.Collections.Generic;
using System.Linq;

namespace CPP_Schedule_Builder
{
    internal abstract class ScheduleTimePreference : Schedule
    {
        private sealed class LectureCandidate
        {
            public LectureCandidate(Lecture lecture, int originalOrder, bool isPreferred, TimeSpan startTime)
            {
                Lecture = lecture;
                OriginalOrder = originalOrder;
                IsPreferred = isPreferred;
                StartTime = startTime;
            }

            public Lecture Lecture { get; }
            public int OriginalOrder { get; }
            public bool IsPreferred { get; }
            public TimeSpan StartTime { get; }
        }

        private sealed class CourseCandidateGroup
        {
            public CourseCandidateGroup(string classCode, int courseOrder, List<LectureCandidate> candidates)
            {
                ClassCode = classCode;
                CourseOrder = courseOrder;
                Candidates = candidates;
            }

            public string ClassCode { get; }
            public int CourseOrder { get; }
            public List<LectureCandidate> Candidates { get; }
        }

        protected abstract string PreferredTimeName { get; }
        protected abstract bool IsPreferredStart(TimeSpan startTime);

        public ScheduleTimePreference()
        {
        }

        public ScheduleTimePreference(IEnumerable<Lecture> selectedLectures)
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
            List<LectureCandidate> bestSchedule = new List<LectureCandidate>();
            List<LectureCandidate> currentSchedule = new List<LectureCandidate>();

            if (candidateGroups.Count == 0)
            {
                scheduleNotes.Add("No classes have been added yet.");
                return false;
            }

            FindBestSchedule(candidateGroups, 0, currentSchedule, ref bestSchedule);

            scheduleLectures.AddRange(
                bestSchedule
                    .OrderBy(candidate => candidateGroups.First(group => group.ClassCode == candidate.Lecture.ClassCode).CourseOrder)
                    .Select(candidate => candidate.Lecture));

            BuildNotes(candidateGroups, bestSchedule);
            return scheduleLectures.Count > 0;
        }

        private List<CourseCandidateGroup> BuildCandidateGroups()
        {
            return Lectures
                .Select((lecture, index) =>
                {
                    TimeSpan startTime = ConvertTo24Hour(lecture.StartTime, lecture.StartAM_PM);
                    return new LectureCandidate(lecture, index, IsPreferredStart(startTime), startTime);
                })
                .GroupBy(candidate => candidate.Lecture.ClassCode)
                .Select((group, courseIndex) => new CourseCandidateGroup(
                    group.Key,
                    courseIndex,
                    group
                        .OrderByDescending(candidate => candidate.IsPreferred)
                        .ThenBy(candidate => candidate.StartTime)
                        .ThenBy(candidate => candidate.OriginalOrder)
                        .ToList()))
                .ToList();
        }

        private void FindBestSchedule(
            List<CourseCandidateGroup> candidateGroups,
            int courseIndex,
            List<LectureCandidate> currentSchedule,
            ref List<LectureCandidate> bestSchedule)
        {
            if (currentSchedule.Count + candidateGroups.Count - courseIndex < bestSchedule.Count)
            {
                return;
            }

            if (courseIndex == candidateGroups.Count)
            {
                if (IsBetterSchedule(currentSchedule, bestSchedule))
                {
                    bestSchedule = new List<LectureCandidate>(currentSchedule);
                }

                return;
            }

            foreach (LectureCandidate candidate in candidateGroups[courseIndex].Candidates)
            {
                bool hasConflict = currentSchedule.Any(selected =>
                    AreConflicting(candidate.Lecture, selected.Lecture));

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

        private static bool IsBetterSchedule(List<LectureCandidate> candidateSchedule, List<LectureCandidate> bestSchedule)
        {
            if (candidateSchedule.Count != bestSchedule.Count)
            {
                return candidateSchedule.Count > bestSchedule.Count;
            }

            int candidatePreferredCount = candidateSchedule.Count(candidate => candidate.IsPreferred);
            int bestPreferredCount = bestSchedule.Count(candidate => candidate.IsPreferred);
            if (candidatePreferredCount != bestPreferredCount)
            {
                return candidatePreferredCount > bestPreferredCount;
            }

            long candidateStartTotal = candidateSchedule.Sum(candidate => candidate.StartTime.Ticks);
            long bestStartTotal = bestSchedule.Sum(candidate => candidate.StartTime.Ticks);
            if (candidateStartTotal != bestStartTotal)
            {
                return candidateStartTotal < bestStartTotal;
            }

            int candidateOriginalOrder = candidateSchedule.Sum(candidate => candidate.OriginalOrder);
            int bestOriginalOrder = bestSchedule.Sum(candidate => candidate.OriginalOrder);
            return candidateOriginalOrder < bestOriginalOrder;
        }

        private void BuildNotes(List<CourseCandidateGroup> candidateGroups, List<LectureCandidate> bestSchedule)
        {
            if (bestSchedule.Count == 0 && candidateGroups.Count > 0)
            {
                scheduleNotes.Add("No non-overlapping class could be found at all.");
            }

            foreach (CourseCandidateGroup group in candidateGroups)
            {
                LectureCandidate? selectedCandidate = bestSchedule.FirstOrDefault(candidate =>
                    candidate.Lecture.ClassCode == group.ClassCode);

                List<LectureCandidate> preferredCandidates = group.Candidates
                    .Where(candidate => candidate.IsPreferred)
                    .ToList();

                if (selectedCandidate == null)
                {
                    AddUnscheduledNote(group, preferredCandidates);
                    continue;
                }

                if (selectedCandidate.IsPreferred)
                {
                    continue;
                }

                if (preferredCandidates.Count == 0)
                {
                    scheduleNotes.Add(
                        $"No {PreferredTimeName} section is available for {FormatCourseLabel(group)}; scheduled {FormatBriefSection(selectedCandidate.Lecture)} instead.");
                }
                else if (!preferredCandidates.Any(candidate => IsNonOverlappingWithFinalSchedule(candidate.Lecture)))
                {
                    scheduleNotes.Add(
                        $"The {PreferredTimeName} sections for {FormatCourseLabel(group)} overlap with the selected schedule; scheduled {FormatBriefSection(selectedCandidate.Lecture)} instead.");
                }
                else
                {
                    scheduleNotes.Add(
                        $"A {PreferredTimeName} section could not be chosen for {FormatCourseLabel(group)}; scheduled {FormatBriefSection(selectedCandidate.Lecture)} instead.");
                }
            }
        }

        private void AddUnscheduledNote(CourseCandidateGroup group, List<LectureCandidate> preferredCandidates)
        {
            if (preferredCandidates.Count == 0)
            {
                scheduleNotes.Add(
                    $"No {PreferredTimeName} section is available for {FormatCourseLabel(group)}, and no non-overlapping fallback section could be scheduled.");
                return;
            }

            if (!preferredCandidates.Any(candidate => IsNonOverlappingWithFinalSchedule(candidate.Lecture)))
            {
                scheduleNotes.Add(
                    $"The {PreferredTimeName} sections for {FormatCourseLabel(group)} overlap with the selected schedule, and no non-overlapping fallback section could be scheduled.");
                return;
            }

            scheduleNotes.Add($"No non-overlapping section could be scheduled for {FormatCourseLabel(group)}.");
        }

        private bool IsNonOverlappingWithFinalSchedule(Lecture lecture)
        {
            return !scheduleLectures.Any(scheduledLecture =>
                scheduledLecture.ClassCode != lecture.ClassCode &&
                AreConflicting(lecture, scheduledLecture));
        }

        private static string FormatCourseLabel(CourseCandidateGroup group)
        {
            Lecture lecture = group.Candidates[0].Lecture;
            return $"{lecture.ClassCode} - {lecture.ClassName}";
        }

        private static string FormatBriefSection(Lecture lecture)
        {
            return $"{ScheduleDisplayHelper.FormatDays(lecture.Days)} {ScheduleDisplayHelper.FormatTimeRange(lecture)}";
        }
    }
}
