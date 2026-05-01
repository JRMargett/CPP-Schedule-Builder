using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CPP_Schedule_Builder
{
    internal static class ScheduleDisplayHelper
    {
        public static string BuildDisplayText(Schedule schedule)
        {
            if (schedule == null)
                return "Schedule is not available.";

            return schedule.GetScheduleText();
        }

        public static void LoadScheduleIntoGrid(DataGridView grid, Schedule schedule)
        {
            if (grid == null || schedule == null)
                return;

            grid.Rows.Clear();

            int maxRows = Math.Max(schedule.ScheduleLectures.Count, 1);
            for (int i = 0; i < maxRows; i++)
            {
                grid.Rows.Add();
            }

            DayOfWeek[] dayOrder =
            {
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday
            };

            for (int col = 0; col < dayOrder.Length; col++)
            {
                DayOfWeek currentDay = dayOrder[col];

                var dayLectures = schedule.ScheduleLectures
                    .Where(l => l.Days.Contains(currentDay))
                    .OrderBy(l => schedule.ConvertTo24Hour(l.StartTime, l.StartAM_PM))
                    .ToList();

                for (int row = 0; row < dayLectures.Count; row++)
                {
                    Lecture lecture = dayLectures[row];

                    grid.Rows[row].Cells[col].Value =
                        $"{lecture.ClassCode}\n" +
                        $"{lecture.ClassName}\n" +
                        $"{FormatTime(lecture.StartTime, lecture.StartAM_PM)} - {FormatTime(lecture.EndTime, lecture.EndAM_PM)}\n" +
                        $"{lecture.Instructor}";
                }
            }

            foreach (DataGridViewRow row in grid.Rows)
            {
                row.Height = 75;
            }

            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private static string FormatTime(TimeSpan time, string amPm)
        {
            return $"{time.Hours:D2}:{time.Minutes:D2} {amPm}";
        }
    }
}
