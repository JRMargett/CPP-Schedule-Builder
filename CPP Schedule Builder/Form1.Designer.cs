namespace CPP_Schedule_Builder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panel1 = new Panel();
            clearButton = new Button();
            LectureDisplay = new RichTextBox();
            groupBox1 = new GroupBox();
            MinCommuteRB = new RadioButton();
            NoOptRB = new RadioButton();
            EarlyMorningRB = new RadioButton();
            AfternoonRB = new RadioButton();
            RateMyRB = new RadioButton();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            ClassIDTB = new TextBox();
            label16 = new Label();
            InstructorTB = new TextBox();
            label15 = new Label();
            EndAM_PMCB = new ComboBox();
            StartAM_PMCB = new ComboBox();
            EndTimeMin = new TextBox();
            EndTimeHr = new TextBox();
            label11 = new Label();
            StartTimeMin = new TextBox();
            label1 = new Label();
            StartTimeHr = new TextBox();
            DayCheckBox = new CheckedListBox();
            label8 = new Label();
            label7 = new Label();
            label2 = new Label();
            CourseNumber = new ComboBox();
            label12 = new Label();
            label10 = new Label();
            label6 = new Label();
            CourseSubject = new ComboBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            comboBox1 = new ComboBox();
            label9 = new Label();
            comboBoxBindingSource1 = new BindingSource(components);
            panel2 = new Panel();
            printschedulebutton = new Button();
            label17 = new Label();
            dataGridView1 = new DataGridView();
            Sunday = new DataGridViewTextBoxColumn();
            Monday = new DataGridViewTextBoxColumn();
            Tuesday = new DataGridViewTextBoxColumn();
            Wednesday = new DataGridViewTextBoxColumn();
            Thursday = new DataGridViewTextBoxColumn();
            Friday = new DataGridViewTextBoxColumn();
            Saturday = new DataGridViewTextBoxColumn();
            radioButton10 = new RadioButton();
            panel3 = new Panel();
            richTextBox2 = new RichTextBox();
            label14 = new Label();
            radioButton1 = new RadioButton();
            panel4 = new Panel();
            label13 = new Label();
            richTextBox1 = new RichTextBox();
            radioButton6 = new RadioButton();
            comboBoxBindingSource = new BindingSource(components);
            autoCompleteCustomSourceBindingSource = new BindingSource(components);
            printDialog1 = new PrintDialog();
            printPreviewDialog1 = new PrintPreviewDialog();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)comboBoxBindingSource1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)comboBoxBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)autoCompleteCustomSourceBindingSource).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(255, 184, 28);
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(clearButton);
            panel1.Controls.Add(LectureDisplay);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(ClassIDTB);
            panel1.Controls.Add(label16);
            panel1.Controls.Add(InstructorTB);
            panel1.Controls.Add(label15);
            panel1.Controls.Add(EndAM_PMCB);
            panel1.Controls.Add(StartAM_PMCB);
            panel1.Controls.Add(EndTimeMin);
            panel1.Controls.Add(EndTimeHr);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(StartTimeMin);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(StartTimeHr);
            panel1.Controls.Add(DayCheckBox);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(CourseNumber);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(CourseSubject);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(label9);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(482, 508);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // clearButton
            // 
            clearButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            clearButton.Location = new Point(375, 478);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(75, 23);
            clearButton.TabIndex = 58;
            clearButton.Text = "Clear";
            clearButton.UseVisualStyleBackColor = true;
            // 
            // LectureDisplay
            // 
            LectureDisplay.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LectureDisplay.Location = new Point(7, 333);
            LectureDisplay.Name = "LectureDisplay";
            LectureDisplay.ReadOnly = true;
            LectureDisplay.Size = new Size(331, 162);
            LectureDisplay.TabIndex = 57;
            LectureDisplay.Text = "";
            LectureDisplay.TextChanged += LectureDisplay_TextChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(MinCommuteRB);
            groupBox1.Controls.Add(NoOptRB);
            groupBox1.Controls.Add(EarlyMorningRB);
            groupBox1.Controls.Add(AfternoonRB);
            groupBox1.Controls.Add(RateMyRB);
            groupBox1.Location = new Point(341, 316);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(134, 130);
            groupBox1.TabIndex = 35;
            groupBox1.TabStop = false;
            groupBox1.Text = "Optimization";
            // 
            // MinCommuteRB
            // 
            MinCommuteRB.AutoSize = true;
            MinCommuteRB.Location = new Point(6, 39);
            MinCommuteRB.Name = "MinCommuteRB";
            MinCommuteRB.Size = new Size(103, 19);
            MinCommuteRB.TabIndex = 23;
            MinCommuteRB.TabStop = true;
            MinCommuteRB.Text = "Min Commute";
            MinCommuteRB.UseVisualStyleBackColor = true;
            MinCommuteRB.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // NoOptRB
            // 
            NoOptRB.AutoSize = true;
            NoOptRB.Location = new Point(6, 94);
            NoOptRB.Name = "NoOptRB";
            NoOptRB.Size = new Size(113, 19);
            NoOptRB.TabIndex = 35;
            NoOptRB.TabStop = true;
            NoOptRB.Text = "No Optimization";
            NoOptRB.UseVisualStyleBackColor = true;
            NoOptRB.CheckedChanged += radioButton7_CheckedChanged;
            // 
            // EarlyMorningRB
            // 
            EarlyMorningRB.AutoSize = true;
            EarlyMorningRB.Location = new Point(6, 22);
            EarlyMorningRB.Name = "EarlyMorningRB";
            EarlyMorningRB.Size = new Size(99, 19);
            EarlyMorningRB.TabIndex = 24;
            EarlyMorningRB.TabStop = true;
            EarlyMorningRB.Text = "Early Morning";
            EarlyMorningRB.UseVisualStyleBackColor = true;
            EarlyMorningRB.CheckedChanged += radioButton3_CheckedChanged;
            // 
            // AfternoonRB
            // 
            AfternoonRB.AutoSize = true;
            AfternoonRB.Location = new Point(6, 56);
            AfternoonRB.Name = "AfternoonRB";
            AfternoonRB.Size = new Size(79, 19);
            AfternoonRB.TabIndex = 25;
            AfternoonRB.TabStop = true;
            AfternoonRB.Text = "Afternoon";
            AfternoonRB.UseVisualStyleBackColor = true;
            // 
            // RateMyRB
            // 
            RateMyRB.AutoSize = true;
            RateMyRB.Location = new Point(6, 73);
            RateMyRB.Name = "RateMyRB";
            RateMyRB.Size = new Size(125, 19);
            RateMyRB.TabIndex = 26;
            RateMyRB.TabStop = true;
            RateMyRB.Text = "Rate My Prof Score";
            RateMyRB.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(400, 103);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 3;
            button4.Text = "Export";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.Location = new Point(400, 77);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 2;
            button3.Text = "Import";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            button2.Location = new Point(375, 449);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 54;
            button2.Text = "Run";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click_1;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(375, 199);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 14;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // ClassIDTB
            // 
            ClassIDTB.Font = new Font("Segoe UI", 10F);
            ClassIDTB.Location = new Point(110, 260);
            ClassIDTB.Name = "ClassIDTB";
            ClassIDTB.Size = new Size(79, 25);
            ClassIDTB.TabIndex = 12;
            ClassIDTB.TextChanged += textBox6_TextChanged;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI", 15F);
            label16.Location = new Point(110, 229);
            label16.Name = "label16";
            label16.Size = new Size(79, 28);
            label16.TabIndex = 51;
            label16.Text = "Class ID";
            // 
            // InstructorTB
            // 
            InstructorTB.Font = new Font("Segoe UI", 10F);
            InstructorTB.Location = new Point(194, 260);
            InstructorTB.Name = "InstructorTB";
            InstructorTB.Size = new Size(243, 25);
            InstructorTB.TabIndex = 13;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI", 15F);
            label15.Location = new Point(195, 229);
            label15.Name = "label15";
            label15.Size = new Size(96, 28);
            label15.TabIndex = 49;
            label15.Text = "Instructor";
            label15.Click += label15_Click;
            // 
            // EndAM_PMCB
            // 
            EndAM_PMCB.Font = new Font("Segoe UI", 10F);
            EndAM_PMCB.FormattingEnabled = true;
            EndAM_PMCB.Items.AddRange(new object[] { "AM", "PM" });
            EndAM_PMCB.Location = new Point(310, 201);
            EndAM_PMCB.Name = "EndAM_PMCB";
            EndAM_PMCB.Size = new Size(43, 25);
            EndAM_PMCB.TabIndex = 11;
            // 
            // StartAM_PMCB
            // 
            StartAM_PMCB.Font = new Font("Segoe UI", 10F);
            StartAM_PMCB.FormattingEnabled = true;
            StartAM_PMCB.Items.AddRange(new object[] { "AM", "PM" });
            StartAM_PMCB.Location = new Point(176, 201);
            StartAM_PMCB.Name = "StartAM_PMCB";
            StartAM_PMCB.Size = new Size(43, 25);
            StartAM_PMCB.TabIndex = 8;
            // 
            // EndTimeMin
            // 
            EndTimeMin.Font = new Font("Segoe UI", 10F);
            EndTimeMin.Location = new Point(268, 201);
            EndTimeMin.Name = "EndTimeMin";
            EndTimeMin.Size = new Size(36, 25);
            EndTimeMin.TabIndex = 10;
            // 
            // EndTimeHr
            // 
            EndTimeHr.Font = new Font("Segoe UI", 10F);
            EndTimeHr.Location = new Point(244, 201);
            EndTimeHr.Name = "EndTimeHr";
            EndTimeHr.Size = new Size(18, 25);
            EndTimeHr.TabIndex = 9;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(261, 206);
            label11.Name = "label11";
            label11.Size = new Size(10, 15);
            label11.TabIndex = 46;
            label11.Text = ":";
            // 
            // StartTimeMin
            // 
            StartTimeMin.Font = new Font("Segoe UI", 10F);
            StartTimeMin.Location = new Point(134, 201);
            StartTimeMin.Name = "StartTimeMin";
            StartTimeMin.Size = new Size(36, 25);
            StartTimeMin.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(226, 206);
            label1.Name = "label1";
            label1.Size = new Size(12, 15);
            label1.TabIndex = 41;
            label1.Text = "-";
            // 
            // StartTimeHr
            // 
            StartTimeHr.Font = new Font("Segoe UI", 10F);
            StartTimeHr.Location = new Point(110, 201);
            StartTimeHr.Name = "StartTimeHr";
            StartTimeHr.Size = new Size(18, 25);
            StartTimeHr.TabIndex = 6;
            StartTimeHr.TextChanged += textBox1_TextChanged;
            // 
            // DayCheckBox
            // 
            DayCheckBox.BackColor = Color.FromArgb(255, 184, 28);
            DayCheckBox.BorderStyle = BorderStyle.None;
            DayCheckBox.Font = new Font("Segoe UI", 10F);
            DayCheckBox.FormattingEnabled = true;
            DayCheckBox.Items.AddRange(new object[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" });
            DayCheckBox.Location = new Point(8, 199);
            DayCheckBox.Name = "DayCheckBox";
            DayCheckBox.Size = new Size(120, 100);
            DayCheckBox.TabIndex = 5;
            DayCheckBox.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 15F);
            label8.Location = new Point(110, 168);
            label8.Name = "label8";
            label8.Size = new Size(62, 28);
            label8.TabIndex = 38;
            label8.Text = "Times";
            label8.Click += label8_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(317, 211);
            label7.Name = "label7";
            label7.Size = new Size(0, 15);
            label7.TabIndex = 37;
            label7.Click += label7_Click_1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15F);
            label2.Location = new Point(7, 168);
            label2.Name = "label2";
            label2.Size = new Size(54, 28);
            label2.TabIndex = 30;
            label2.Text = "Days";
            label2.Click += label2_Click;
            // 
            // CourseNumber
            // 
            CourseNumber.Font = new Font("Segoe UI", 15F);
            CourseNumber.FormattingEnabled = true;
            CourseNumber.Location = new Point(96, 129);
            CourseNumber.Name = "CourseNumber";
            CourseNumber.Size = new Size(379, 36);
            CourseNumber.TabIndex = 5;
            CourseNumber.SelectedIndexChanged += CourseNumber_SelectedIndexChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(342, 315);
            label12.Name = "label12";
            label12.Size = new Size(0, 15);
            label12.TabIndex = 27;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(7, 302);
            label10.Name = "label10";
            label10.Size = new Size(171, 28);
            label10.TabIndex = 20;
            label10.Text = "Selected Courses";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 15F);
            label6.Location = new Point(96, 98);
            label6.Name = "label6";
            label6.Size = new Size(149, 28);
            label6.TabIndex = 9;
            label6.Text = "Course Number";
            // 
            // CourseSubject
            // 
            CourseSubject.Font = new Font("Segoe UI", 15F);
            CourseSubject.FormattingEnabled = true;
            CourseSubject.Items.AddRange(new object[] { "ABM", "ACC", "AG", "AGS", "AHS", "AMM", "ANT", "ARC", "ARO", "AST", "AVS", "BIO", "BUS", "CE", "CHE", "CHM", "CHN", "CIS", "CLS", "COM", "CPU", "CRM", "CS", "DAN", "EBZ", "EC", "ECE", "ECI", "ECS", "EDD", "EDL", "EDU", "EGR", "EMM", "EMT", "ENG", "ENV", "ERA", "ETE", "ETM", "EWS", "FRE", "FRL", "FST", "GBA", "GEO", "GER", "GSC", "HRT", "HST", "IAM", "IBM", "IE", "IGE", "IME", "INA", "KIN", "LA", "LIB", "LRC", "LS", "MAE", "MAT", "ME", "MFE", "MHR", "MPA", "MSL", "MTE", "MU", "NTR", "PHL", "PHY", "PLS", "PLT", "PSY", "RS", "SCI", "SE", "SME", "SOC", "SPN", "STA", "STS", "SW", "TH", "TOM", "URP", "VCD" });
            CourseSubject.Location = new Point(7, 129);
            CourseSubject.Name = "CourseSubject";
            CourseSubject.Size = new Size(83, 36);
            CourseSubject.TabIndex = 4;
            CourseSubject.SelectedIndexChanged += CourseSubject_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 15F);
            label5.Location = new Point(5, 98);
            label5.Name = "label5";
            label5.Size = new Size(77, 28);
            label5.TabIndex = 6;
            label5.Text = "Subject";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label4.Location = new Point(3, 0);
            label4.Name = "label4";
            label4.RightToLeft = RightToLeft.Yes;
            label4.Size = new Size(122, 28);
            label4.TabIndex = 5;
            label4.Text = "Select Term";
            label4.Click += label4_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label3.Location = new Point(5, 70);
            label3.Name = "label3";
            label3.Size = new Size(123, 28);
            label3.TabIndex = 4;
            label3.Text = "Add Classes";
            label3.Click += label3_Click;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 15F);
            comboBox1.FormattingEnabled = true;
            comboBox1.IntegralHeight = false;
            comboBox1.Items.AddRange(new object[] { "Spring 2026", "Summer 2026", "Fall 2026", "Winter 2026" });
            comboBox1.Location = new Point(5, 31);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(242, 36);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(127, 206);
            label9.Name = "label9";
            label9.Size = new Size(10, 15);
            label9.TabIndex = 43;
            label9.Text = ":";
            // 
            // comboBoxBindingSource1
            // 
            comboBoxBindingSource1.DataSource = typeof(ComboBox);
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(255, 184, 28);
            panel2.BorderStyle = BorderStyle.Fixed3D;
            panel2.Controls.Add(printschedulebutton);
            panel2.Controls.Add(label17);
            panel2.Controls.Add(dataGridView1);
            panel2.Controls.Add(radioButton10);
            panel2.Location = new Point(500, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(593, 393);
            panel2.TabIndex = 1;
            panel2.Paint += panel2_Paint;
            // 
            // printschedulebutton
            // 
            printschedulebutton.Location = new Point(502, 3);
            printschedulebutton.Name = "printschedulebutton";
            printschedulebutton.Size = new Size(75, 23);
            printschedulebutton.TabIndex = 58;
            printschedulebutton.Text = "Print";
            printschedulebutton.UseVisualStyleBackColor = true;
            printschedulebutton.Click += printschedulebutton_Click;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label17.Location = new Point(3, 0);
            label17.Name = "label17";
            label17.RightToLeft = RightToLeft.Yes;
            label17.Size = new Size(149, 28);
            label17.TabIndex = 57;
            label17.Text = "Schedule View";
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.FromArgb(255, 255, 128);
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday });
            dataGridView1.Location = new Point(8, 29);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(569, 357);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // Sunday
            // 
            Sunday.HeaderText = "Sunday";
            Sunday.Name = "Sunday";
            Sunday.Width = 75;
            // 
            // Monday
            // 
            Monday.HeaderText = "Monday";
            Monday.Name = "Monday";
            Monday.Width = 75;
            // 
            // Tuesday
            // 
            Tuesday.HeaderText = "Tuesday";
            Tuesday.Name = "Tuesday";
            Tuesday.Width = 75;
            // 
            // Wednesday
            // 
            Wednesday.HeaderText = "Wednesday";
            Wednesday.Name = "Wednesday";
            Wednesday.Width = 75;
            // 
            // Thursday
            // 
            Thursday.HeaderText = "Thursday";
            Thursday.Name = "Thursday";
            Thursday.Width = 75;
            // 
            // Friday
            // 
            Friday.HeaderText = "Friday";
            Friday.Name = "Friday";
            Friday.Width = 75;
            // 
            // Saturday
            // 
            Saturday.HeaderText = "Saturday";
            Saturday.Name = "Saturday";
            Saturday.Width = 75;
            // 
            // radioButton10
            // 
            radioButton10.AutoSize = true;
            radioButton10.Font = new Font("Segoe UI", 15F);
            radioButton10.Location = new Point(120, 130);
            radioButton10.Name = "radioButton10";
            radioButton10.Size = new Size(157, 32);
            radioButton10.TabIndex = 34;
            radioButton10.TabStop = true;
            radioButton10.Text = "radioButton10";
            radioButton10.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(255, 184, 28);
            panel3.BorderStyle = BorderStyle.Fixed3D;
            panel3.Controls.Add(richTextBox2);
            panel3.Controls.Add(label14);
            panel3.Controls.Add(radioButton1);
            panel3.Location = new Point(500, 411);
            panel3.Name = "panel3";
            panel3.Size = new Size(298, 109);
            panel3.TabIndex = 2;
            // 
            // richTextBox2
            // 
            richTextBox2.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox2.Location = new Point(8, 19);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(279, 83);
            richTextBox2.TabIndex = 30;
            richTextBox2.Text = "";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label14.Location = new Point(3, 1);
            label14.Name = "label14";
            label14.Size = new Size(127, 15);
            label14.TabIndex = 29;
            label14.Text = "Final Schedule Details";
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(-95, -12);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(94, 19);
            radioButton1.TabIndex = 16;
            radioButton1.TabStop = true;
            radioButton1.Text = "radioButton1";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(255, 184, 28);
            panel4.BorderStyle = BorderStyle.Fixed3D;
            panel4.Controls.Add(label13);
            panel4.Controls.Add(richTextBox1);
            panel4.Controls.Add(radioButton6);
            panel4.Location = new Point(804, 411);
            panel4.Name = "panel4";
            panel4.Size = new Size(289, 109);
            panel4.TabIndex = 3;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label13.Location = new Point(3, 1);
            label13.Name = "label13";
            label13.Size = new Size(113, 15);
            label13.TabIndex = 18;
            label13.Text = "Notifcations/Errors";
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox1.Location = new Point(3, 19);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(279, 83);
            richTextBox1.TabIndex = 17;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // radioButton6
            // 
            radioButton6.AutoSize = true;
            radioButton6.Location = new Point(-95, -12);
            radioButton6.Name = "radioButton6";
            radioButton6.Size = new Size(94, 19);
            radioButton6.TabIndex = 16;
            radioButton6.TabStop = true;
            radioButton6.Text = "radioButton6";
            radioButton6.UseVisualStyleBackColor = true;
            // 
            // comboBoxBindingSource
            // 
            comboBoxBindingSource.DataSource = typeof(ComboBox);
            // 
            // autoCompleteCustomSourceBindingSource
            // 
            autoCompleteCustomSourceBindingSource.DataMember = "AutoCompleteCustomSource";
            autoCompleteCustomSourceBindingSource.DataSource = comboBoxBindingSource;
            // 
            // printDialog1
            // 
            printDialog1.UseEXDialog = true;
            // 
            // printPreviewDialog1
            // 
            printPreviewDialog1.AutoScrollMargin = new Size(0, 0);
            printPreviewDialog1.AutoScrollMinSize = new Size(0, 0);
            printPreviewDialog1.ClientSize = new Size(400, 300);
            printPreviewDialog1.Enabled = true;
            printPreviewDialog1.Icon = (Icon)resources.GetObject("printPreviewDialog1.Icon");
            printPreviewDialog1.Name = "printPreviewDialog1";
            printPreviewDialog1.Visible = false;
            printPreviewDialog1.Load += printPreviewDialog1_Load;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 80, 48);
            ClientSize = new Size(1105, 532);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "CPP Schedule Builder";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)comboBoxBindingSource1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)comboBoxBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)autoCompleteCustomSourceBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Sunday;
        private DataGridViewTextBoxColumn Monday;
        private DataGridViewTextBoxColumn Tuesday;
        private DataGridViewTextBoxColumn Wednesday;
        private DataGridViewTextBoxColumn Thursday;
        private DataGridViewTextBoxColumn Friday;
        private DataGridViewTextBoxColumn Saturday;
        private ComboBox comboBox1;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private ComboBox CourseSubject;
        private Label label10;
        private RadioButton radioButton1;
        private RadioButton MinCommuteRB;
        private Label label12;
        private RadioButton RateMyRB;
        private RadioButton AfternoonRB;
        private RadioButton EarlyMorningRB;
        private Panel panel4;
        private RadioButton radioButton6;
        private RichTextBox richTextBox1;
        private Label label14;
        private Label label13;
        private ComboBox CourseNumber;
        private BindingSource comboBoxBindingSource1;
        private BindingSource comboBoxBindingSource;
        private BindingSource autoCompleteCustomSourceBindingSource;
        private Label label2;
        private RadioButton radioButton10;
        private Label label7;
        private Label label8;
        private CheckedListBox DayCheckBox;
        private ComboBox StartAM_PMCB;
        private TextBox EndTimeMin;
        private TextBox EndTimeHr;
        private Label label11;
        private TextBox StartTimeMin;
        private Label label1;
        private TextBox StartTimeHr;
        private Label label9;
        private ComboBox EndAM_PMCB;
        private Label label15;
        private TextBox InstructorTB;
        private Button button1;
        private TextBox ClassIDTB;
        private Label label16;
        private Button button2;
        private Button button4;
        private Button button3;
        private GroupBox groupBox1;
        private RadioButton NoOptRB;
        private Label label17;
        private RichTextBox richTextBox2;
        private RichTextBox LectureDisplay;
        private Button printschedulebutton;
        private PrintDialog printDialog1;
        private PrintPreviewDialog printPreviewDialog1;
        private Button clearButton;
    }
}
