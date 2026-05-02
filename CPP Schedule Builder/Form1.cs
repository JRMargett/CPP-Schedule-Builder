using System.Drawing.Text;
using System.Text.Json;
using System.IO;

namespace CPP_Schedule_Builder
{
    public partial class Form1 : Form
    {

        private const string CourseDataFileName = "courses.json";

        private Dictionary<string, string[]> CoursesBySubject = LoadCoursesBySubject();

        private static Dictionary<string, string[]> LoadCoursesBySubject()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Data", CourseDataFileName);

            try {
                return JsonSerializer.Deserialize<Dictionary<string, string[]>>(File.ReadAllText(path)) ?? new Dictionary<string, string[]>();
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException) {
                MessageBox.Show(
                    $"Unable to load course data from {path}.{Environment.NewLine}{ex.Message}",
                    "Course Data Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return new Dictionary<string, string[]>();
            }
        }

        private Schedule studentSchedule = new Schedule();
        public Form1()
        {
            InitializeComponent();
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
            if (CourseNumber.SelectedItem == null)
            {
                richTextBox1.Text = "Please select a course number.";
                return;
            }

            if (StartAM_PMCB.SelectedItem == null || EndAM_PMCB.SelectedItem == null)
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

            string input = CourseNumber.SelectedItem.ToString();
            string[] parts = input.Split('-');
            string code = parts[0].Trim();
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

            string startAM_PM = StartAM_PMCB.SelectedItem.ToString();
            string endAM_PM = EndAM_PMCB.SelectedItem.ToString();

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


            LectureDisplay.Items.Add(
                $"{lecture.ClassCode} - {lecture.ClassName} | {lecture.Instructor}"
            );
            studentSchedule.AddLecture(lecture);

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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (MinCommuteRB.Checked)
            {
                if (studentSchedule.BuildMinCommuteSchedule())
                {
                    ScheduleDisplayHelper.LoadScheduleIntoGrid(dataGridView1, studentSchedule);
                    richTextBox1.Text = "Schedule created successfully.";
                }
                else
                {
                    richTextBox1.Text = "No valid schedule could be built.";
                }
            }
            if (studentSchedule.TryBuildSchedule())
            {
                ScheduleDisplayHelper.LoadScheduleIntoGrid(dataGridView1, studentSchedule);
                richTextBox1.Text = "Schedule created successfully.";
            }
            else
            {
                richTextBox1.Text = "No valid schedule could be built.";
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
                LectureDisplay.Items.Clear();
                foreach (Lecture lecture in studentSchedule.Lectures)
                {
                    LectureDisplay.Items.Add($"{lecture.ClassCode} - {lecture.ClassName} | {lecture.Instructor}");
                }
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
    }
}

