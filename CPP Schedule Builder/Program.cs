using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPP_Schedule_Builder
{
    internal class Lecture
    {
        public int ClassID {get; set;}
        public string ClassName {get; set; }
        public string ClassCode {get; set; }
        public string Subject { get; set; }
        public string Instructor { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public TimeSpan StartTime { get; set; }
        public string StartAM_PM { get; set; }
        public string EndAM_PM { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Color { get; set; }
        public Lecture(int classID, string subject, string className, string classCode, string instructor, List<DayOfWeek> days, TimeSpan startTime, string startAM_PM, TimeSpan endTime, string endAM_PM)
        {
            ClassID = classID;
            Subject = subject;
            ClassName = className;
            ClassCode = classCode;
            Instructor = instructor;
            Days = days;
            StartTime = startTime;
            StartAM_PM = startAM_PM;
            EndTime = endTime;
            EndAM_PM = endAM_PM;
        }

        private string FormatDays()
        {
            if (Days == null || Days.Count == 0) return "";

            var names = new List<string>();
            foreach (var d in Days)
            {
                switch (d)
                {
                    case DayOfWeek.Monday: names.Add("Mon"); break;
                    case DayOfWeek.Tuesday: names.Add("Tue"); break;
                    case DayOfWeek.Wednesday: names.Add("Wed"); break;
                    case DayOfWeek.Thursday: names.Add("Thu"); break;
                    case DayOfWeek.Friday: names.Add("Fri"); break;
                    case DayOfWeek.Saturday: names.Add("Sat"); break;
                    case DayOfWeek.Sunday: names.Add("Sun"); break;
                }
            }
            return string.Join("/", names);
        }

        private string FormatTime(TimeSpan t, string ampm)
        {
            int hour = t.Hours;
            int displayHour = hour == 0 ? 12 : hour; 

            return $"{displayHour}:{t.Minutes:00} {ampm}";
        }

        public override string ToString()
        {
            string idPart = ClassID.ToString();
            string subjPart = string.IsNullOrWhiteSpace(Subject) ? "" : Subject + " ";
            string codePart = ClassCode.ToString();
            string daysPart = FormatDays();
            string timePart = FormatTime(StartTime, StartAM_PM) + " - " + FormatTime(EndTime, EndAM_PM);
            string instrPart = string.IsNullOrWhiteSpace(Instructor) ? "" : Instructor;

            return $"{idPart} - {subjPart}{codePart} | {daysPart} {timePart} | {instrPart}".Trim();
        }
    }

    
}
