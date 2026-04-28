namespace CPP_Schedule_Builder
{
    internal sealed class ScheduleOptimizationResult
    {
        public ScheduleOptimizationResult(IReadOnlyList<Lecture> selectedLectures, IReadOnlyList<string> errors)
        {
            SelectedLectures = selectedLectures;
            Errors = errors;
        }

        public IReadOnlyList<Lecture> SelectedLectures { get; }
        public IReadOnlyList<string> Errors { get; }
        public bool Success => Errors.Count == 0;
    }
}
