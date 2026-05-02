using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CPP_Schedule_Builder
{
    internal static class ScheduleDisplayHelper
    {
        private static readonly TimeSpan DefaultScheduleStart = new TimeSpan(7, 0, 0);
        private static readonly TimeSpan DefaultScheduleEnd = new TimeSpan(22, 0, 0);
        private static readonly TimeSpan TimeBlockSize = TimeSpan.FromMinutes(30);

        private static readonly Color[] ClassColors =
        {
            Color.FromArgb(187, 222, 251),
            Color.FromArgb(200, 230, 201),
            Color.FromArgb(255, 224, 178),
            Color.FromArgb(248, 187, 208),
            Color.FromArgb(209, 196, 233),
            Color.FromArgb(178, 235, 242),
            Color.FromArgb(255, 245, 157),
            Color.FromArgb(215, 204, 200),
            Color.FromArgb(197, 225, 165),
            Color.FromArgb(255, 204, 188)
        };

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

            ConfigureTimeBlockGrid(grid);
            grid.Rows.Clear();

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

            TimeSpan scheduleStart = GetScheduleStart(schedule);
            TimeSpan scheduleEnd = GetScheduleEnd(schedule);
            List<TimeSpan> timeBlocks = BuildTimeBlocks(scheduleStart, scheduleEnd);

            foreach (TimeSpan timeBlock in timeBlocks)
            {
                int rowIndex = grid.Rows.Add();
                DataGridViewRow row = grid.Rows[rowIndex];
                row.Height = 34;
                row.HeaderCell.Value = FormatBlockHeader(timeBlock);
            }

            Dictionary<string, Color> colorByClassCode = BuildClassColorMap(schedule.ScheduleLectures);

            foreach (Lecture lecture in schedule.ScheduleLectures)
            {
                TimeSpan lectureStart = schedule.ConvertTo24Hour(lecture.StartTime, lecture.StartAM_PM);
                TimeSpan lectureEnd = schedule.ConvertTo24Hour(lecture.EndTime, lecture.EndAM_PM);
                Color classColor = colorByClassCode[lecture.ClassCode];

                foreach (DayOfWeek day in lecture.Days)
                {
                    int columnIndex = Array.IndexOf(dayOrder, day);
                    if (columnIndex < 0)
                    {
                        continue;
                    }

                    FillLectureBlock(
                        grid,
                        timeBlocks,
                        columnIndex,
                        lecture,
                        lectureStart,
                        lectureEnd,
                        classColor);
                }
            }
        }

        private static void ConfigureTimeBlockGrid(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.BackgroundColor = Color.White;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.Black;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 80, 48);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = Color.FromArgb(210, 210, 210);
            grid.ReadOnly = true;
            grid.RowHeadersVisible = true;
            grid.RowHeadersWidth = 76;
            grid.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 48);
            grid.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ScrollBars = ScrollBars.Both;
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;

            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Width = 76;
            }
        }

        private static TimeSpan GetScheduleStart(Schedule schedule)
        {
            if (!schedule.ScheduleLectures.Any())
            {
                return DefaultScheduleStart;
            }

            TimeSpan earliestStart = schedule.ScheduleLectures
                .Min(lecture => schedule.ConvertTo24Hour(lecture.StartTime, lecture.StartAM_PM));

            return FloorToTimeBlock(earliestStart < DefaultScheduleStart ? earliestStart : DefaultScheduleStart);
        }

        private static TimeSpan GetScheduleEnd(Schedule schedule)
        {
            if (!schedule.ScheduleLectures.Any())
            {
                return DefaultScheduleEnd;
            }

            TimeSpan latestEnd = schedule.ScheduleLectures
                .Max(lecture => schedule.ConvertTo24Hour(lecture.EndTime, lecture.EndAM_PM));

            return CeilingToTimeBlock(latestEnd > DefaultScheduleEnd ? latestEnd : DefaultScheduleEnd);
        }

        private static List<TimeSpan> BuildTimeBlocks(TimeSpan start, TimeSpan end)
        {
            List<TimeSpan> blocks = new List<TimeSpan>();

            for (TimeSpan time = start; time < end; time = time.Add(TimeBlockSize))
            {
                blocks.Add(time);
            }

            return blocks;
        }

        private static void FillLectureBlock(
            DataGridView grid,
            List<TimeSpan> timeBlocks,
            int columnIndex,
            Lecture lecture,
            TimeSpan lectureStart,
            TimeSpan lectureEnd,
            Color classColor)
        {
            int firstLectureRow = -1;

            for (int rowIndex = 0; rowIndex < timeBlocks.Count; rowIndex++)
            {
                TimeSpan blockStart = timeBlocks[rowIndex];
                TimeSpan blockEnd = blockStart.Add(TimeBlockSize);

                if (blockStart >= lectureEnd || lectureStart >= blockEnd)
                {
                    continue;
                }

                DataGridViewCell cell = grid.Rows[rowIndex].Cells[columnIndex];
                cell.Style.BackColor = classColor;
                cell.Style.SelectionBackColor = Darken(classColor);
                cell.ToolTipText = BuildLectureTooltip(lecture);

                if (firstLectureRow == -1)
                {
                    firstLectureRow = rowIndex;
                    cell.Value = BuildLectureGridText(lecture);
                }
            }
        }

        private static Dictionary<string, Color> BuildClassColorMap(IEnumerable<Lecture> lectures)
        {
            Dictionary<string, Color> colorByClassCode = new Dictionary<string, Color>();

            foreach (string classCode in lectures.Select(lecture => lecture.ClassCode).Distinct())
            {
                colorByClassCode[classCode] = ClassColors[colorByClassCode.Count % ClassColors.Length];
            }

            return colorByClassCode;
        }

        private static string BuildLectureGridText(Lecture lecture)
        {
            return $"{lecture.ClassCode}\n{FormatTimeRange(lecture)}";
        }

        private static string BuildLectureTooltip(Lecture lecture)
        {
            return $"{lecture.ClassID} | {lecture.ClassCode} | {lecture.ClassName} | {lecture.Instructor} | {FormatDays(lecture.Days)} | {FormatTimeRange(lecture)}";
        }

        private static TimeSpan FloorToTimeBlock(TimeSpan time)
        {
            long ticks = time.Ticks - time.Ticks % TimeBlockSize.Ticks;
            return new TimeSpan(ticks);
        }

        private static TimeSpan CeilingToTimeBlock(TimeSpan time)
        {
            TimeSpan flooredTime = FloorToTimeBlock(time);
            return flooredTime == time ? time : flooredTime.Add(TimeBlockSize);
        }

        private static string FormatBlockHeader(TimeSpan time)
        {
            return DateTime.Today.Add(time).ToString("h:mm tt");
        }

        private static Color Darken(Color color)
        {
            return Color.FromArgb(
                Math.Max(color.R - 45, 0),
                Math.Max(color.G - 45, 0),
                Math.Max(color.B - 45, 0));
        }

        public static string FormatTime(TimeSpan time, string amPm)
        {
            return $"{time.Hours:D2}:{time.Minutes:D2} {amPm}";
        }

        public static string FormatTimeRange(Lecture lecture)
        {
            return $"{FormatTime(lecture.StartTime, lecture.StartAM_PM)} - {FormatTime(lecture.EndTime, lecture.EndAM_PM)}";
        }

        public static string FormatDays(IEnumerable<DayOfWeek> days)
        {
            DayOfWeek[] dayOrder =
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };

            return string.Join(", ", days
                .OrderBy(day => Array.IndexOf(dayOrder, day))
                .Select(day => day.ToString()));
        }
    }
}
