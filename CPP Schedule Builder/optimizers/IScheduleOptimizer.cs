namespace CPP_Schedule_Builder
{
    internal interface IScheduleOptimizer
    {
        string Name { get; }

        ScheduleOptimizationResult Optimize(IEnumerable<Lecture> candidates);
    }
}
