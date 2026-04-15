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
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            dataGridView1 = new DataGridView();
            Sunday = new DataGridViewTextBoxColumn();
            Monday = new DataGridViewTextBoxColumn();
            Tuesday = new DataGridViewTextBoxColumn();
            Wednesday = new DataGridViewTextBoxColumn();
            Thursday = new DataGridViewTextBoxColumn();
            Friday = new DataGridViewTextBoxColumn();
            Saturday = new DataGridViewTextBoxColumn();
            label1 = new Label();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            label2 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(comboBox2);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(195, 426);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridView1);
            panel2.Location = new Point(213, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(575, 320);
            panel2.TabIndex = 1;
            panel2.Paint += panel2_Paint;
            // 
            // panel3
            // 
            panel3.Location = new Point(213, 338);
            panel3.Name = "panel3";
            panel3.Size = new Size(575, 100);
            panel3.TabIndex = 2;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday });
            dataGridView1.Location = new Point(0, 75);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(569, 214);
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 5);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 0;
            label1.Text = "Semester";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Spring", "Summer", "Fall", "Winter" });
            comboBox1.Location = new Point(3, 23);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(83, 23);
            comboBox1.TabIndex = 1;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(92, 23);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(57, 23);
            comboBox2.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(107, 5);
            label2.Name = "label2";
            label2.Size = new Size(29, 15);
            label2.TabIndex = 3;
            label2.Text = "Year";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private DataGridView dataGridView1;
        private Label label1;
        private DataGridViewTextBoxColumn Sunday;
        private DataGridViewTextBoxColumn Monday;
        private DataGridViewTextBoxColumn Tuesday;
        private DataGridViewTextBoxColumn Wednesday;
        private DataGridViewTextBoxColumn Thursday;
        private DataGridViewTextBoxColumn Friday;
        private DataGridViewTextBoxColumn Saturday;
        private Label label2;
        private ComboBox comboBox2;
        private ComboBox comboBox1;
    }
}
