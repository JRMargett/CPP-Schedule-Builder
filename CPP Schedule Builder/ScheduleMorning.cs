using System;
using System.Collections.Generic;

namespace CPP_Schedule_Builder
{
    internal class ScheduleMorning : ScheduleTimePreference
    {
        private static readonly TimeSpan Noon = new TimeSpan(12, 0, 0);

        public ScheduleMorning()
        {
        }

        public ScheduleMorning(IEnumerable<Lecture> selectedLectures): base(selectedLectures)
        {
        }

        protected override string PreferredTimeName => "morning";

        protected override bool IsPreferredStart(TimeSpan startTime)
        {
            return startTime < Noon;
        }
    }
}
