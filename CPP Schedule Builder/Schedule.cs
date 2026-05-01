using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPP_Schedule_Builder
{
    internal class Schedule
    {
        private readonly List<Lecture> lectures = new List<Lecture>();
        private readonly List<Lecture> scheduleLectures = new List<Lecture>();
        public IReadOnlyList<Lecture> Lectures => lectures.AsReadOnly();
        public IReadOnlyList<Lecture> ScheduleLectures=> scheduleLectures.AsReadOnly();    

        public bool AddLecture(Lecture lecture)
        {
            if (lecture == null)
                return false;
            lectures.Add(lecture);
            return true;
        }

        public bool RemoveLecture(int classId)
        {
            Lecture lectureToRemove = lectures.FirstOrDefault(l => l.ClassID == classId);

            if (lectureToRemove == null)
                return false;

            lectures.Remove(lectureToRemove);
            return true;
        }


        public bool HasConflict(Lecture newLecture)
        {
            foreach (Lecture existingLecture in scheduleLectures)
            {
                if (existingLecture == newLecture)
                    continue;
                if (AreConflicting(existingLecture, newLecture))
                        return true;
                if (existingLecture.ClassCode == newLecture.ClassCode)
                    return true;
            }
            return false;
        }

        private bool AreConflicting(Lecture a, Lecture b)
        {
            List<DayOfWeek> sharedDays = a.Days.Intersect(b.Days).ToList();

            if (!sharedDays.Any())
                return false;

            TimeSpan aStart = ConvertTo24Hour(a.StartTime, a.StartAM_PM);
            TimeSpan aEnd = ConvertTo24Hour(a.EndTime, a.EndAM_PM);
            TimeSpan bStart = ConvertTo24Hour(b.StartTime, b.StartAM_PM);
            TimeSpan bEnd = ConvertTo24Hour(b.EndTime, b.EndAM_PM);

            return aStart < bEnd && bStart < aEnd;
        }

        public TimeSpan ConvertTo24Hour(TimeSpan time, string amPm)
        {
            int hour = time.Hours;
            int minute = time.Minutes;

            string marker = amPm.Trim().ToUpper();

            if (marker == "PM" && hour != 12)
                hour += 12;
            else if (marker == "AM" && hour == 12)
                hour = 0;

            return new TimeSpan(hour, minute, 0);
        }

        public string GetScheduleText()
        {
            if (!lectures.Any())
                return "No classes added yet.";

            StringBuilder sb = new StringBuilder();

            DayOfWeek[] order =
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };

            foreach (DayOfWeek day in order)
            {
                sb.AppendLine(day + ":");

                List<Lecture> dayLectures = lectures
                    .Where(l => l.Days.Contains(day))
                    .OrderBy(l => ConvertTo24Hour(l.StartTime, l.StartAM_PM))
                    .ToList();

                if (!dayLectures.Any())
                {
                    sb.AppendLine("  No classes");
                }
                else
                {
                    foreach (Lecture lecture in dayLectures)
                    {
                        sb.AppendLine(
                            $"  {lecture.ClassCode} - {lecture.ClassName} | " +
                            $"{FormatTime(lecture.StartTime, lecture.StartAM_PM)} - {FormatTime(lecture.EndTime, lecture.EndAM_PM)} | " +
                            $"{lecture.Instructor}"
                        );
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string FormatTime(TimeSpan time, string amPm)
        {
            return $"{time.Hours:D2}:{time.Minutes:D2} {amPm}";
        }
        public bool TryBuildSchedule()
        {
            scheduleLectures.Clear();
            foreach (Lecture lecture in lectures)
            {
                if (!HasConflict(lecture))
                {
                    scheduleLectures.Add(lecture);
                }
            }

            return scheduleLectures.Count > 0;
        }

    }
}
