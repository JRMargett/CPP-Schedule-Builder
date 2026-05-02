using System;
using System.Collections.Generic;

namespace CPP_Schedule_Builder
{
    internal class ScheduleAfternoon : ScheduleTimePreference
    {
        private static readonly TimeSpan Noon = new TimeSpan(12, 0, 0);

        public ScheduleAfternoon()
        {
        }

        public ScheduleAfternoon(IEnumerable<Lecture> selectedLectures)
            : base(selectedLectures)
        {
        }

        protected override string PreferredTimeName => "afternoon";

        protected override bool IsPreferredStart(TimeSpan startTime)
        {
            return startTime >= Noon;
        }
    }
}
