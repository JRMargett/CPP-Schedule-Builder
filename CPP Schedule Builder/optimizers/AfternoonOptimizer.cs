namespace CPP_Schedule_Builder
{
    internal sealed class AfternoonOptimizer : ScheduleOptimizerBase
    {
        public override string Name => "Afternoon Preferred";

        protected override IEnumerable<Lecture> OrderCandidates(IEnumerable<Lecture> candidates)
        {
            return candidates.OrderByDescending(GetStartTime);
        }

        protected override double GetOptimizationScore(IReadOnlyList<Lecture> schedule)
        {
            int afternoonClasses = schedule.Count(lecture => GetStartTime(lecture).Hours >= 12);
            double startMinuteTotal = schedule.Sum(lecture => GetStartTime(lecture).TotalMinutes);

            return (afternoonClasses * 10000) + startMinuteTotal;
        }
    }
}
