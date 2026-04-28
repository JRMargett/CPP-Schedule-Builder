using System.Text.Json;

namespace CPP_Schedule_Builder
{
    public partial class Form1 : Form
    {
        private const string CourseDataFileName = "courses.json";
        private static readonly Color[] ScheduleColors =
        {
            Color.FromArgb(0, 80, 48),
            Color.FromArgb(0, 122, 204),
            Color.FromArgb(117, 63, 152),
            Color.FromArgb(180, 83, 9),
            Color.FromArgb(146, 64, 14),
            Color.FromArgb(15, 118, 110)
        };

        private Dictionary<string, string[]>? coursesBySubject;
        private readonly RateMyProfessorClient rateMyProfessorClient = new();

        private Dictionary<string, string[]> CoursesBySubject
        {
            get
            {
                coursesBySubject ??= LoadCoursesBySubject();
                return coursesBySubject;
            }
        }

        private static Dictionary<string, string[]> LoadCoursesBySubject()
        {
            string courseDataPath = Path.Combine(AppContext.BaseDirectory, "Data", CourseDataFileName);

            try
            {
                string json = File.ReadAllText(courseDataPath);
                return JsonSerializer.Deserialize<Dictionary<string, string[]>>(json) ?? new Dictionary<string, string[]>();
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is JsonException)
            {
                MessageBox.Show(
                    $"Unable to load course data from {courseDataPath}.{Environment.NewLine}{ex.Message}",
                    "Course Data Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return new Dictionary<string, string[]>();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CourseSubject.Items.Add("ACC");
            CourseSubject.SelectedIndex = -1;
            CourseNumber.DropDownStyle = ComboBoxStyle.DropDownList;
            CourseSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            LectureDisplay.HorizontalScrollbar = true;
            SetupScheduleGrid();
            radioButton2.Checked = true;
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

        private void richTextBox1_TextChanged(object sender, EventArgs e) //notifcations
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

            if (CoursesBySubject.TryGetValue(selectedSubject, out string[]? courses) && courses is { Length: > 0 })
            {
                CourseNumber.Items.AddRange(courses);
                CourseNumber.Enabled = true;
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
            if (CourseSubject.SelectedItem is not string subject || CourseNumber.SelectedItem is not string selectedCourse)
            {
                MessageBox.Show("Please select a course subject and course number.", "Missing Course", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string classIdText = ClassIDTB.Text.Trim();
            string professor = InstructorTB.Text.Trim();

            if (!int.TryParse(classIdText, out int classId) || string.IsNullOrWhiteSpace(professor))
            {
                MessageBox.Show("Please enter a numeric class ID and professor name.", "Missing Class Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<DayOfWeek> days = DayCheckBox.CheckedItems
                .Cast<string>()
                .Select(day => Enum.Parse<DayOfWeek>(day))
                .ToList();

            if (days.Count == 0)
            {
                MessageBox.Show("Please select at least one class day.", "Missing Days", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryReadTime(StartTimeHr.Text, StartTimeMin.Text, StartAM_PMCB.SelectedItem, out TimeSpan start, out string startAM_PM) ||
                !TryReadTime(EndTimeHr.Text, EndTimeMin.Text, EndAM_PMCB.SelectedItem, out TimeSpan end, out string endAM_PM))
            {
                MessageBox.Show("Please enter valid start and end times.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ToTimeOfDay(end, endAM_PM) <= ToTimeOfDay(start, startAM_PM))
            {
                MessageBox.Show("Please make sure the end time is after the start time.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SplitCourse(selectedCourse, out string courseNumber, out string courseName);

            Lecture lecture = new Lecture(classId, subject, courseName, courseNumber, professor, days, start, startAM_PM, end, endAM_PM);
            LectureDisplay.Items.Add(lecture);
        }

        private static void SplitCourse(string selectedCourse, out string courseNumber, out string courseName)
        {
            int separatorIndex = selectedCourse.IndexOf(" - ", StringComparison.Ordinal);

            if (separatorIndex < 0)
            {
                courseNumber = selectedCourse.Trim();
                courseName = string.Empty;
                return;
            }

            courseNumber = selectedCourse[..separatorIndex].Trim();
            courseName = selectedCourse[(separatorIndex + 3)..].Trim();
        }

        private static bool TryReadTime(string hourText, string minuteText, object? amPmSelection, out TimeSpan time, out string amPm)
        {
            time = TimeSpan.Zero;
            amPm = string.Empty;

            if (!int.TryParse(hourText.Trim(), out int hour) ||
                !int.TryParse(minuteText.Trim(), out int minute) ||
                amPmSelection is not string selectedAmPm ||
                hour < 1 ||
                hour > 12 ||
                minute < 0 ||
                minute > 59)
            {
                return false;
            }

            time = new TimeSpan(hour, minute, 0);
            amPm = selectedAmPm;
            return true;
        }

        private static TimeSpan ToTimeOfDay(TimeSpan time, string amPm)
        {
            int hour = time.Hours;

            if (string.Equals(amPm, "PM", StringComparison.OrdinalIgnoreCase) && hour != 12)
            {
                hour += 12;
            }
            else if (string.Equals(amPm, "AM", StringComparison.OrdinalIgnoreCase) && hour == 12)
            {
                hour = 0;
            }

            return new TimeSpan(hour, time.Minutes, 0);
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SetupScheduleGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dataGridView1.Columns.Add("Time", "Time");
            dataGridView1.Columns.Add("Sunday", "Sunday");
            dataGridView1.Columns.Add("Monday", "Monday");
            dataGridView1.Columns.Add("Tuesday", "Tuesday");
            dataGridView1.Columns.Add("Wednesday", "Wednesday");
            dataGridView1.Columns.Add("Thursday", "Thursday");
            dataGridView1.Columns.Add("Friday", "Friday");
            dataGridView1.Columns.Add("Saturday", "Saturday");

            TimeSpan time = TimeSpan.FromHours(7);
            TimeSpan end = TimeSpan.FromHours(24);

            while (time <= end)
            {
                int rowIndex = dataGridView1.Rows.Add(DateTime.Today.Add(time).ToString("h:mm tt"));
                dataGridView1.Rows[rowIndex].Tag = time;
                dataGridView1.Rows[rowIndex].Height = 28;
                time = time.Add(TimeSpan.FromMinutes(30));
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ClearScheduleGrid()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                for (int columnIndex = 1; columnIndex < dataGridView1.Columns.Count; columnIndex++)
                {
                    DataGridViewCell cell = row.Cells[columnIndex];
                    cell.Value = null;
                    cell.Style.BackColor = Color.Empty;
                    cell.Style.ForeColor = Color.Empty;
                    cell.Style.SelectionBackColor = Color.Empty;
                    cell.Style.SelectionForeColor = Color.Empty;
                }
            }
        }

        private void ShowScheduleResult(ScheduleOptimizationResult result, IScheduleOptimizer optimizer, int totalCourseCount)
        {
            listBox3.Items.Clear();
            ClearScheduleGrid();

            foreach (Lecture lecture in result.SelectedLectures)
            {
                listBox3.Items.Add(lecture);
            }

            List<string> gridErrors = MarkScheduleGrid(result.SelectedLectures);
            List<string> errors = result.Errors.Concat(gridErrors).ToList();

            if (errors.Count == 0)
            {
                AppendNotification($"Success: scheduled all {totalCourseCount} class(es) using {optimizer.Name}.");
                return;
            }

            AppendNotification($"Scheduled {result.SelectedLectures.Count} of {totalCourseCount} class(es) using {optimizer.Name}.");

            foreach (string error in errors)
            {
                AppendNotification($"Error: {error}");
            }
        }

        private List<string> MarkScheduleGrid(IReadOnlyList<Lecture> lectures)
        {
            List<string> errors = new();

            for (int lectureIndex = 0; lectureIndex < lectures.Count; lectureIndex++)
            {
                Lecture lecture = lectures[lectureIndex];
                Color scheduleColor = ScheduleColors[lectureIndex % ScheduleColors.Length];
                string cellText = $"{lecture.Subject} {lecture.ClassCode}{Environment.NewLine}ID {lecture.ClassID}";
                TimeSpan start = ToTimeOfDay(lecture.StartTime, lecture.StartAM_PM);
                TimeSpan end = ToTimeOfDay(lecture.EndTime, lecture.EndAM_PM);

                foreach (DayOfWeek day in lecture.Days)
                {
                    int columnIndex = GetDayColumnIndex(day);
                    int markedRows = 0;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Tag is not TimeSpan slotStart)
                        {
                            continue;
                        }

                        TimeSpan slotEnd = slotStart.Add(TimeSpan.FromMinutes(30));

                        if (slotStart >= end || start >= slotEnd)
                        {
                            continue;
                        }

                        DataGridViewCell cell = row.Cells[columnIndex];
                        cell.Value = cellText;
                        cell.Style.BackColor = scheduleColor;
                        cell.Style.ForeColor = Color.White;
                        cell.Style.SelectionBackColor = scheduleColor;
                        cell.Style.SelectionForeColor = Color.White;
                        markedRows++;
                    }

                    if (markedRows == 0)
                    {
                        errors.Add($"Could not mark {GetCourseKey(lecture)} on {day}; its time is outside the grid.");
                    }
                }
            }

            return errors;
        }

        private static int GetDayColumnIndex(DayOfWeek day)
        {
            return (int)day + 1;
        }

        private static string GetCourseKey(Lecture lecture)
        {
            return $"{lecture.Subject} {lecture.ClassCode}".Trim();
        }

        private void AppendNotification(string message)
        {
            if (richTextBox1.TextLength > 0)
            {
                richTextBox1.AppendText(Environment.NewLine);
            }

            richTextBox1.AppendText(message);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)//scheduled class sections
        {

        }

        private async void button2_Click_1(object sender, EventArgs e)//schedule button
        {
            List<Lecture> candidates = LectureDisplay.Items.OfType<Lecture>().ToList();

            richTextBox1.Clear();

            if (candidates.Count == 0)
            {
                richTextBox1.Text = "Error: Add at least one class section before building a schedule.";
                return;
            }

            IScheduleOptimizer? optimizer = GetSelectedOptimizer();

            if (optimizer == null)
            {
                richTextBox1.Text = "Error: Select an optimization option before building a schedule.";
                return;
            }

            int totalCourseCount = candidates
                .GroupBy(GetCourseKey)
                .Count();

            button2.Enabled = false;

            try
            {
                if (optimizer is RateMyProfessorOptimizer)
                {
                    await LoadRateMyProfessorScoresAsync(candidates);
                }

                ScheduleOptimizationResult result = optimizer.Optimize(candidates);
                ShowScheduleResult(result, optimizer, totalCourseCount);
            }
            finally
            {
                button2.Enabled = true;
            }
        }

        private async Task LoadRateMyProfessorScoresAsync(List<Lecture> lectures)
        {
            AppendNotification("Loading Rate My Professor scores...");

            foreach (Lecture lecture in lectures)
            {
                RateMyProfessorRating? rating = await rateMyProfessorClient.GetProfessorRatingAsync(lecture.Instructor);

                lecture.RateMyProfessorScore = rating?.Score;
                lecture.RateMyProfessorRatingsCount = rating?.RatingsCount;
                lecture.RateMyProfessorMatchedName = rating?.Name;
                lecture.RateMyProfessorProfileUrl = rating?.ProfileUrl;
            }

            foreach (Lecture lecture in lectures.DistinctBy(lecture => lecture.Instructor))
            {
                if (lecture.RateMyProfessorScore.HasValue)
                {
                    AppendNotification(
                        $"Matched {lecture.Instructor} to {lecture.RateMyProfessorMatchedName}: {lecture.RateMyProfessorScore:0.0}/5 from {lecture.RateMyProfessorRatingsCount ?? 0} rating(s).");
                }
                else
                {
                    AppendNotification($"Error: No Rate My Professor score found for {lecture.Instructor}.");
                }
            }
        }

        private IScheduleOptimizer? GetSelectedOptimizer()
        {
            if (radioButton2.Checked)
            {
                return new MinCommuteDaysOptimizer();
            }

            if (radioButton3.Checked)
            {
                return new EarlyMorningOptimizer();
            }

            if (radioButton4.Checked)
            {
                return new AfternoonOptimizer();
            }

            if (radioButton5.Checked)
            {
                return new RateMyProfessorOptimizer();
            }

            return null;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)//min commmute days optimization
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)// early morning classes perfered
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)// afternoon classes perfered
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)//pick based off rate my profesor api
        {

        }
    }
}
