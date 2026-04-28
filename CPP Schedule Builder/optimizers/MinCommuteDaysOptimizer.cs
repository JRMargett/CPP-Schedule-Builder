namespace CPP_Schedule_Builder
{
    internal sealed class MinCommuteDaysOptimizer : ScheduleOptimizerBase
    {
        public override string Name => "Min Commute Days";

        protected override double GetOptimizationScore(IReadOnlyList<Lecture> schedule)
        {
            int daysOnCampus = schedule
                .SelectMany(lecture => lecture.Days)
                .Distinct()
                .Count();

            return -daysOnCampus;
        }
    }
}
