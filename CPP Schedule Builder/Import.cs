using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPP_Schedule_Builder
{
    internal class Import
    {
        private List<DayOfWeek> GetDays(string days)
        {
            var Days = new List<DayOfWeek>();
            if (days.Contains("Mon"))
                Days.Add(DayOfWeek.Monday);
            if (days.Contains("Tue"))
                Days.Add(DayOfWeek.Tuesday);
            if (days.Contains("Wed"))
                Days.Add(DayOfWeek.Wednesday);
            if (days.Contains("Thu"))
                Days.Add(DayOfWeek.Thursday);
            if (days.Contains("Fri"))
                Days.Add(DayOfWeek.Friday);
            return Days;
        }
        public bool ImportSchedule(string filePath, Schedule schedule)
        {
            if (!File.Exists(filePath))
                return false;
            foreach (string line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                string[] parts = line.Split(',');
                if (parts.Length != 8)
                    continue;
                Lecture lecture = new Lecture(int.Parse(parts[0]), parts[1], parts[2], parts[3], GetDays(parts[4]), TimeSpan.Parse(parts[5]), parts[6], TimeSpan.Parse(parts[7]), parts[8]);
                schedule.AddLecture(lecture);
            }
            ;

        }
    }
}
