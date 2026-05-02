using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPP_Schedule_Builder
{
    internal class ScheduleRMPscore : Schedule
    {
        private sealed class ScoredLecture
        {
            public ScoredLecture(Lecture lecture, double rating, double difficulty, int numRatings)
            {
                Lecture = lecture;
                Rating = rating;
                Difficulty = difficulty;
                NumRatings = numRatings;
            }

            public Lecture Lecture { get; }
            public double Rating { get; }
            public double Difficulty { get; }
            public int NumRatings { get; }
        }

        public ScheduleRMPscore()
        {
        }

        public ScheduleRMPscore(IEnumerable<Lecture> selectedLectures)
        {
            foreach (Lecture lecture in selectedLectures)
            {
                AddLecture(lecture);
            }
        }

        public override bool TryBuildSchedule()
        {
            List<Lecture> rmpSchedule = BuildBestRMPscoreSchedule();
            scheduleLectures.Clear();
            scheduleLectures.AddRange(rmpSchedule);
            return scheduleLectures.Count > 0;
        }

        public bool TryBuildSchedule(IEnumerable<Lecture> selectedLectures)
        {
            List<Lecture> selectedLectureList = selectedLectures.ToList();

            lectures.Clear();
            foreach (Lecture lecture in selectedLectureList)
            {
                AddLecture(lecture);
            }

            return TryBuildSchedule();
        }

        public Lecture? PickBestRatedLecture(IEnumerable<Lecture> selectedLectures)
        {
            return ScoreLectures(selectedLectures)
                .OrderByDescending(x => x.Rating)
                .ThenBy(x => x.Difficulty)
                .ThenByDescending(x => x.NumRatings)
                .ThenBy(x => x.Lecture.ClassID)
                .FirstOrDefault()
                ?.Lecture;
        }

        private List<Lecture> BuildBestRMPscoreSchedule()
        {
            List<ScoredLecture> scoredLectures = ScoreLectures(Lectures);

            Dictionary<string, int> courseOrder = Lectures
                .Select((lecture, index) => new { lecture.ClassCode, index })
                .GroupBy(x => x.ClassCode)
                .ToDictionary(g => g.Key, g => g.Min(x => x.index));

            List<List<ScoredLecture>> candidatesByCourse = scoredLectures
                .GroupBy(x => x.Lecture.ClassCode)
                .Select(g => g
                    .OrderByDescending(x => x.Rating)
                    .ThenBy(x => x.Difficulty)
                    .ThenByDescending(x => x.NumRatings)
                    .ThenBy(x => x.Lecture.ClassID)
                    .ToList())
                .OrderBy(g => g.Count)
                .ThenBy(g => courseOrder[g[0].Lecture.ClassCode])
                .ToList();

            List<ScoredLecture> currentSchedule = new List<ScoredLecture>();
            List<ScoredLecture> bestSchedule = new List<ScoredLecture>();

            FindBestSchedule(candidatesByCourse, 0, currentSchedule, ref bestSchedule);

            return bestSchedule
                .OrderBy(x => courseOrder[x.Lecture.ClassCode])
                .Select(x => x.Lecture)
                .ToList();
        }

        private List<ScoredLecture> ScoreLectures(IEnumerable<Lecture> selectedLectures)
        {
            List<ScoredLecture> scoredLectures = new List<ScoredLecture>();
            Dictionary<string, ProfessorRating> ratingByInstructor =
                new Dictionary<string, ProfessorRating>(StringComparer.OrdinalIgnoreCase);

            foreach (Lecture lecture in selectedLectures)
            {
                double rating = 0;
                double difficulty = 5;
                int numRatings = 0;

                if (!string.IsNullOrWhiteSpace(lecture.Instructor))
                {
                    if (!ratingByInstructor.TryGetValue(lecture.Instructor, out ProfessorRating? professorRating))
                    {
                        try
                        {
                            professorRating = ProfessorRating.GetProfessorRating(lecture.Instructor);
                        }
                        catch
                        {
                            professorRating = new ProfessorRating { Found = false };
                        }

                        ratingByInstructor[lecture.Instructor] = professorRating;
                    }

                    if (professorRating.Found)
                    {
                        rating = professorRating.Rating ?? 0;
                        difficulty = professorRating.Difficulty ?? 5;
                        numRatings = professorRating.NumRatings ?? 0;
                    }
                }

                scoredLectures.Add(new ScoredLecture(lecture, rating, difficulty, numRatings));
            }

            return scoredLectures;
        }

        private void FindBestSchedule(
            List<List<ScoredLecture>> candidatesByCourse,
            int courseIndex,
            List<ScoredLecture> currentSchedule,
            ref List<ScoredLecture> bestSchedule)
        {
            if (currentSchedule.Count + candidatesByCourse.Count - courseIndex < bestSchedule.Count)
            {
                return;
            }

            if (courseIndex == candidatesByCourse.Count)
            {
                if (IsBetterSchedule(currentSchedule, bestSchedule))
                {
                    bestSchedule = new List<ScoredLecture>(currentSchedule);
                }

                return;
            }

            foreach (ScoredLecture candidate in candidatesByCourse[courseIndex])
            {
                bool hasConflict = currentSchedule.Any(selected =>
                    AreConflicting(candidate.Lecture, selected.Lecture));

                if (!hasConflict)
                {
                    currentSchedule.Add(candidate);
                    FindBestSchedule(candidatesByCourse, courseIndex + 1, currentSchedule, ref bestSchedule);
                    currentSchedule.RemoveAt(currentSchedule.Count - 1);
                }
            }

            FindBestSchedule(candidatesByCourse, courseIndex + 1, currentSchedule, ref bestSchedule);
        }

        private static bool IsBetterSchedule(List<ScoredLecture> candidateSchedule, List<ScoredLecture> bestSchedule)
        {
            if (candidateSchedule.Count != bestSchedule.Count)
                return candidateSchedule.Count > bestSchedule.Count;

            double candidateRating = candidateSchedule.Sum(x => x.Rating);
            double bestRating = bestSchedule.Sum(x => x.Rating);
            if (candidateRating != bestRating)
                return candidateRating > bestRating;

            double candidateDifficulty = candidateSchedule.Sum(x => x.Difficulty);
            double bestDifficulty = bestSchedule.Sum(x => x.Difficulty);
            if (candidateDifficulty != bestDifficulty)
                return candidateDifficulty < bestDifficulty;

            int candidateNumRatings = candidateSchedule.Sum(x => x.NumRatings);
            int bestNumRatings = bestSchedule.Sum(x => x.NumRatings);
            return candidateNumRatings > bestNumRatings;
        }
    }
}
