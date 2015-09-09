namespace WindowsFormsApplication1
{
    partial class FreezeDetectorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.StartButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.NameLabel = new System.Windows.Forms.Label();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.NumSamplesLabel = new System.Windows.Forms.Label();
            this.DirectoryLabel = new System.Windows.Forms.Label();
            this.UserNameBox = new System.Windows.Forms.TextBox();
            this.DirectoryBox = new System.Windows.Forms.TextBox();
            this.UserEmailBox = new System.Windows.Forms.TextBox();
            this.NumSamplesBox = new System.Windows.Forms.ComboBox();
            this.TemperatureLabel = new System.Windows.Forms.Label();
            this.MyHelpButton = new System.Windows.Forms.Button();
            this.AboutButton = new System.Windows.Forms.Button();
            this.SampleBox1 = new System.Windows.Forms.TextBox();
            this.DirectoryButton = new System.Windows.Forms.Button();
            this.SampleBox2 = new System.Windows.Forms.TextBox();
            this.SampleBox3 = new System.Windows.Forms.TextBox();
            this.SampleBox6 = new System.Windows.Forms.TextBox();
            this.SampleBox5 = new System.Windows.Forms.TextBox();
            this.SampleBox4 = new System.Windows.Forms.TextBox();
            this.SampleBox8 = new System.Windows.Forms.TextBox();
            this.SampleBox7 = new System.Windows.Forms.TextBox();
            this.GraphLabel = new System.Windows.Forms.Label();
            this.SampleLabel1 = new System.Windows.Forms.Label();
            this.SampleLabel2 = new System.Windows.Forms.Label();
            this.SampleLabel3 = new System.Windows.Forms.Label();
            this.SampleLabel6 = new System.Windows.Forms.Label();
            this.SampleLabel5 = new System.Windows.Forms.Label();
            this.SampleLabel4 = new System.Windows.Forms.Label();
            this.SampleLabel8 = new System.Windows.Forms.Label();
            this.SampleLabel7 = new System.Windows.Forms.Label();
            this.TempGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.TuneButton = new System.Windows.Forms.Button();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.TimeBox = new System.Windows.Forms.TextBox();
            this.NameBox1 = new System.Windows.Forms.TextBox();
            this.NameBox2 = new System.Windows.Forms.TextBox();
            this.NameBox3 = new System.Windows.Forms.TextBox();
            this.NameBox4 = new System.Windows.Forms.TextBox();
            this.NameBox8 = new System.Windows.Forms.TextBox();
            this.NameBox7 = new System.Windows.Forms.TextBox();
            this.NameBox6 = new System.Windows.Forms.TextBox();
            this.NameBox5 = new System.Windows.Forms.TextBox();
            this.NumThermocouplesLabel = new System.Windows.Forms.Label();
            this.NumThermocouplesBox = new System.Windows.Forms.TextBox();
            this.LogIntervalLabel = new System.Windows.Forms.Label();
            this.LogIntervalBox = new System.Windows.Forms.TextBox();
            this.BoardNumBox = new System.Windows.Forms.TextBox();
            this.BoardNumLabel = new System.Windows.Forms.Label();
            this.StateBox8 = new System.Windows.Forms.TextBox();
            this.StateBox7 = new System.Windows.Forms.TextBox();
            this.StateBox6 = new System.Windows.Forms.TextBox();
            this.StateBox5 = new System.Windows.Forms.TextBox();
            this.StateBox4 = new System.Windows.Forms.TextBox();
            this.StateBox3 = new System.Windows.Forms.TextBox();
            this.StateBox2 = new System.Windows.Forms.TextBox();
            this.StateBox1 = new System.Windows.Forms.TextBox();
            this.InfoBox8 = new System.Windows.Forms.TextBox();
            this.InfoBox7 = new System.Windows.Forms.TextBox();
            this.InfoBox6 = new System.Windows.Forms.TextBox();
            this.InfoBox5 = new System.Windows.Forms.TextBox();
            this.InfoBox4 = new System.Windows.Forms.TextBox();
            this.InfoBox3 = new System.Windows.Forms.TextBox();
            this.InfoBox2 = new System.Windows.Forms.TextBox();
            this.InfoBox1 = new System.Windows.Forms.TextBox();
            this.TestButton = new System.Windows.Forms.Button();
            this.ShutDownLabel = new System.Windows.Forms.Label();
            this.ShutDownMinutesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TempGraph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShutDownMinutesNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(933, 16);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(85, 34);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "START";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.Location = new System.Drawing.Point(1119, 16);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(85, 34);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "EXIT";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopButton.Location = new System.Drawing.Point(1026, 16);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(85, 34);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "STOP";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(30, 19);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(43, 13);
            this.NameLabel.TabIndex = 3;
            this.NameLabel.Text = "Name:";
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmailLabel.Location = new System.Drawing.Point(30, 46);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(41, 13);
            this.EmailLabel.TabIndex = 4;
            this.EmailLabel.Text = "Email:";
            // 
            // NumSamplesLabel
            // 
            this.NumSamplesLabel.AutoSize = true;
            this.NumSamplesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumSamplesLabel.Location = new System.Drawing.Point(420, 46);
            this.NumSamplesLabel.Name = "NumSamplesLabel";
            this.NumSamplesLabel.Size = new System.Drawing.Size(58, 13);
            this.NumSamplesLabel.TabIndex = 6;
            this.NumSamplesLabel.Text = "Samples:";
            // 
            // DirectoryLabel
            // 
            this.DirectoryLabel.AutoSize = true;
            this.DirectoryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DirectoryLabel.Location = new System.Drawing.Point(420, 19);
            this.DirectoryLabel.Name = "DirectoryLabel";
            this.DirectoryLabel.Size = new System.Drawing.Size(62, 13);
            this.DirectoryLabel.TabIndex = 5;
            this.DirectoryLabel.Text = "Directory:";
            // 
            // UserNameBox
            // 
            this.UserNameBox.Location = new System.Drawing.Point(83, 16);
            this.UserNameBox.Name = "UserNameBox";
            this.UserNameBox.Size = new System.Drawing.Size(192, 20);
            this.UserNameBox.TabIndex = 7;
            // 
            // DirectoryBox
            // 
            this.DirectoryBox.BackColor = System.Drawing.SystemColors.Window;
            this.DirectoryBox.Location = new System.Drawing.Point(488, 16);
            this.DirectoryBox.Name = "DirectoryBox";
            this.DirectoryBox.ReadOnly = true;
            this.DirectoryBox.Size = new System.Drawing.Size(257, 20);
            this.DirectoryBox.TabIndex = 8;
            // 
            // UserEmailBox
            // 
            this.UserEmailBox.Location = new System.Drawing.Point(83, 43);
            this.UserEmailBox.Name = "UserEmailBox";
            this.UserEmailBox.Size = new System.Drawing.Size(192, 20);
            this.UserEmailBox.TabIndex = 9;
            this.UserEmailBox.Text = "kobayashi.a.an@m.titech.ac.jp";
            // 
            // NumSamplesBox
            // 
            this.NumSamplesBox.FormattingEnabled = true;
            this.NumSamplesBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.NumSamplesBox.Location = new System.Drawing.Point(488, 42);
            this.NumSamplesBox.Name = "NumSamplesBox";
            this.NumSamplesBox.Size = new System.Drawing.Size(58, 21);
            this.NumSamplesBox.TabIndex = 10;
            // 
            // TemperatureLabel
            // 
            this.TemperatureLabel.AutoSize = true;
            this.TemperatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TemperatureLabel.Location = new System.Drawing.Point(24, 116);
            this.TemperatureLabel.Name = "TemperatureLabel";
            this.TemperatureLabel.Size = new System.Drawing.Size(225, 20);
            this.TemperatureLabel.TabIndex = 11;
            this.TemperatureLabel.Text = "Sample Temperatures (°C):";
            // 
            // MyHelpButton
            // 
            this.MyHelpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MyHelpButton.Location = new System.Drawing.Point(1119, 53);
            this.MyHelpButton.Name = "MyHelpButton";
            this.MyHelpButton.Size = new System.Drawing.Size(85, 34);
            this.MyHelpButton.TabIndex = 12;
            this.MyHelpButton.Text = "HELP";
            this.MyHelpButton.UseVisualStyleBackColor = true;
            this.MyHelpButton.Click += new System.EventHandler(this.MyHelpButton_Click);
            // 
            // AboutButton
            // 
            this.AboutButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AboutButton.Location = new System.Drawing.Point(1026, 53);
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(85, 34);
            this.AboutButton.TabIndex = 13;
            this.AboutButton.Text = "ABOUT";
            this.AboutButton.UseVisualStyleBackColor = true;
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // SampleBox1
            // 
            this.SampleBox1.BackColor = System.Drawing.Color.Black;
            this.SampleBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SampleBox1.ForeColor = System.Drawing.Color.Lime;
            this.SampleBox1.Location = new System.Drawing.Point(28, 165);
            this.SampleBox1.Multiline = true;
            this.SampleBox1.Name = "SampleBox1";
            this.SampleBox1.ReadOnly = true;
            this.SampleBox1.Size = new System.Drawing.Size(105, 32);
            this.SampleBox1.TabIndex = 14;
            // 
            // DirectoryButton
            // 
            this.DirectoryButton.Location = new System.Drawing.Point(751, 16);
            this.DirectoryButton.Name = "DirectoryButton";
            this.DirectoryButton.Size = new System.Drawing.Size(28, 20);
            this.DirectoryButton.TabIndex = 15;
            this.DirectoryButton.Text = "...";
            this.DirectoryButton.UseVisualStyleBackColor = true;
            this.DirectoryButton.Click += new System.EventHandler(this.DirectoryButton_Click);
            // 
            // SampleBox2
            // 
            this.SampleBox2.BackColor = System.Drawing.Color.Black;
            this.SampleBox2.ForeColor = System.Drawing.Color.Lime;
            this.SampleBox2.Location = new System.Drawing.Point(181, 165);
            this.SampleBox2.Multiline = true;
            this.SampleBox2.Name = "SampleBox2";
            this.SampleBox2.ReadOnly = true;
            this.SampleBox2.Size = new System.Drawing.Size(105, 32);
            this.SampleBox2.TabIndex = 16;
            // 
            // SampleBox3
            // 
            this.SampleBox3.BackColor = System.Drawing.Color.Black;
            this.SampleBox3.ForeColor = System.Drawing.Color.Lime;
            this.SampleBox3.Location = new System.Drawing.Point(334, 165);
            this.SampleBox3.Multiline = true;
            this.SampleBox3.Name = "SampleBox3";
            this.SampleBox3.ReadOnly = true;
            this.SampleBox3.Size = new System.Drawing.Size(105, 32);
            this.SampleBox3.TabIndex = 17;
            // 
            // SampleBox6
            // 
            this.SampleBox6.BackColor = System.Drawing.Color.Black;
            this.SampleBox6.ForeColor = System.Drawing.Color.Lime;
            this.SampleBox6.Location = new System.Drawing.Point(793, 165);
            this.SampleBox6.Multiline = true;
            this.SampleBox6.Name = "SampleBox6";
            this.SampleBox6.ReadOnly = true;
            this.SampleBox6.Size = new System.Drawing.Size(105, 32);
            this.SampleBox6.TabIndex = 21;
            // 
            // SampleBox5
            // 
            this.SampleBox5.BackColor = System.Drawing.Color.Black;
            this.SampleBox5.ForeColor = System.Drawing.Color.Lime;
            this.SampleBox5.Location = new System.Drawing.Point(640, 165);
            this.SampleBox5.Multiline = true;
            this.SampleBox5.Name = "SampleBox5";
            this.SampleBox5.ReadOnly = true;
            this.SampleBox5.Size = new System.Drawing.Size(105, 32);
            this.SampleBox5.TabIndex = 20;
            // 
            // SampleBox4
            // 
            this.SampleBox4.BackColor = System.Drawing.Color.Black;
            this.SampleBox4.ForeColor = System.Drawing.Color.Lime;
            this.SampleBox4.Location = new System.Drawing.Point(487, 165);
            this.SampleBox4.Multiline = true;
            this.SampleBox4.Name = "SampleBox4";
            this.SampleBox4.ReadOnly = true;
            this.SampleBox4.Size = new System.Drawing.Size(105, 32);
            this.SampleBox4.TabIndex = 19;
            // 
            // SampleBox8
            // 
            this.SampleBox8.BackColor = System.Drawing.Color.Black;
            this.SampleBox8.ForeColor = System.Drawing.Color.Lime;
            this.SampleBox8.Location = new System.Drawing.Point(1099, 165);
            this.SampleBox8.Multiline = true;
            this.SampleBox8.Name = "SampleBox8";
            this.SampleBox8.ReadOnly = true;
            this.SampleBox8.Size = new System.Drawing.Size(105, 32);
            this.SampleBox8.TabIndex = 23;
            // 
            // SampleBox7
            // 
            this.SampleBox7.BackColor = System.Drawing.Color.Black;
            this.SampleBox7.ForeColor = System.Drawing.Color.Lime;
            this.SampleBox7.Location = new System.Drawing.Point(946, 165);
            this.SampleBox7.Multiline = true;
            this.SampleBox7.Name = "SampleBox7";
            this.SampleBox7.ReadOnly = true;
            this.SampleBox7.Size = new System.Drawing.Size(105, 32);
            this.SampleBox7.TabIndex = 22;
            // 
            // GraphLabel
            // 
            this.GraphLabel.AutoSize = true;
            this.GraphLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GraphLabel.Location = new System.Drawing.Point(24, 279);
            this.GraphLabel.Name = "GraphLabel";
            this.GraphLabel.Size = new System.Drawing.Size(186, 20);
            this.GraphLabel.TabIndex = 24;
            this.GraphLabel.Text = "Temperature vs. Time:";
            // 
            // SampleLabel1
            // 
            this.SampleLabel1.AutoSize = true;
            this.SampleLabel1.Location = new System.Drawing.Point(56, 148);
            this.SampleLabel1.Name = "SampleLabel1";
            this.SampleLabel1.Size = new System.Drawing.Size(51, 13);
            this.SampleLabel1.TabIndex = 25;
            this.SampleLabel1.Text = "Sample 1";
            // 
            // SampleLabel2
            // 
            this.SampleLabel2.AutoSize = true;
            this.SampleLabel2.Location = new System.Drawing.Point(209, 148);
            this.SampleLabel2.Name = "SampleLabel2";
            this.SampleLabel2.Size = new System.Drawing.Size(51, 13);
            this.SampleLabel2.TabIndex = 26;
            this.SampleLabel2.Text = "Sample 2";
            // 
            // SampleLabel3
            // 
            this.SampleLabel3.AutoSize = true;
            this.SampleLabel3.Location = new System.Drawing.Point(362, 148);
            this.SampleLabel3.Name = "SampleLabel3";
            this.SampleLabel3.Size = new System.Drawing.Size(51, 13);
            this.SampleLabel3.TabIndex = 27;
            this.SampleLabel3.Text = "Sample 3";
            // 
            // SampleLabel6
            // 
            this.SampleLabel6.AutoSize = true;
            this.SampleLabel6.Location = new System.Drawing.Point(821, 148);
            this.SampleLabel6.Name = "SampleLabel6";
            this.SampleLabel6.Size = new System.Drawing.Size(51, 13);
            this.SampleLabel6.TabIndex = 30;
            this.SampleLabel6.Text = "Sample 6";
            // 
            // SampleLabel5
            // 
            this.SampleLabel5.AutoSize = true;
            this.SampleLabel5.Location = new System.Drawing.Point(668, 148);
            this.SampleLabel5.Name = "SampleLabel5";
            this.SampleLabel5.Size = new System.Drawing.Size(51, 13);
            this.SampleLabel5.TabIndex = 29;
            this.SampleLabel5.Text = "Sample 5";
            // 
            // SampleLabel4
            // 
            this.SampleLabel4.AutoSize = true;
            this.SampleLabel4.Location = new System.Drawing.Point(515, 148);
            this.SampleLabel4.Name = "SampleLabel4";
            this.SampleLabel4.Size = new System.Drawing.Size(51, 13);
            this.SampleLabel4.TabIndex = 28;
            this.SampleLabel4.Text = "Sample 4";
            // 
            // SampleLabel8
            // 
            this.SampleLabel8.AutoSize = true;
            this.SampleLabel8.Location = new System.Drawing.Point(1127, 148);
            this.SampleLabel8.Name = "SampleLabel8";
            this.SampleLabel8.Size = new System.Drawing.Size(51, 13);
            this.SampleLabel8.TabIndex = 32;
            this.SampleLabel8.Text = "Sample 8";
            // 
            // SampleLabel7
            // 
            this.SampleLabel7.AutoSize = true;
            this.SampleLabel7.Location = new System.Drawing.Point(974, 148);
            this.SampleLabel7.Name = "SampleLabel7";
            this.SampleLabel7.Size = new System.Drawing.Size(51, 13);
            this.SampleLabel7.TabIndex = 31;
            this.SampleLabel7.Text = "Sample 7";
            // 
            // TempGraph
            // 
            this.TempGraph.BackColor = System.Drawing.Color.Transparent;
            this.TempGraph.BackImageTransparentColor = System.Drawing.Color.Transparent;
            this.TempGraph.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            this.TempGraph.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.TempGraph.Legends.Add(legend1);
            this.TempGraph.Location = new System.Drawing.Point(12, 302);
            this.TempGraph.Name = "TempGraph";
            this.TempGraph.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            this.TempGraph.Size = new System.Drawing.Size(1166, 376);
            this.TempGraph.TabIndex = 33;
            this.TempGraph.Text = "TempVsTime";
            // 
            // TuneButton
            // 
            this.TuneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TuneButton.Location = new System.Drawing.Point(933, 53);
            this.TuneButton.Name = "TuneButton";
            this.TuneButton.Size = new System.Drawing.Size(85, 34);
            this.TuneButton.TabIndex = 34;
            this.TuneButton.Text = "TUNE";
            this.TuneButton.UseVisualStyleBackColor = true;
            this.TuneButton.Click += new System.EventHandler(this.TuneButton_Click);
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLabel.Location = new System.Drawing.Point(593, 46);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(87, 13);
            this.TimeLabel.TabIndex = 36;
            this.TimeLabel.Text = "Time Elapsed:";
            // 
            // TimeBox
            // 
            this.TimeBox.BackColor = System.Drawing.SystemColors.Window;
            this.TimeBox.Location = new System.Drawing.Point(687, 42);
            this.TimeBox.Name = "TimeBox";
            this.TimeBox.ReadOnly = true;
            this.TimeBox.Size = new System.Drawing.Size(92, 20);
            this.TimeBox.TabIndex = 37;
            // 
            // NameBox1
            // 
            this.NameBox1.Location = new System.Drawing.Point(28, 236);
            this.NameBox1.Name = "NameBox1";
            this.NameBox1.Size = new System.Drawing.Size(105, 20);
            this.NameBox1.TabIndex = 53;
            // 
            // NameBox2
            // 
            this.NameBox2.Location = new System.Drawing.Point(181, 236);
            this.NameBox2.Name = "NameBox2";
            this.NameBox2.Size = new System.Drawing.Size(105, 20);
            this.NameBox2.TabIndex = 54;
            // 
            // NameBox3
            // 
            this.NameBox3.Location = new System.Drawing.Point(334, 236);
            this.NameBox3.Name = "NameBox3";
            this.NameBox3.Size = new System.Drawing.Size(105, 20);
            this.NameBox3.TabIndex = 55;
            // 
            // NameBox4
            // 
            this.NameBox4.Location = new System.Drawing.Point(488, 236);
            this.NameBox4.Name = "NameBox4";
            this.NameBox4.Size = new System.Drawing.Size(105, 20);
            this.NameBox4.TabIndex = 56;
            // 
            // NameBox8
            // 
            this.NameBox8.Location = new System.Drawing.Point(1100, 236);
            this.NameBox8.Name = "NameBox8";
            this.NameBox8.Size = new System.Drawing.Size(105, 20);
            this.NameBox8.TabIndex = 60;
            // 
            // NameBox7
            // 
            this.NameBox7.Location = new System.Drawing.Point(946, 236);
            this.NameBox7.Name = "NameBox7";
            this.NameBox7.Size = new System.Drawing.Size(105, 20);
            this.NameBox7.TabIndex = 59;
            // 
            // NameBox6
            // 
            this.NameBox6.Location = new System.Drawing.Point(793, 236);
            this.NameBox6.Name = "NameBox6";
            this.NameBox6.Size = new System.Drawing.Size(105, 20);
            this.NameBox6.TabIndex = 58;
            // 
            // NameBox5
            // 
            this.NameBox5.Location = new System.Drawing.Point(640, 236);
            this.NameBox5.Name = "NameBox5";
            this.NameBox5.Size = new System.Drawing.Size(105, 20);
            this.NameBox5.TabIndex = 57;
            // 
            // NumThermocouplesLabel
            // 
            this.NumThermocouplesLabel.AutoSize = true;
            this.NumThermocouplesLabel.Location = new System.Drawing.Point(268, 120);
            this.NumThermocouplesLabel.Name = "NumThermocouplesLabel";
            this.NumThermocouplesLabel.Size = new System.Drawing.Size(176, 13);
            this.NumThermocouplesLabel.TabIndex = 61;
            this.NumThermocouplesLabel.Text = "Number of thermocouples detected:";
            // 
            // NumThermocouplesBox
            // 
            this.NumThermocouplesBox.BackColor = System.Drawing.SystemColors.Window;
            this.NumThermocouplesBox.Location = new System.Drawing.Point(447, 117);
            this.NumThermocouplesBox.Name = "NumThermocouplesBox";
            this.NumThermocouplesBox.ReadOnly = true;
            this.NumThermocouplesBox.Size = new System.Drawing.Size(41, 20);
            this.NumThermocouplesBox.TabIndex = 62;
            // 
            // LogIntervalLabel
            // 
            this.LogIntervalLabel.AutoSize = true;
            this.LogIntervalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogIntervalLabel.Location = new System.Drawing.Point(30, 74);
            this.LogIntervalLabel.Name = "LogIntervalLabel";
            this.LogIntervalLabel.Size = new System.Drawing.Size(137, 13);
            this.LogIntervalLabel.TabIndex = 65;
            this.LogIntervalLabel.Text = "Log interval (seconds):";
            // 
            // LogIntervalBox
            // 
            this.LogIntervalBox.Location = new System.Drawing.Point(181, 69);
            this.LogIntervalBox.Name = "LogIntervalBox";
            this.LogIntervalBox.Size = new System.Drawing.Size(94, 20);
            this.LogIntervalBox.TabIndex = 66;
            // 
            // BoardNumBox
            // 
            this.BoardNumBox.BackColor = System.Drawing.SystemColors.Window;
            this.BoardNumBox.Location = new System.Drawing.Point(740, 117);
            this.BoardNumBox.Name = "BoardNumBox";
            this.BoardNumBox.ReadOnly = true;
            this.BoardNumBox.Size = new System.Drawing.Size(39, 20);
            this.BoardNumBox.TabIndex = 68;
            // 
            // BoardNumLabel
            // 
            this.BoardNumLabel.AutoSize = true;
            this.BoardNumLabel.Location = new System.Drawing.Point(533, 118);
            this.BoardNumLabel.Name = "BoardNumLabel";
            this.BoardNumLabel.Size = new System.Drawing.Size(201, 13);
            this.BoardNumLabel.TabIndex = 67;
            this.BoardNumLabel.Text = "USB-TC Board Number (board detected):";
            // 
            // StateBox8
            // 
            this.StateBox8.BackColor = System.Drawing.Color.Black;
            this.StateBox8.ForeColor = System.Drawing.Color.Lime;
            this.StateBox8.Location = new System.Drawing.Point(1100, 197);
            this.StateBox8.Name = "StateBox8";
            this.StateBox8.ReadOnly = true;
            this.StateBox8.Size = new System.Drawing.Size(105, 20);
            this.StateBox8.TabIndex = 76;
            // 
            // StateBox7
            // 
            this.StateBox7.BackColor = System.Drawing.Color.Black;
            this.StateBox7.ForeColor = System.Drawing.Color.Lime;
            this.StateBox7.Location = new System.Drawing.Point(946, 197);
            this.StateBox7.Name = "StateBox7";
            this.StateBox7.ReadOnly = true;
            this.StateBox7.Size = new System.Drawing.Size(105, 20);
            this.StateBox7.TabIndex = 75;
            // 
            // StateBox6
            // 
            this.StateBox6.BackColor = System.Drawing.Color.Black;
            this.StateBox6.ForeColor = System.Drawing.Color.Lime;
            this.StateBox6.Location = new System.Drawing.Point(793, 197);
            this.StateBox6.Name = "StateBox6";
            this.StateBox6.ReadOnly = true;
            this.StateBox6.Size = new System.Drawing.Size(105, 20);
            this.StateBox6.TabIndex = 74;
            // 
            // StateBox5
            // 
            this.StateBox5.BackColor = System.Drawing.Color.Black;
            this.StateBox5.ForeColor = System.Drawing.Color.Lime;
            this.StateBox5.Location = new System.Drawing.Point(640, 197);
            this.StateBox5.Name = "StateBox5";
            this.StateBox5.ReadOnly = true;
            this.StateBox5.Size = new System.Drawing.Size(105, 20);
            this.StateBox5.TabIndex = 73;
            // 
            // StateBox4
            // 
            this.StateBox4.BackColor = System.Drawing.Color.Black;
            this.StateBox4.ForeColor = System.Drawing.Color.Lime;
            this.StateBox4.Location = new System.Drawing.Point(488, 197);
            this.StateBox4.Name = "StateBox4";
            this.StateBox4.ReadOnly = true;
            this.StateBox4.Size = new System.Drawing.Size(105, 20);
            this.StateBox4.TabIndex = 72;
            // 
            // StateBox3
            // 
            this.StateBox3.BackColor = System.Drawing.Color.Black;
            this.StateBox3.ForeColor = System.Drawing.Color.Lime;
            this.StateBox3.Location = new System.Drawing.Point(334, 197);
            this.StateBox3.Name = "StateBox3";
            this.StateBox3.ReadOnly = true;
            this.StateBox3.Size = new System.Drawing.Size(105, 20);
            this.StateBox3.TabIndex = 71;
            // 
            // StateBox2
            // 
            this.StateBox2.BackColor = System.Drawing.Color.Black;
            this.StateBox2.ForeColor = System.Drawing.Color.Lime;
            this.StateBox2.Location = new System.Drawing.Point(181, 197);
            this.StateBox2.Name = "StateBox2";
            this.StateBox2.ReadOnly = true;
            this.StateBox2.Size = new System.Drawing.Size(105, 20);
            this.StateBox2.TabIndex = 70;
            // 
            // StateBox1
            // 
            this.StateBox1.BackColor = System.Drawing.Color.Black;
            this.StateBox1.ForeColor = System.Drawing.Color.Lime;
            this.StateBox1.Location = new System.Drawing.Point(28, 197);
            this.StateBox1.Name = "StateBox1";
            this.StateBox1.ReadOnly = true;
            this.StateBox1.Size = new System.Drawing.Size(105, 20);
            this.StateBox1.TabIndex = 69;
            // 
            // InfoBox8
            // 
            this.InfoBox8.BackColor = System.Drawing.Color.Black;
            this.InfoBox8.ForeColor = System.Drawing.Color.Lime;
            this.InfoBox8.Location = new System.Drawing.Point(1100, 217);
            this.InfoBox8.Name = "InfoBox8";
            this.InfoBox8.ReadOnly = true;
            this.InfoBox8.Size = new System.Drawing.Size(105, 20);
            this.InfoBox8.TabIndex = 84;
            // 
            // InfoBox7
            // 
            this.InfoBox7.BackColor = System.Drawing.Color.Black;
            this.InfoBox7.ForeColor = System.Drawing.Color.Lime;
            this.InfoBox7.Location = new System.Drawing.Point(946, 217);
            this.InfoBox7.Name = "InfoBox7";
            this.InfoBox7.ReadOnly = true;
            this.InfoBox7.Size = new System.Drawing.Size(105, 20);
            this.InfoBox7.TabIndex = 83;
            // 
            // InfoBox6
            // 
            this.InfoBox6.BackColor = System.Drawing.Color.Black;
            this.InfoBox6.ForeColor = System.Drawing.Color.Lime;
            this.InfoBox6.Location = new System.Drawing.Point(793, 217);
            this.InfoBox6.Name = "InfoBox6";
            this.InfoBox6.ReadOnly = true;
            this.InfoBox6.Size = new System.Drawing.Size(105, 20);
            this.InfoBox6.TabIndex = 82;
            // 
            // InfoBox5
            // 
            this.InfoBox5.BackColor = System.Drawing.Color.Black;
            this.InfoBox5.ForeColor = System.Drawing.Color.Lime;
            this.InfoBox5.Location = new System.Drawing.Point(640, 217);
            this.InfoBox5.Name = "InfoBox5";
            this.InfoBox5.ReadOnly = true;
            this.InfoBox5.Size = new System.Drawing.Size(105, 20);
            this.InfoBox5.TabIndex = 81;
            // 
            // InfoBox4
            // 
            this.InfoBox4.BackColor = System.Drawing.Color.Black;
            this.InfoBox4.ForeColor = System.Drawing.Color.Lime;
            this.InfoBox4.Location = new System.Drawing.Point(488, 217);
            this.InfoBox4.Name = "InfoBox4";
            this.InfoBox4.ReadOnly = true;
            this.InfoBox4.Size = new System.Drawing.Size(105, 20);
            this.InfoBox4.TabIndex = 80;
            // 
            // InfoBox3
            // 
            this.InfoBox3.BackColor = System.Drawing.Color.Black;
            this.InfoBox3.ForeColor = System.Drawing.Color.Lime;
            this.InfoBox3.Location = new System.Drawing.Point(334, 217);
            this.InfoBox3.Name = "InfoBox3";
            this.InfoBox3.ReadOnly = true;
            this.InfoBox3.Size = new System.Drawing.Size(105, 20);
            this.InfoBox3.TabIndex = 79;
            // 
            // InfoBox2
            // 
            this.InfoBox2.BackColor = System.Drawing.Color.Black;
            this.InfoBox2.ForeColor = System.Drawing.Color.Lime;
            this.InfoBox2.Location = new System.Drawing.Point(181, 217);
            this.InfoBox2.Name = "InfoBox2";
            this.InfoBox2.ReadOnly = true;
            this.InfoBox2.Size = new System.Drawing.Size(105, 20);
            this.InfoBox2.TabIndex = 78;
            // 
            // InfoBox1
            // 
            this.InfoBox1.BackColor = System.Drawing.Color.Black;
            this.InfoBox1.ForeColor = System.Drawing.Color.Lime;
            this.InfoBox1.Location = new System.Drawing.Point(28, 217);
            this.InfoBox1.Name = "InfoBox1";
            this.InfoBox1.ReadOnly = true;
            this.InfoBox1.Size = new System.Drawing.Size(105, 20);
            this.InfoBox1.TabIndex = 77;
            // 
            // TestButton
            // 
            this.TestButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.TestButton.Location = new System.Drawing.Point(824, 29);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(74, 47);
            this.TestButton.TabIndex = 85;
            this.TestButton.Text = "Test";
            this.TestButton.UseVisualStyleBackColor = false;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // ShutDownLabel
            // 
            this.ShutDownLabel.AutoSize = true;
            this.ShutDownLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShutDownLabel.Location = new System.Drawing.Point(420, 74);
            this.ShutDownLabel.Name = "ShutDownLabel";
            this.ShutDownLabel.Size = new System.Drawing.Size(260, 13);
            this.ShutDownLabel.TabIndex = 86;
            this.ShutDownLabel.Text = "Number of minutes after freezing to stop run:";
            // 
            // ShutDownMinutesNumericUpDown
            // 
            this.ShutDownMinutesNumericUpDown.Location = new System.Drawing.Point(686, 72);
            this.ShutDownMinutesNumericUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.ShutDownMinutesNumericUpDown.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.ShutDownMinutesNumericUpDown.Name = "ShutDownMinutesNumericUpDown";
            this.ShutDownMinutesNumericUpDown.Size = new System.Drawing.Size(59, 20);
            this.ShutDownMinutesNumericUpDown.TabIndex = 87;
            this.ShutDownMinutesNumericUpDown.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(748, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 88;
            this.label1.Text = "minutes";
            // 
            // FreezeDetectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 671);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ShutDownMinutesNumericUpDown);
            this.Controls.Add(this.ShutDownLabel);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.InfoBox8);
            this.Controls.Add(this.InfoBox7);
            this.Controls.Add(this.InfoBox6);
            this.Controls.Add(this.InfoBox5);
            this.Controls.Add(this.InfoBox4);
            this.Controls.Add(this.InfoBox3);
            this.Controls.Add(this.InfoBox2);
            this.Controls.Add(this.InfoBox1);
            this.Controls.Add(this.StateBox8);
            this.Controls.Add(this.StateBox7);
            this.Controls.Add(this.StateBox6);
            this.Controls.Add(this.StateBox5);
            this.Controls.Add(this.StateBox4);
            this.Controls.Add(this.StateBox3);
            this.Controls.Add(this.StateBox2);
            this.Controls.Add(this.StateBox1);
            this.Controls.Add(this.BoardNumBox);
            this.Controls.Add(this.BoardNumLabel);
            this.Controls.Add(this.LogIntervalBox);
            this.Controls.Add(this.LogIntervalLabel);
            this.Controls.Add(this.NumThermocouplesBox);
            this.Controls.Add(this.NumThermocouplesLabel);
            this.Controls.Add(this.NameBox8);
            this.Controls.Add(this.NameBox7);
            this.Controls.Add(this.NameBox6);
            this.Controls.Add(this.NameBox5);
            this.Controls.Add(this.NameBox4);
            this.Controls.Add(this.NameBox3);
            this.Controls.Add(this.NameBox2);
            this.Controls.Add(this.NameBox1);
            this.Controls.Add(this.TimeBox);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.TuneButton);
            this.Controls.Add(this.TempGraph);
            this.Controls.Add(this.SampleLabel8);
            this.Controls.Add(this.SampleLabel7);
            this.Controls.Add(this.SampleLabel6);
            this.Controls.Add(this.SampleLabel5);
            this.Controls.Add(this.SampleLabel4);
            this.Controls.Add(this.SampleLabel3);
            this.Controls.Add(this.SampleLabel2);
            this.Controls.Add(this.SampleLabel1);
            this.Controls.Add(this.GraphLabel);
            this.Controls.Add(this.SampleBox8);
            this.Controls.Add(this.SampleBox7);
            this.Controls.Add(this.SampleBox6);
            this.Controls.Add(this.SampleBox5);
            this.Controls.Add(this.SampleBox4);
            this.Controls.Add(this.SampleBox3);
            this.Controls.Add(this.SampleBox2);
            this.Controls.Add(this.DirectoryButton);
            this.Controls.Add(this.SampleBox1);
            this.Controls.Add(this.AboutButton);
            this.Controls.Add(this.MyHelpButton);
            this.Controls.Add(this.TemperatureLabel);
            this.Controls.Add(this.NumSamplesBox);
            this.Controls.Add(this.UserEmailBox);
            this.Controls.Add(this.DirectoryBox);
            this.Controls.Add(this.UserNameBox);
            this.Controls.Add(this.NumSamplesLabel);
            this.Controls.Add(this.DirectoryLabel);
            this.Controls.Add(this.EmailLabel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.StartButton);
            this.Name = "FreezeDetectorForm";
            this.Text = "  ";
            this.Load += new System.EventHandler(this.FreezeDetectorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TempGraph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShutDownMinutesNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.Label NumSamplesLabel;
        private System.Windows.Forms.Label DirectoryLabel;
        private System.Windows.Forms.TextBox UserNameBox;
        private System.Windows.Forms.TextBox DirectoryBox;
        private System.Windows.Forms.TextBox UserEmailBox;
        private System.Windows.Forms.ComboBox NumSamplesBox;
        private System.Windows.Forms.Label TemperatureLabel;
        private System.Windows.Forms.Button MyHelpButton;
        private System.Windows.Forms.Button AboutButton;
        private System.Windows.Forms.TextBox SampleBox1;
        private System.Windows.Forms.Button DirectoryButton;
        private System.Windows.Forms.TextBox SampleBox2;
        private System.Windows.Forms.TextBox SampleBox3;
        private System.Windows.Forms.TextBox SampleBox6;
        private System.Windows.Forms.TextBox SampleBox5;
        private System.Windows.Forms.TextBox SampleBox4;
        private System.Windows.Forms.TextBox SampleBox8;
        private System.Windows.Forms.TextBox SampleBox7;
        private System.Windows.Forms.Label GraphLabel;
        private System.Windows.Forms.Label SampleLabel1;
        private System.Windows.Forms.Label SampleLabel2;
        private System.Windows.Forms.Label SampleLabel3;
        private System.Windows.Forms.Label SampleLabel6;
        private System.Windows.Forms.Label SampleLabel5;
        private System.Windows.Forms.Label SampleLabel4;
        private System.Windows.Forms.Label SampleLabel8;
        private System.Windows.Forms.Label SampleLabel7;
        private System.Windows.Forms.DataVisualization.Charting.Chart TempGraph;
        private System.Windows.Forms.Button TuneButton;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.TextBox TimeBox;
        private System.Windows.Forms.TextBox NameBox1;
        private System.Windows.Forms.TextBox NameBox2;
        private System.Windows.Forms.TextBox NameBox3;
        private System.Windows.Forms.TextBox NameBox4;
        private System.Windows.Forms.TextBox NameBox8;
        private System.Windows.Forms.TextBox NameBox7;
        private System.Windows.Forms.TextBox NameBox6;
        private System.Windows.Forms.TextBox NameBox5;
        private System.Windows.Forms.Label NumThermocouplesLabel;
        private System.Windows.Forms.TextBox NumThermocouplesBox;
        private System.Windows.Forms.Label LogIntervalLabel;
        private System.Windows.Forms.TextBox LogIntervalBox;
        private System.Windows.Forms.TextBox BoardNumBox;
        private System.Windows.Forms.Label BoardNumLabel;
        private System.Windows.Forms.TextBox StateBox8;
        private System.Windows.Forms.TextBox StateBox7;
        private System.Windows.Forms.TextBox StateBox6;
        private System.Windows.Forms.TextBox StateBox5;
        private System.Windows.Forms.TextBox StateBox4;
        private System.Windows.Forms.TextBox StateBox3;
        private System.Windows.Forms.TextBox StateBox2;
        private System.Windows.Forms.TextBox StateBox1;
        private System.Windows.Forms.TextBox InfoBox8;
        private System.Windows.Forms.TextBox InfoBox7;
        private System.Windows.Forms.TextBox InfoBox6;
        private System.Windows.Forms.TextBox InfoBox5;
        private System.Windows.Forms.TextBox InfoBox4;
        private System.Windows.Forms.TextBox InfoBox3;
        private System.Windows.Forms.TextBox InfoBox2;
        private System.Windows.Forms.TextBox InfoBox1;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.Label ShutDownLabel;
        private System.Windows.Forms.NumericUpDown ShutDownMinutesNumericUpDown;
        private System.Windows.Forms.Label label1;
    }
}

