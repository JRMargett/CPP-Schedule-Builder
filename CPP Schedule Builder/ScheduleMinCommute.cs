using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPP_Schedule_Builder
{
    internal class TrafficWindow
    {
        public string Label { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public TrafficWindow(string label, string start, string end)
        {
            Label = label;
            Start = TimeSpan.Parse(start);
            End = TimeSpan.Parse(end);
        }
        public bool Contains(TimeSpan time) => time >= Start && time <= End;
    }
    internal class ScheduleMinCommute:Schedule
    {
        private static readonly List<TrafficWindow> TrafficWindows = new List<TrafficWindow>
        {
            new TrafficWindow("Morning Rush Hour", "07:00", "09:00"),
            new TrafficWindow("Evening Rush Hour", "16:00", "18:00")
        };

        private const int Buffer = 30;
        public static bool CheckTraffic(string startTimeStr, string endTimeStr)
        {
            var startTime = TimeSpan.Parse(startTimeStr);
            var endTime = TimeSpan.Parse(endTimeStr);
            foreach (var window in TrafficWindows)
            {
                if (window.Contains(endTime) || window.Contains(startTime))
                {
                    return true;
                }
                var bufferEdge = window.Start - TimeSpan.FromMinutes(Buffer);
                var bufferEdge2 = window.End + TimeSpan.FromMinutes(Buffer);
                if ((endTime >= bufferEdge && endTime < window.Start) || (startTime >= bufferEdge2 && startTime < window.Start))
                    return true;
            }
            return false;
        }

    }
}
