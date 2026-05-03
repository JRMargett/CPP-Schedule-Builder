using System.Drawing.Text;
using System.Drawing.Printing;
using System.Text.Json;
using System.IO;
using System.Text;

namespace CPP_Schedule_Builder
{
    public partial class Form1 : Form
    {

        private const string CourseDataFileName = "courses.json";

        private Dictionary<string, string[]> CoursesBySubject = LoadCoursesBySubject();

        private static Dictionary<string, string[]> LoadCoursesBySubject()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Data", CourseDataFileName);

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, string[]>>(File.ReadAllText(path)) ?? new Dictionary<string, string[]>();
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
            {
                MessageBox.Show(
                    $"Unable to load course data from {path}.{Environment.NewLine}{ex.Message}",
                    "Course Data Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return new Dictionary<string, string[]>();
            }
        }

        private Schedule studentSchedule = new Schedule();
        private int printScheduleRowIndex;
        public Form1()
        {
            InitializeComponent();
            clearButton.Click += clearButton_Click;
            ConfigureLectureDisplay();
            ConfigureScheduleDetailsDisplay();
        }

        private bool isUpdatingLectureDisplay;

        private void ConfigureLectureDisplay()
        {
            LectureDisplay.HideSelection = false;
            LectureDisplay.WordWrap = false;
            LectureDisplay.SelectionChanged += LectureDisplay_SelectionChanged;
        }

        private void ConfigureScheduleDetailsDisplay()
        {
            richTextBox2.ScrollBars = RichTextBoxScrollBars.Both;
            richTextBox2.WordWrap = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CourseSubject.SelectedIndex = -1;
            CourseNumber.DropDownStyle = ComboBoxStyle.DropDownList;
            CourseSubject.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void CourseNumber_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CourseSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            CourseNumber.Items.Clear();
            CourseNumber.Enabled = false;

            if (CourseSubject.SelectedItem is not string selectedSubject)
            {
                return;
            }

            if (CoursesBySubject.TryGetValue(selectedSubject, out string[]? courses))
            {
                for (int i = 0; i < courses.Length; i++)
                {
                    CourseNumber.Items.Add(courses[i]);
                }

                CourseNumber.Enabled = courses.Length > 0;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click_1(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (CourseSubject.SelectedItem is not string selectedSubject)
            {
                richTextBox1.Text = "Please select a subject.";
                return;
            }

            if (CourseNumber.SelectedItem is not string input)
            {
                richTextBox1.Text = "Please select a course number.";
                return;
            }

            if (StartAM_PMCB.SelectedItem is not string startAM_PM ||
                EndAM_PMCB.SelectedItem is not string endAM_PM)
            {
                richTextBox1.Text = "Please select AM or PM for both start and end times.";
                return;
            }

            if (string.IsNullOrWhiteSpace(ClassIDTB.Text) ||
                string.IsNullOrWhiteSpace(InstructorTB.Text) ||
                string.IsNullOrWhiteSpace(StartTimeHr.Text) ||
                string.IsNullOrWhiteSpace(StartTimeMin.Text) ||
                string.IsNullOrWhiteSpace(EndTimeHr.Text) ||
                string.IsNullOrWhiteSpace(EndTimeMin.Text))
            {
                richTextBox1.Text = "Please fill in all required fields.";
                return;
            }

            string[] parts = input.Split('-', 2);
            if (parts.Length != 2)
            {
                richTextBox1.Text = "Selected course format is invalid.";
                return;
            }

            string code = $"{selectedSubject} {parts[0].Trim()}";
            string name = parts[1].Trim();

            var days = new List<DayOfWeek>();
            if (DayCheckBox.GetItemChecked(0)) days.Add(DayOfWeek.Monday);
            if (DayCheckBox.GetItemChecked(1)) days.Add(DayOfWeek.Tuesday);
            if (DayCheckBox.GetItemChecked(2)) days.Add(DayOfWeek.Wednesday);
            if (DayCheckBox.GetItemChecked(3)) days.Add(DayOfWeek.Thursday);
            if (DayCheckBox.GetItemChecked(4)) days.Add(DayOfWeek.Friday);

            if (days.Count == 0)
            {
                richTextBox1.Text = "Please select at least one day.";
                return;
            }

            TimeSpan start = new TimeSpan(int.Parse(StartTimeHr.Text), int.Parse(StartTimeMin.Text), 0);
            TimeSpan end = new TimeSpan(int.Parse(EndTimeHr.Text), int.Parse(EndTimeMin.Text), 0);

            Lecture lecture = new Lecture(
                int.Parse(ClassIDTB.Text),
                name,
                code,
                InstructorTB.Text,
                days,
                start,
                startAM_PM,
                end,
                endAM_PM
            );


            if (studentSchedule.AddLecture(lecture))
            {
                AddLectureToDisplay(lecture);
            }

            ClassIDTB.Clear();
            InstructorTB.Clear();
            StartTimeHr.Clear();
            StartTimeMin.Clear();
            EndTimeHr.Clear();
            EndTimeMin.Clear();
            StartAM_PMCB.SelectedIndex = -1;
            EndAM_PMCB.SelectedIndex = -1;
            DayCheckBox.ClearSelected();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void LectureDisplay_SelectionChanged(object? sender, EventArgs e)
        {
            DisplaySelectedLectureDetails();
        }

        private void DisplaySelectedLectureDetails()
        {
            if (isUpdatingLectureDisplay)
            {
                return;
            }

            Lecture? selectedLecture = GetSelectedLectureFromDisplay();
            if (selectedLecture != null)
            {
                DisplayLectureDetails(selectedLecture);
            }
        }

        private Lecture? GetSelectedLectureFromDisplay()
        {
            if (LectureDisplay.TextLength == 0)
            {
                return null;
            }

            int lineIndex = LectureDisplay.GetLineFromCharIndex(LectureDisplay.SelectionStart);
            if (lineIndex >= 0 && lineIndex < studentSchedule.Lectures.Count)
            {
                return studentSchedule.Lectures[lineIndex];
            }

            return null;
        }

        private void AddLectureToDisplay(Lecture lecture)
        {
            isUpdatingLectureDisplay = true;

            try
            {
                if (LectureDisplay.TextLength > 0)
                {
                    LectureDisplay.AppendText(Environment.NewLine);
                }

                LectureDisplay.AppendText(FormatLectureDisplayLine(lecture));
            }
            finally
            {
                isUpdatingLectureDisplay = false;
            }
        }

        private void RefreshLectureDisplay()
        {
            isUpdatingLectureDisplay = true;

            try
            {
                LectureDisplay.Clear();

                foreach (Lecture lecture in studentSchedule.Lectures)
                {
                    if (LectureDisplay.TextLength > 0)
                    {
                        LectureDisplay.AppendText(Environment.NewLine);
                    }

                    LectureDisplay.AppendText(FormatLectureDisplayLine(lecture));
                }
            }
            finally
            {
                isUpdatingLectureDisplay = false;
            }
        }

        private static string FormatLectureDisplayLine(Lecture lecture)
        {
            string classType = GetClassType(lecture);
            string className = GetClassNameWithoutType(lecture.ClassName, classType);
            string professorName = string.IsNullOrWhiteSpace(lecture.Instructor)
                ? "TBA"
                : lecture.Instructor.Trim();

            return $"{lecture.ClassID} | {lecture.ClassCode} | {className} | Prof. {professorName} | {FormatCompactDays(lecture.Days)} | {FormatCompactTimeRange(lecture)}";
        }

        private void DisplayLectureDetails(Lecture lecture)
        {
            ClearScheduleDetailsDisplay();
            AddLectureDetailsToDisplay(lecture, new Dictionary<string, ProfessorRating>(StringComparer.OrdinalIgnoreCase));
        }

        private void DisplayChosenScheduleDetails()
        {
            ClearScheduleDetailsDisplay();
            Dictionary<string, ProfessorRating> ratingByInstructor =
                new Dictionary<string, ProfessorRating>(StringComparer.OrdinalIgnoreCase);

            foreach (Lecture lecture in studentSchedule.ScheduleLectures)
            {
                AddLectureDetailsToDisplay(lecture, ratingByInstructor);
            }
        }

        private void ClearScheduleDetailsDisplay()
        {
            richTextBox2.Clear();
        }

        private void AddScheduleDetailLine(string text)
        {
            richTextBox2.AppendText(text);
            richTextBox2.AppendText(Environment.NewLine);
        }

        private void AddLectureDetailsToDisplay(Lecture lecture, Dictionary<string, ProfessorRating> ratingByInstructor)
        {
            if (lecture == null)
                return;

            string classType = GetClassType(lecture);
            string className = GetClassNameWithoutType(lecture.ClassName, classType);
            string rmsScore = GetRmpScoreText(lecture, ratingByInstructor);
            string professorName = string.IsNullOrWhiteSpace(lecture.Instructor)
                ? "TBA"
                : lecture.Instructor.Trim();

            AddScheduleDetailLine(
                $"{lecture.ClassID} | {lecture.ClassCode} | {className} | Prof. {professorName} | RMS{{{rmsScore}}} | {FormatCompactDays(lecture.Days)} | {FormatCompactTimeRange(lecture)}");
        }

        private static string GetClassType(Lecture lecture)
        {
            string className = lecture.ClassName.Trim();
            string classCode = lecture.ClassCode.Trim();
            string[] knownTypes =
            {
                "Service Learning Activity",
                "Recitation Activity",
                "Laboratory",
                "Discussion",
                "Recitation",
                "Activity",
                "Seminar",
                "Lecture",
                "Studio",
                "Lab"
            };

            foreach (string knownType in knownTypes)
            {
                if (className.EndsWith(" " + knownType, StringComparison.OrdinalIgnoreCase))
                {
                    return knownType == "Lab" ? "Laboratory" : knownType;
                }
            }

            if (classCode.EndsWith("L", StringComparison.OrdinalIgnoreCase))
            {
                return "Laboratory";
            }

            if (classCode.EndsWith("A", StringComparison.OrdinalIgnoreCase))
            {
                return "Activity";
            }

            return "Lecture";
        }

        private static string GetClassNameWithoutType(string className, string classType)
        {
            string trimmedClassName = className.Trim();
            if (trimmedClassName.EndsWith(" " + classType, StringComparison.OrdinalIgnoreCase))
            {
                return trimmedClassName[..^classType.Length].Trim();
            }

            if (classType == "Laboratory" &&
                trimmedClassName.EndsWith(" Lab", StringComparison.OrdinalIgnoreCase))
            {
                return trimmedClassName[..^"Lab".Length].Trim();
            }

            return trimmedClassName;
        }

        private string GetRmpScoreText(Lecture lecture, Dictionary<string, ProfessorRating> ratingByInstructor)
        {
            if (string.IsNullOrWhiteSpace(lecture.Instructor))
            {
                return "no instructor";
            }

            if (!ratingByInstructor.TryGetValue(lecture.Instructor, out ProfessorRating? professorRating))
            {
                try
                {
                    professorRating = ProfessorRating.GetProfessorRating(lecture.Instructor);
                }
                catch
                {
                    professorRating = new ProfessorRating { Found = false };
                }

                ratingByInstructor[lecture.Instructor] = professorRating;
            }

            if (!professorRating.Found || !professorRating.Rating.HasValue)
            {
                return "not found";
            }

            return professorRating.Rating.Value.ToString("F1");
        }

        private static string FormatCompactDays(IEnumerable<DayOfWeek> days)
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

            Dictionary<DayOfWeek, string> dayNames = new Dictionary<DayOfWeek, string>
            {
                [DayOfWeek.Monday] = "Mo",
                [DayOfWeek.Tuesday] = "Tu",
                [DayOfWeek.Wednesday] = "We",
                [DayOfWeek.Thursday] = "Th",
                [DayOfWeek.Friday] = "Fr",
                [DayOfWeek.Saturday] = "Sa",
                [DayOfWeek.Sunday] = "Su"
            };

            return string.Join(" ", days
                .OrderBy(day => Array.IndexOf(dayOrder, day))
                .Select(day => dayNames[day]));
        }

        private static string FormatCompactTimeRange(Lecture lecture)
        {
            return $"{FormatCompactTime(lecture.StartTime, lecture.StartAM_PM)}-{FormatCompactTime(lecture.EndTime, lecture.EndAM_PM)}";
        }

        private static string FormatCompactTime(TimeSpan time, string amPm)
        {
            return $"{time.Hours}:{time.Minutes:D2}{amPm.Trim().ToLowerInvariant()}";
        }

        private void ShowScheduleStatus(string message, IEnumerable<string> notes)
        {
            StringBuilder statusText = new StringBuilder(message);

            foreach (string note in notes.Where(note => !string.IsNullOrWhiteSpace(note)))
            {
                statusText.AppendLine();
                statusText.Append("- ");
                statusText.Append(note);
            }

            richTextBox1.Text = statusText.ToString();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Text = "Creating schedule, please wait...";
            richTextBox1.Refresh();

            Cursor previousCursor = Cursor.Current ?? Cursors.Default;
            Cursor.Current = Cursors.WaitCursor;
            button2.Enabled = false;

            try
            {
                bool scheduleBuilt;

                if (EarlyMorningRB.Checked)
                {
                    scheduleBuilt = studentSchedule.BuildMorningSchedule();
                }
                else if (AfternoonRB.Checked)
                {
                    scheduleBuilt = studentSchedule.BuildAfternoonSchedule();
                }
                else if (MinCommuteRB.Checked)
                {
                    scheduleBuilt = studentSchedule.BuildMinCommuteSchedule();
                }
                else if (RateMyRB.Checked)
                {
                    scheduleBuilt = studentSchedule.BuildRMPscoreSchedule();
                }
                else
                {
                    scheduleBuilt = studentSchedule.TryBuildSchedule();
                }

                if (scheduleBuilt)
                {
                    DisplayChosenScheduleDetails();
                    ScheduleDisplayHelper.LoadScheduleIntoGrid(dataGridView1, studentSchedule);
                    ShowScheduleStatus("Schedule created successfully.", studentSchedule.ScheduleNotes);
                }
                else
                {
                    ClearScheduleDetailsDisplay();
                    ShowScheduleStatus("No valid schedule could be built.", studentSchedule.ScheduleNotes);
                }
            }
            finally
            {
                button2.Enabled = true;
                Cursor.Current = previousCursor;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Text Files (*.txt)|*.txt",
                Title = "Import Lectures"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                studentSchedule.ImportSchedule(dialog.FileName);
                RefreshLectureDisplay();
                richTextBox1.Text = "Lectures imported successfully.";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                Title = "Export Lectures"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                studentSchedule.ExportSchedule(dialog.FileName);
                richTextBox1.Text = "Lectures exported successfully.";
            }
            else
                richTextBox1.Text = "Export Failed.";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LectureDisplay_TextChanged(object sender, EventArgs e)
        {

        }

        private void clearButton_Click(object? sender, EventArgs e)
        {
            studentSchedule = new Schedule();
            LectureDisplay.Clear();
            ClearScheduleDetailsDisplay();
            dataGridView1.Rows.Clear();
            richTextBox1.Text = "Selected classes cleared.";
        }

        private void printschedulebutton_Click(object sender, EventArgs e)
        {
            if (!studentSchedule.ScheduleLectures.Any() || dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Please generate a finalized schedule before printing.",
                    "No Schedule to Print",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using PrintDocument printDocument = new PrintDocument();
            printDocument.DocumentName = "CPP Final Schedule";
            printDocument.DefaultPageSettings.Landscape = true;
            printDocument.BeginPrint += (_, _) => printScheduleRowIndex = 0;
            printDocument.PrintPage += PrintSchedulePage;

            printDialog1.Document = printDocument;

            if (printDialog1.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                printDocument.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to print the schedule.{Environment.NewLine}{ex.Message}",
                    "Print Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void PrintSchedulePage(object sender, PrintPageEventArgs e)
        {
            List<DataGridViewColumn> columns = GetPrintableGridColumns();
            List<DataGridViewRow> rows = GetPrintableGridRows();

            if (columns.Count == 0 || rows.Count == 0)
            {
                e.HasMorePages = false;
                return;
            }

            Graphics? graphics = e.Graphics;
            if (graphics == null)
            {
                e.HasMorePages = false;
                return;
            }

            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using Font titleFont = new Font(Font.FontFamily, 14f, FontStyle.Bold);
            using Font headerFont = new Font(Font.FontFamily, 8f, FontStyle.Bold);
            using Font rowHeaderFont = new Font(Font.FontFamily, 7f, FontStyle.Regular);
            using Font cellFont = new Font(Font.FontFamily, 6.5f, FontStyle.Regular);
            using Pen borderPen = new Pen(Color.FromArgb(180, 180, 180));
            using StringFormat centeredText = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.LineLimit
            };

            RectangleF pageBounds = e.MarginBounds;
            float titleHeight = titleFont.GetHeight(graphics) + 8f;
            RectangleF titleBounds = new RectangleF(pageBounds.Left, pageBounds.Top, pageBounds.Width, titleHeight);
            graphics.DrawString("Final Schedule", titleFont, Brushes.Black, titleBounds, centeredText);

            float gridTop = titleBounds.Bottom + 8f;
            float headerHeight = 28f;
            float availableGridHeight = pageBounds.Bottom - gridTop;
            float availableRowHeight = availableGridHeight - headerHeight;

            if (availableRowHeight <= 0)
            {
                e.HasMorePages = false;
                return;
            }

            float timeColumnWidth = Math.Min(72f, pageBounds.Width * 0.14f);
            float dayColumnWidth = (pageBounds.Width - timeColumnWidth) / columns.Count;
            int remainingRowCount = rows.Count - printScheduleRowIndex;
            float fittedRowHeight = availableRowHeight / Math.Max(remainingRowCount, 1);
            float rowHeight = Math.Min(34f, Math.Max(18f, fittedRowHeight));
            int rowsPerPage = Math.Max(1, (int)Math.Floor(availableRowHeight / rowHeight));
            int lastRowIndex = Math.Min(printScheduleRowIndex + rowsPerPage, rows.Count);

            Color headerBackColor = Color.FromArgb(0, 80, 48);
            Color rowHeaderBackColor = GetPrintableHeaderBackColor(dataGridView1.RowHeadersDefaultCellStyle.BackColor);

            RectangleF timeHeaderBounds = new RectangleF(pageBounds.Left, gridTop, timeColumnWidth, headerHeight);
            DrawPrintCell(graphics, timeHeaderBounds, "Time", headerFont, headerBackColor, Color.White, borderPen, centeredText);

            for (int columnPosition = 0; columnPosition < columns.Count; columnPosition++)
            {
                DataGridViewColumn column = columns[columnPosition];
                RectangleF columnHeaderBounds = new RectangleF(
                    pageBounds.Left + timeColumnWidth + (dayColumnWidth * columnPosition),
                    gridTop,
                    dayColumnWidth,
                    headerHeight);

                DrawPrintCell(graphics, columnHeaderBounds, column.HeaderText, headerFont, headerBackColor, Color.White, borderPen, centeredText);
            }

            float y = gridTop + headerHeight;

            for (int rowIndex = printScheduleRowIndex; rowIndex < lastRowIndex; rowIndex++)
            {
                DataGridViewRow row = rows[rowIndex];
                RectangleF rowHeaderBounds = new RectangleF(pageBounds.Left, y, timeColumnWidth, rowHeight);
                string rowHeaderText = Convert.ToString(row.HeaderCell.Value) ?? string.Empty;
                DrawPrintCell(graphics, rowHeaderBounds, rowHeaderText, rowHeaderFont, rowHeaderBackColor, Color.White, borderPen, centeredText);

                for (int columnPosition = 0; columnPosition < columns.Count; columnPosition++)
                {
                    DataGridViewColumn column = columns[columnPosition];
                    DataGridViewCell cell = row.Cells[column.Index];
                    RectangleF cellBounds = new RectangleF(
                        pageBounds.Left + timeColumnWidth + (dayColumnWidth * columnPosition),
                        y,
                        dayColumnWidth,
                        rowHeight);

                    DrawPrintCell(
                        graphics,
                        cellBounds,
                        Convert.ToString(cell.Value) ?? string.Empty,
                        cellFont,
                        GetPrintableCellBackColor(cell),
                        GetPrintableCellForeColor(cell),
                        borderPen,
                        centeredText);
                }

                y += rowHeight;
            }

            printScheduleRowIndex = lastRowIndex;
            e.HasMorePages = printScheduleRowIndex < rows.Count;

            if (!e.HasMorePages)
            {
                printScheduleRowIndex = 0;
            }
        }

        private List<DataGridViewColumn> GetPrintableGridColumns()
        {
            return dataGridView1.Columns
                .Cast<DataGridViewColumn>()
                .Where(column => column.Visible)
                .OrderBy(column => column.DisplayIndex)
                .ToList();
        }

        private List<DataGridViewRow> GetPrintableGridRows()
        {
            return dataGridView1.Rows
                .Cast<DataGridViewRow>()
                .Where(row => row.Visible && !row.IsNewRow)
                .ToList();
        }

        private static void DrawPrintCell(
            Graphics graphics,
            RectangleF bounds,
            string text,
            Font font,
            Color backColor,
            Color foreColor,
            Pen borderPen,
            StringFormat stringFormat)
        {
            RectangleF textBounds = RectangleF.Inflate(bounds, -3f, -2f);

            using SolidBrush backBrush = new SolidBrush(backColor);
            using SolidBrush textBrush = new SolidBrush(foreColor);

            graphics.FillRectangle(backBrush, bounds);
            graphics.DrawRectangle(borderPen, bounds.X, bounds.Y, bounds.Width, bounds.Height);
            graphics.DrawString(text, font, textBrush, textBounds, stringFormat);
        }

        private static Color GetPrintableHeaderBackColor(Color color)
        {
            return color.IsEmpty ? Color.FromArgb(0, 80, 48) : color;
        }

        private static Color GetPrintableCellBackColor(DataGridViewCell cell)
        {
            Color color = cell.Style.BackColor;

            if (color.IsEmpty)
            {
                color = cell.InheritedStyle.BackColor;
            }

            return color.IsEmpty ? Color.White : color;
        }

        private static Color GetPrintableCellForeColor(DataGridViewCell cell)
        {
            Color color = cell.Style.ForeColor;

            if (color.IsEmpty)
            {
                color = cell.InheritedStyle.ForeColor;
            }

            return color.IsEmpty ? Color.Black : color;
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }
    }
}
