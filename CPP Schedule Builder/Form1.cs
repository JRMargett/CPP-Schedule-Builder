using System.Text.Json;

namespace CPP_Schedule_Builder
{
    public partial class Form1 : Form
    {
        private const string CourseDataFileName = "courses.json";
        private Dictionary<string, string[]>? coursesBySubject;

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

            string classId = ClassIDTB.Text.Trim();
            string professor = InstructorTB.Text.Trim();

            if (string.IsNullOrWhiteSpace(classId) || string.IsNullOrWhiteSpace(professor))
            {
                MessageBox.Show("Please enter the class ID and professor name.", "Missing Class Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string days = string.Join(", ", DayCheckBox.CheckedItems.Cast<string>());

            if (string.IsNullOrWhiteSpace(days))
            {
                MessageBox.Show("Please select at least one class day.", "Missing Days", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryFormatTime(StartTimeHr.Text, StartTimeMin.Text, StartAM_PMCB.SelectedItem, out string startTime) ||
                !TryFormatTime(EndTimeHr.Text, EndTimeMin.Text, EndAM_PMCB.SelectedItem, out string endTime))
            {
                MessageBox.Show("Please enter valid start and end times.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SplitCourse(selectedCourse, out string courseNumber, out string courseName);

            string displayText = $"ID: {classId} | {subject} {courseNumber} - {courseName} | {days} {startTime} - {endTime} | Professor: {professor}";
            LectureDisplay.Items.Add(displayText);
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

        private static bool TryFormatTime(string hourText, string minuteText, object? amPmSelection, out string formattedTime)
        {
            formattedTime = string.Empty;

            if (!int.TryParse(hourText.Trim(), out int hour) ||
                !int.TryParse(minuteText.Trim(), out int minute) ||
                amPmSelection is not string amPm ||
                hour < 1 ||
                hour > 12 ||
                minute < 0 ||
                minute > 59)
            {
                return false;
            }

            formattedTime = $"{hour}:{minute:00} {amPm}";
            return true;
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

            dataGridView1.Columns.Add("Time", "Time");
            dataGridView1.Columns.Add("Sunday", "Sunday");
            dataGridView1.Columns.Add("Monday", "Monday");
            dataGridView1.Columns.Add("Tuesday", "Tuesday");
            dataGridView1.Columns.Add("Wednesday", "Wednesday");
            dataGridView1.Columns.Add("Thursday", "Thursday");
            dataGridView1.Columns.Add("Friday", "Friday");
            dataGridView1.Columns.Add("Saturday", "Saturday");

            DateTime time = DateTime.Today.AddHours(7);
            DateTime end = DateTime.Today.AddHours(24);

            while (time <= end)
            {
                dataGridView1.Rows.Add(time.ToString("h:mm tt"));
                time = time.AddMinutes(30);
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)//scheduled class sections
        {

        }

        private void button2_Click_1(object sender, EventArgs e)//schedule button
        {

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
