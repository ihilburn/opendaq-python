// These things were included by default:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// Included MccDaq to use the Universal Library functions.
using MccDaq;

// Included this since this program uses threads.
using System.Threading;

// Included this since this program writes to a file.
using System.IO;

// Included this to send emails
using System.Net.Mail;

// Included this to use regular expressions
using System.Text.RegularExpressions;

// Included this for graphing
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    
    public partial class FreezeDetectorForm : Form
    {
        //////////////////// VARIABLES /////////////////////////

        // The number of thermocouple input channels, set to 8 by default.
        private static int CHANCOUNT = 8;

        // The max. number of thermocouple input channels supported.
        private const int MAX_CHANCOUNT = 8;
        
        // Give the device a name. Valid names are USB-TC and TC.
        private const string DEVICE = "TC";
        
        // This stores the number given to the USB-TC board. The number
        // given is usually 0 by default.
        private static int BoardNum;

        // This is an array of sample temperatures in degree celsius. This
        // is the array used for logging the temperature only.
        private static float[] TempData = new float[8];

        // Default number of samples is 8.
        private static int NumSamples = 8;

        // String variable used to to format date_time to a string.
        private string format = " M_d_yy @ h.mm tt";

        // String variable used for complete file directory and name.
        private string directory = "";

        // String variable used for sending emails.
        private string directory_copy = "";

        // String variable used for file folder.
        private string filepath = "";

        // String variable used for file name.
        private string filename = "";

        // Regular expression to check email addresses
        private const string permissive_email_reg_ex =
                        @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+" +
                        @"(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)" +
                        @"*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+" +
                        @"[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        // These are global variables used to check button click states.
        // 0 == untouched, never been clicked
        // 1 == clicked state
        // 2 == unclicked state
        // -1 == end program state
        private static int tune_check = 0;
        private static int start_check = 0;
        private static int stop_check = 0;
        
        // This checks if user has input enough information to begin
        // tuning the experiment
        private static bool tuning_ready = false;

        // This checks if tuning appears to have been completed properly
        private static bool all_frozen = false;

        // This checks if samples have not gone from frozen to liquid
        private static bool reset_all_frozen = true;
        
        // This is the number of minutes after freezing that the program
        // should shut down
        private int ShutDownMinutes = -9999;

        // This checks if the program is closing
        private static bool close_program_inprogress = false;

        // These variables are for the email function
        private SmtpClient client = new SmtpClient();
        private MailMessage message = new MailMessage();
        private System.Net.NetworkCredential SMTP_Creds = new
           System.Net.NetworkCredential("rapid.beta.tester@gmail.com",
           "igfoup666");

        // Variables to keep track of time elapsed
        private DateTime Start_Time = new DateTime();
        private DateTime Frozen_Time = new DateTime();
        private System.TimeSpan TimeSpan = new System.TimeSpan();
        private System.TimeSpan FrozenTimeSpan = new System.TimeSpan();

        // This thread handles displaying temperatures and TTL State.
        private Thread display_thread;

        // This thread is used for tuning loop.
        private Thread tuning_thread;

        // This thread handles the data logging during the cooling
        // process.
        private Thread log_thread;

        // Variable that keeps track of the samples' states
        private double[] SamplesStates = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
        // Sample state = 0 means that the sample is not being run
        // Sample state = 1.0 means that the sample is still liquid
        // Sample state = 1.2 means that the sample is frozen

        // Variable to see if sample switches from frozen to liquid state repeatedly
        private bool[] IsSampleFrozen = {false, false, false, false, false, false, false, false};

        // Color array used for graphing
        private Color[] GraphColors = new Color[] {Color.DarkOrange, Color.LawnGreen, Color.Red, 
                                                 Color.Yellow, Color.DarkGreen, Color.White, 
                                                 Color.Blue, Color.HotPink};



        //////////////////// DELEGATES /////////////////////////


        // A delegate used to update the temperature readouts.
        public delegate void Update_Temperature(float[] TempData);

        // A delegate used for tuning.
        public delegate void Update_Freeze(DigitalLogicState[] TuneData);

        
        //////////////////// FUNCTIONS /////////////////////////

        // Necessary constructor for the program to load properly.
        public FreezeDetectorForm()
        {
            InitializeComponent();
        }

        // This function detects the board number. The two parameters of this
        // function are the device name and a TextBox object that holds the
        // value of the board number (which is set when the board is detected by
        // Instacal). It creates a board object with a board number from 0 to 99
        // (since that is the range of MCC board numbers) and checks if that  
        // board number corresponds to the USB-TC device. It is important that
        // the variable DEVICE is set to "TC" or "USB-TC", since that is what
        // InstaCal names the USB-TC device when it is detected. If no board is
        // found, a value of -1 is returned.
        public static int GetBoardNum(string dev, TextBox BoardNumBox)
        {
            for (int BoardNum = 0; BoardNum < 99; BoardNum++)
            {
                MccDaq.MccBoard daq = new MccDaq.MccBoard(BoardNum);

                if (daq.BoardName.Contains(dev))
                {
                    BoardNumBox.Text = " " + BoardNum.ToString();
                    return BoardNum;
                }
            }
            return -1;
        }

        // This function changes CHANCOUNT, by detecting the actual number of
        // working thermocouple inputs connected to the USB-TC board.
        public void TC_finder()
        {
            MccBoard daq = new MccDaq.MccBoard(BoardNum);
            MccDaq.ErrorInfo RetVal;

            CHANCOUNT = MAX_CHANCOUNT;

            float[] temp = new float[MAX_CHANCOUNT];

            try
            {
                for (int i = 0; i < MAX_CHANCOUNT; i++)
                {
                    RetVal = daq.TIn(i, TempScale.Celsius,
                        out temp[i], ThermocoupleOptions.Filter);

                    if (RetVal.Value != 0)
                    {
                        CHANCOUNT--;  
                    }
                }

                if (CHANCOUNT <= 0)
                {
                    MessageBox.Show("No thermocouples detected!", "   " + "ERROR!");
                    return;
                }

                NumThermocouplesBox.Text = " " + CHANCOUNT.ToString();

                return;
            }

            catch
            {
                MessageBox.Show("Function TC_finder has an error!", "   " + "ERROR!");
                return;
            }
        }

        // This function updates the GUI's temperature readout and graphs
        public void update_gui()
        {
            while (start_check != 3)
            {
                float[] TempData = new float[MAX_CHANCOUNT];

                MccBoard daq = new MccDaq.MccBoard(BoardNum);
                MccDaq.ErrorInfo RetVal;

                Update_Temperature update_temp_del = new
                    Update_Temperature(update_gui_helper);

                for (int i = 0; i < MAX_CHANCOUNT; i++)
                {
                    RetVal = daq.TIn(i, TempScale.Celsius,
                                out TempData[i],
                                ThermocoupleOptions.Filter);
                }

                Invoke(update_temp_del, TempData);

                Thread.Sleep(500);
            }
        }

        // This function updates the  values and colors for the temperature 
        // readout and the times elapsed for the oven run. It uses a for loop to
        // set the values of the textboxes that shows the temperature values. It
        // shows an error using a message box.
        public void update_gui_helper(float[] TempData)
        {
            double Time;

            if (start_check == 1)
            {
                try
                {
                    for (int i = 1; i <= NumSamples; i++)
                    {
                        this.Controls["SampleBox" + i].Font =
                            new Font(this.Controls["SampleBox" + i].Font.FontFamily, 18);
                        this.Controls["SampleBox" + i].ForeColor = Color.LawnGreen;
                        this.Controls["SampleBox" + i].Text = "  " +
                            ((TempData[(i - 1)])).ToString("#0.00");
                    }

                    // Graphing section:

                    TimeSpan = DateTime.Now.Subtract(Start_Time);
                    FrozenTimeSpan = DateTime.Now.Subtract(Frozen_Time);

                    if (all_frozen == true)
                    {
                        if ((int)(FrozenTimeSpan.TotalMinutes) >= ShutDownMinutes
                            && ShutDownMinutes != -9999)
                        {
                            Rectangle bounds = Screen.GetBounds(Point.Empty);
                            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                            {
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                                }
                                bitmap.Save(directory_copy + ".png");
                            }

                            // Stop logging
                            start_check = 0;
                            log_thread.Abort();

                            Thread.Sleep(500);

                            SendEmail("Freezing run complete!", "See attached data", 
                                directory_copy + ".png", directory);
                            
                            StopThreads_AndCloseProgram();
                        }
                    }

                    TimeBox.Text = TimeSpan.ToString(@"h\:mm\:ss");

                    Time = TimeSpan.TotalMinutes;
                    
                    for (int i = 1; i <= NumSamples; i++)
                    {
                        TempGraph.Series[this.Controls["NameBox" + i].Text].Color = GraphColors[(i-1)];
                        TempGraph.Series[this.Controls["NameBox" + i].Text].Points.AddXY(Time, TempData[(i-1)]);

                        TempGraph.Series[this.Controls["NameBox" + i].Text + " State"].Color = GraphColors[(i-1)];
                        TempGraph.Series[this.Controls["NameBox" + i].Text + " State"].Points.AddXY(Time,
                            SamplesStates[(i - 1)]*30 + i * 0.5);
                    }

                    return;
                }

                catch
                {
                    MessageBox.Show("update_gui_helper has an error!", "   " + "ERROR!");
                    return;
                }
            }
        }

        // This is the tuning function
        public void tuning()
        {
            try
            {
                while (stop_check != 1)
                {
                    MccBoard daq = new MccDaq.MccBoard(BoardNum);
                    MccDaq.ErrorInfo RetVal;

                    DigitalLogicState[] TuneData = new DigitalLogicState[MAX_CHANCOUNT];

                    Update_Freeze update_freeze_del = new
                        Update_Freeze(tuning_helper);

                    for (int i = 0; i < MAX_CHANCOUNT; i++)
                    {
                        RetVal = daq.DConfigBit(DigitalPortType.AuxPort, i, DigitalPortDirection.DigitalIn);
                        RetVal = daq.DBitIn(DigitalPortType.AuxPort, i, out TuneData[i]);
                    }

                    Invoke(update_freeze_del, TuneData);

                    Thread.Sleep(500);
                }

                return;
            }
            catch
            {
                return;
            }
        }

        // Tuning helper function
        public void tuning_helper(DigitalLogicState[] TuneData)
        {
            try
            {
                for (int i = 1; i <= NumSamples; i++)
                {
                    this.Controls["StateBox" + i].ForeColor = Color.LawnGreen;
                    if (TuneData[(i - 1)] == DigitalLogicState.High)
                    {
                        this.Controls["StateBox" + i].Text = "LIQUID";
                        this.Controls["InfoBox" + i].Text = null;
                        SamplesStates[(i - 1)] = 1.0;
                        IsSampleFrozen[(i - 1)] = false;
                    }
                    else
                    {
                        this.Controls["StateBox" + i].Text = "FROZEN";

                        if (start_check == 1)
                        {
                            if (IsSampleFrozen[(i - 1)] == false)
                            {
                                this.Controls["InfoBox" + i].Text = "at " +
                                TimeSpan.ToString(@"h\:mm\:ss");

                                IsSampleFrozen[(i - 1)] = true;
                            }

                            SamplesStates[(i - 1)] = 1.2;
                        }
                    }
                }

                if (start_check == 1)
                {
                    int count = 0;
                    for (int i = 0; i < NumSamples; i++)
                    {
                        if (SamplesStates[i] == 1.2)
                            count++;
                    }
                    if (count == NumSamples)
                    {
                        all_frozen = true;

                        if (reset_all_frozen == true)
                        {
                            Frozen_Time = DateTime.Now;
                            reset_all_frozen = false;
                        }
                    }
                    else
                    {
                        all_frozen = false;
                        reset_all_frozen = true;
                    }
                }
            }
            catch
            {
                MessageBox.Show("tuning_helper has an error!", "   " + "ERROR!");
                return;
            }
        }

        // Logging function
        public void update_log()
        {
            MccBoard daq = new MccDaq.MccBoard(BoardNum);
            MccDaq.ErrorInfo RetVal;
            float[] LogData = new float[NumSamples];
            System.TimeSpan TimeSpan = new System.TimeSpan();

            int interval = (int)(float.Parse(LogIntervalBox.Text)*1000);

            string text1 = "Time (in hours),";
            for (int i = 1; i <= NumSamples; i++)
                {
                    text1 += this.Controls["NameBox" + i].Text + ",";
                    text1 += this.Controls["NameBox" + i].Text + " Frozen?";
    
                    if (i < NumSamples)
                    {
                        text1 += ",";
                    }
                }
            using (StreamWriter sw = File.AppendText(directory))
            {
                sw.WriteLine(text1);
            }            

            while (start_check != 0)
            {
                TimeSpan = DateTime.Now.Subtract(Start_Time);
            
                string text2 = TimeSpan.TotalHours.ToString() + ",";
            
                for (int i = 0; i < NumSamples; i++)
                {
                    RetVal = daq.TIn(i, TempScale.Celsius,
                                out LogData[i], ThermocoupleOptions.Filter);

                    text2 += LogData[i].ToString() + ",";

                    if (SamplesStates[i] == 0)
                    {
                        text2 += "-";
                    }
                    else if (SamplesStates[i] == 1)
                    {
                        text2 += "No";
                    }
                    else
                    {
                        text2 += "Yes";
                    }

                    if (i < NumSamples - 1)
                    {
                        text2 += ",";
                    }
                }
            
                using (StreamWriter sw = File.AppendText(directory))
                {
                    sw.WriteLine(text2);
                }

                Thread.Sleep(interval);
            }
        }

        // This function checks the user input parameters and changes the
        // tuning_ready variable
        public void checkparameters()
        {
            int check = 0;

            if (String.IsNullOrWhiteSpace(UserNameBox.Text))
            {
                UserNameBox.BackColor = Color.MistyRose;
                check++;
            }
            else
                UserNameBox.BackColor = Color.White;

            try
            {
                String[] delimiters = new String[] { ",", ";" };
                String[] email_addresses = UserEmailBox.Text.Split(delimiters,
                StringSplitOptions.RemoveEmptyEntries);

                if (email_addresses.Length <= 0)
                {
                    throw new Exception();
                }

                foreach (string addr in email_addresses)
                {
                    if (!Regex.IsMatch(addr.Trim(), permissive_email_reg_ex))
                    {
                        throw new Exception();
                    }
                }
                UserEmailBox.BackColor = Color.White;
            }
            catch
            {
                UserEmailBox.BackColor = Color.MistyRose;
                check++;
            }

            if (NumSamplesBox.SelectedItem == null)
            {
                NumSamplesBox.BackColor = Color.MistyRose;
                check++;
            }
            else
            {
                NumSamplesBox.BackColor = default(Color);
                NumSamples = int.Parse(NumSamplesBox.SelectedItem.ToString());

                for (int i = 1; i <= NumSamples; i++)
                {
                    if (this.Controls["NameBox" + i].Text.Trim() == "")
                    {
                        this.Controls["NameBox" + i].BackColor = Color.MistyRose;
                        check++;
                    }
                    else
                    {
                        this.Controls["NameBox" + i].BackColor = default(Color);
                    }
                }
            }

            try
            {
                ShutDownMinutes = (int)ShutDownMinutesNumericUpDown.Value;
                ShutDownMinutesNumericUpDown.BackColor = default(Color);
            }
            catch
            {
                ShutDownMinutes = -9999;
                ShutDownMinutesNumericUpDown.BackColor = Color.MistyRose;
                check++;
            }

            if (String.IsNullOrWhiteSpace(DirectoryBox.Text))
            {
                DirectoryBox.BackColor = Color.MistyRose;
                check++;
            }
            else
            {
                DirectoryBox.BackColor = default(Color);
            }

            if (!string.IsNullOrWhiteSpace(LogIntervalBox.Text))
            {
                try
                {
                    float interval = float.Parse(LogIntervalBox.Text);
                    LogIntervalBox.BackColor = default(Color);
                }
                catch
                {
                    MessageBox.Show("Please only enter numbers for Log Interval");
                    LogIntervalBox.BackColor = Color.MistyRose;
                    check++;
                }
            }

            else
            {
                LogIntervalBox.BackColor = Color.MistyRose;
                check++;
            }

            if (check == 0)
            {
                tuning_ready = true;
            }

            return;
        }

        // This function is used to send out the oven notification emails.
        private void SendEmail(string subject, string body, string file1, string file2)
        {
            try
            {
                // SMPT host setup
                client.Host = "smtp.gmail.com";
                client.Port = 465;
                client.UseDefaultCredentials = false;
                client.Credentials = SMTP_Creds;
                client.EnableSsl = true;

                string temp = UserEmailBox.Text.Replace(" ", string.Empty);
                temp = temp.Replace(";", ",");

                MailAddress from =
                    new MailAddress("rapid.beta.tester@gmail.com");

                System.Net.Mail.Attachment graph;
                graph = new System.Net.Mail.Attachment(file1);

                System.Net.Mail.Attachment data;
                data = new System.Net.Mail.Attachment(file2);

                message.Attachments.Add(graph);
                message.Attachments.Add(data);

                message.Subject = subject;
                message.Body = body;
                message.From = from;
                message.To.Add(temp);
                client.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "   ERROR!");
            }
        }

        // This function closes the window and exits the program. It checks
        // if the threads are in the stopped state, and if not it stops them.
        public void StopThreads_AndCloseProgram()
        {
            this.Refresh();

            Thread.Sleep(1000);

            if ((log_thread != null && log_thread.ThreadState !=
                ThreadState.Stopped) || (display_thread != null &&
                display_thread.ThreadState != ThreadState.Stopped) ||
                (tuning_thread != null && tuning_thread.ThreadState
                != ThreadState.Stopped))
            {
                log_thread.Abort();
                display_thread.Abort();
                tuning_thread.Abort();

                tune_check = -1;
                start_check = -1;
                stop_check = -1;
            }

            int counter = 0;

            while ((log_thread != null && log_thread.ThreadState !=
                ThreadState.Stopped && log_thread.ThreadState !=
                ThreadState.Aborted) || (display_thread != null &&
                   display_thread.ThreadState != ThreadState.Stopped &&
                   display_thread.ThreadState != ThreadState.Aborted) ||
                   (tuning_thread != null && tuning_thread.ThreadState
                   != ThreadState.Stopped && tuning_thread.ThreadState !=
                   ThreadState.Aborted))
            {
                counter++;

                if (counter > 10)
                {
                    if (log_thread != null)
                        log_thread.Abort();
                    if (display_thread != null)
                        display_thread.Abort();
                    if (tuning_thread != null)
                        tuning_thread.Abort();
                }

                Thread.Sleep(100);
            }

            //Capture the current program window to image and save to downloads folder
            CaptureWindow_SaveToDownloadsFolderAsImageFile();

            this.Close();
        }

        // This function starts when the program is started. It calls the  
        // function that detects the USB-TC device and finds the board by its 
        // number. The threads are started in this function.
        private void FreezeDetectorForm_Load(object sender, EventArgs e)
        {
            // Locate the USB-TC and give it a number.
            BoardNum = GetBoardNum(DEVICE, BoardNumBox);

            if (BoardNum == -1)
            {
                MessageBox.Show("No USB-TC detected! Restart program after " + 
                    "locating the board");
                // Show a message box if USB device not found.

                return;
            }

            // Detect number of connected thermocouples
            TC_finder();

            // Setup graph
            TempGraph.ChartAreas[0].BackColor = Color.Black;
            TempGraph.ChartAreas[0].AxisX.MajorGrid.LineColor =
                Color.DarkSlateGray;
            TempGraph.ChartAreas[0].AxisY.MajorGrid.LineColor =
                Color.DarkSlateGray;

            TempGraph.ChartAreas[0].AxisX.Minimum = 0;
            
        }
        
        // The functions below handle button presses by changing the state
        // variables
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (tune_check == 0 || tune_check == 1) // if tuning is not complete yet
            {
                MessageBox.Show("Click Tune and finish tuning first!");
                return;
            }
 
            if (start_check == 2 || start_check == 0)
            {
                if (start_check == 0)
                {
                    TuneButton.BackColor = default(Color);
                    tune_check = 2;

                    NumSamples = int.Parse(NumSamplesBox.SelectedItem.ToString());
                    
                    for (int i = 1; i <= NumSamples; i++)
                    {
                        TempGraph.Series.Add(this.Controls["NameBox" + i].Text);
                        TempGraph.Series[this.Controls["NameBox" + i].Text].ChartType = SeriesChartType.FastLine;
                        TempGraph.Series[this.Controls["NameBox" + i].Text].IsVisibleInLegend = false;
                        TempGraph.Series[this.Controls["NameBox" + i].Text].Points.AddXY(0, 0);

                        TempGraph.Series.Add(this.Controls["NameBox" + i].Text + " State");
                        TempGraph.Series[this.Controls["NameBox" + i].Text + " State"].ChartType = SeriesChartType.FastLine;
                        TempGraph.Series[this.Controls["NameBox" + i].Text + " State"].IsVisibleInLegend = false;
                        TempGraph.Series[this.Controls["NameBox" + i].Text + " State"].Points.AddXY(0, 0);
                    
                        LegendItem customItem = new LegendItem();
                        customItem.Name = this.Controls["NameBox" + i].Text;
                        customItem.Color = GraphColors[(i - 1)];
                        customItem.ImageStyle = LegendImageStyle.Marker;
                        customItem.MarkerStyle = MarkerStyle.Square;
                        customItem.MarkerSize = 30;
                        customItem.BorderWidth = 0;
                        TempGraph.Legends[0].CustomItems.Add(customItem);
                    }
                    
                    display_thread = new Thread(new ThreadStart(update_gui));
                    Start_Time = DateTime.Now;
                    display_thread.Start();
                    log_thread = new Thread(new ThreadStart(update_log));
                    log_thread.Start();
                }

                start_check = 1;
                StartButton.BackColor = Color.LightGreen;
                TuneButton.BackColor = default(Color);

                if (stop_check == 1)
                {
                    stop_check = 2;
                    StopButton.BackColor = default(Color);
                }
            }
            return;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (start_check == 1)
            {
                start_check = 2;
                StartButton.BackColor = default(Color);
                stop_check = 1;
                StopButton.BackColor = Color.LightGreen;
            }

            return;
        }

        private void TuneButton_Click(object sender, EventArgs e)
        {
            checkparameters(); // Check if paramemters are okay before tuning

            if (tuning_ready)
            {
                if (tune_check == 1)
                {
                    TuneButton.BackColor = default(Color);
                    tune_check = 2;
                }

                if (tune_check == 0)
                {
                    TuneButton.BackColor = Color.LightGreen;
                    tuning_thread = new Thread(new ThreadStart(tuning));
                    tuning_thread.Start();
                    tune_check = 1;
                }
            }

            else
            {
                MessageBox.Show("Please complete the highlighted fields!");
            }
            return;
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Freeze Detector 1.0 by Harry Golash" +
                "\n\nAn application that detects freezing in liquid samples through" +
           " LED and photodiode sensors and graphs their cooling curves simultaneously." +
            "\n\nKirschvink Lab\nCalifornia Institute of Technology\n2014",
           "About");
            return;
        }

        private void MyHelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not ready yet!");
            return;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            if (close_program_inprogress)
                return;

            close_program_inprogress = true;

            DialogResult dialogResult = MessageBox.Show("WARNING! This is" +
                        " will exit the program and shut down all ongoing processes"
                        + " Are you sure you want to exit?", "   " +
                        "Exit Application?", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.No)
            {
                close_program_inprogress = false;
                return;
            }
            
            StopThreads_AndCloseProgram();
        }

        private void DirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "Select a location in which the temperature log" +
                " files will be saved.";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryBox.BackColor = Color.White;

                filepath = fbd.SelectedPath;

                filename = "Freeze_run_" + DateTime.Now.ToString(format);

                directory_copy = Path.Combine(filepath, filename);

                directory = directory_copy + ".csv";

                DirectoryBox.Text = directory;
            }   
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            string time_var;
            time_var = DateTime.Now.ToString(format);

            UserNameBox.Text = "Atsuko";
            UserEmailBox.Text = "kobayashi.a.an@m.titech.ac.jp";
            filepath = @"C:\Users\Atsuko Kobayashi\Desktop\Cryo Experiment data\New folder test\";
            filename = "test_" + time_var;
            DirectoryBox.Text = Path.Combine(filepath, filename + ".csv");
            directory = DirectoryBox.Text;
            LogIntervalBox.Text = "1";
            NameBox1.Text = "1";
            NameBox2.Text = "2";
            NameBox3.Text = "3";
            NameBox4.Text = "4";
            NameBox5.Text = "5";
            NameBox6.Text = "6";
            NameBox7.Text = "7";
            NameBox8.Text = "8";
            NumSamplesBox.SelectedIndex = 7;
        }

        private void CaptureWindow_SaveToDownloadsFolderAsImageFile()
        {
            try
            {
                //Create a new bitmap.
                var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                               Screen.PrimaryScreen.Bounds.Height,
                                               PixelFormat.Format32bppArgb);

                // Create a graphics object from the bitmap.
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                            Screen.PrimaryScreen.Bounds.Y,
                                            0,
                                            0,
                                            Screen.PrimaryScreen.Bounds.Size,
                                            CopyPixelOperation.SourceCopy);

                //Generate save file path for Screen Shot
                string image_file_name = filename;
                string image_file_path =
                    Path.Combine(
                        filepath,
                        String.Format(
                            "ScreenShot_{0}.png",
                            image_file_name));

                // Save the screenshot to the specified path that the user has chosen.
                bmpScreenshot.Save(image_file_path, ImageFormat.Png);
            }
            catch (Exception screen_capture_exception)
            {
                MessageBox.Show(
                    String.Format(
                        "{0}{1}{1}{2}{1}{3}{1}{4}",
                        "Error Capturing Screen and saving as image file.",
                        Environment.NewLine,
                        "Message: " + screen_capture_exception.Message,
                        "Source: " + screen_capture_exception.Source,
                        "Stack Trace: " + screen_capture_exception.StackTrace),
                    "Screen Capture Error");
            }
        }
    }
}
