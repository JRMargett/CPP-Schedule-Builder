namespace CPP_Schedule_Builder
{
    internal sealed class RateMyProfessorOptimizer : ScheduleOptimizerBase
    {
        public override string Name => "Rate My Professor Score";

        protected override IEnumerable<Lecture> OrderCandidates(IEnumerable<Lecture> candidates)
        {
            return candidates
                .OrderByDescending(GetProfessorRating)
                .ThenByDescending(lecture => lecture.RateMyProfessorRatingsCount ?? 0);
        }

        protected override double GetOptimizationScore(IReadOnlyList<Lecture> schedule)
        {
            return schedule.Sum(GetProfessorRating);
        }

        private static double GetProfessorRating(Lecture lecture)
        {
            return lecture.RateMyProfessorScore ?? 0;
        }
    }
}
