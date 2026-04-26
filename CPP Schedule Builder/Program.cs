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
        public int ClassCode {get; set; }
        public string Instructor { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public TimeSpan StartTime { get; set; }
        public string StartAM_PM { get; set; }
        public string EndAM_PM { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Color { get; set; }
        public Lecture(int classID, string className, int classCode, string instructor, List<DayOfWeek> days, TimeSpan startTime, string startAM_PM, TimeSpan endTime, string endAM_PM)
            {
                ClassID = classID;
                ClassName = className;
                ClassCode = classCode;
                Instructor = instructor;
                Days = days;
                StartTime = startTime;
                StartAM_PM = startAM_PM;
                EndTime = endTime;
                EndAM_PM = endAM_PM;
            }
    }

    
}
