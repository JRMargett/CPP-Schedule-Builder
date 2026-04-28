namespace CPP_Schedule_Builder
{
    internal sealed class EarlyMorningOptimizer : ScheduleOptimizerBase
    {
        public override string Name => "Early Morning Preferred";

        protected override IEnumerable<Lecture> OrderCandidates(IEnumerable<Lecture> candidates)
        {
            return candidates.OrderBy(GetStartTime);
        }

        protected override double GetOptimizationScore(IReadOnlyList<Lecture> schedule)
        {
            int morningClasses = schedule.Count(lecture => GetStartTime(lecture).Hours < 12);
            double startMinuteTotal = schedule.Sum(lecture => GetStartTime(lecture).TotalMinutes);

            return (morningClasses * 10000) - startMinuteTotal;
        }
    }
}
