using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CPP_Schedule_Builder
{
    internal class Schedule
    {
        protected readonly List<Lecture> lectures = new List<Lecture>();
        protected readonly List<Lecture> scheduleLectures = new List<Lecture>();
        protected readonly List<string> scheduleNotes = new List<string>();
        public IReadOnlyList<Lecture> Lectures => lectures.AsReadOnly();
        public IReadOnlyList<Lecture> ScheduleLectures => scheduleLectures.AsReadOnly();
        public IReadOnlyList<string> ScheduleNotes => scheduleNotes.AsReadOnly();

        public bool AddLecture(Lecture lecture)
        {
            if (lecture == null)
                return false;
            lectures.Add(lecture);
            return true;
        }

        public bool RemoveLecture(int classId)
        {
            Lecture? lectureToRemove = lectures.FirstOrDefault(l => l.ClassID == classId);

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

        protected bool AreConflicting(Lecture a, Lecture b)
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
        public virtual bool TryBuildSchedule()
        {
            scheduleNotes.Clear();
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

        private List<DayOfWeek> GetDays(string days)
        {
            var Days = new List<DayOfWeek>();
            if (days.Contains("M"))
                Days.Add(DayOfWeek.Monday);
            if (days.Contains("T"))
                Days.Add(DayOfWeek.Tuesday);
            if (days.Contains("W"))
                Days.Add(DayOfWeek.Wednesday);
            if (days.Contains("Th"))
                Days.Add(DayOfWeek.Thursday);
            if (days.Contains("F"))
                Days.Add(DayOfWeek.Friday);
            return Days;
        }
        private static string FormatDays(List<DayOfWeek> days)
        {
            var dayStrings = new List<string>();
            foreach (DayOfWeek day in days)
            {
                switch (day)
                {
                    case DayOfWeek.Monday:
                        dayStrings.Add("M");
                        break;
                    case DayOfWeek.Tuesday:
                        dayStrings.Add("T");
                        break;
                    case DayOfWeek.Wednesday:
                        dayStrings.Add("W");
                        break;
                    case DayOfWeek.Thursday:
                        dayStrings.Add("Th");
                        break;
                    case DayOfWeek.Friday:
                        dayStrings.Add("F");
                        break;
                }
            }
            return string.Join("", dayStrings);
        }

        public void ImportSchedule(string filePath)
        {
            if (!File.Exists(filePath))
                return;
            foreach (string line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                string[] parts = line.Split(',');
                if (parts.Length != 9)
                    continue;
                Lecture lecture = new Lecture(int.Parse(parts[0]), parts[1], parts[2], parts[3], GetDays(parts[4]), TimeSpan.Parse(parts[5]), parts[6], TimeSpan.Parse(parts[7]), parts[8]);
                AddLecture(lecture);
            }
            return;
        }
        public void ExportSchedule(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Lecture lecture in lectures)
            {
                string days = FormatDays(lecture.Days);
                sb.AppendLine($"{lecture.ClassID},{lecture.ClassName},{lecture.ClassCode},{lecture.Instructor},{days},{lecture.StartTime},{lecture.StartAM_PM},{lecture.EndTime},{lecture.EndAM_PM}");
            }
            File.WriteAllText(filePath, sb.ToString());
        }
        public bool BuildMinCommuteSchedule()
        {
            ScheduleMinCommute minCommuteSchedule = new ScheduleMinCommute(Lectures);
            bool scheduleBuilt = minCommuteSchedule.TryBuildSchedule();

            scheduleNotes.Clear();
            scheduleLectures.Clear();
            scheduleLectures.AddRange(minCommuteSchedule.ScheduleLectures);
            scheduleNotes.AddRange(minCommuteSchedule.ScheduleNotes);

            return scheduleBuilt;
        }
        public bool BuildMorningSchedule()
        {
            ScheduleMorning morningSchedule = new ScheduleMorning(Lectures);
            bool scheduleBuilt = morningSchedule.TryBuildSchedule();

            scheduleLectures.Clear();
            scheduleLectures.AddRange(morningSchedule.ScheduleLectures);
            scheduleNotes.Clear();
            scheduleNotes.AddRange(morningSchedule.ScheduleNotes);

            return scheduleBuilt;
        }
        public bool BuildAfternoonSchedule()
        {
            ScheduleAfternoon afternoonSchedule = new ScheduleAfternoon(Lectures);
            bool scheduleBuilt = afternoonSchedule.TryBuildSchedule();

            scheduleLectures.Clear();
            scheduleLectures.AddRange(afternoonSchedule.ScheduleLectures);
            scheduleNotes.Clear();
            scheduleNotes.AddRange(afternoonSchedule.ScheduleNotes);

            return scheduleBuilt;
        }
        public bool BuildRMPscoreSchedule()
        {
            ScheduleRMPscore rmpSchedule = new ScheduleRMPscore(Lectures);
            bool scheduleBuilt = rmpSchedule.TryBuildSchedule();
            scheduleLectures.Clear();
            scheduleLectures.AddRange(rmpSchedule.ScheduleLectures);
            scheduleNotes.Clear();

            return scheduleBuilt;
        }
    }
}

