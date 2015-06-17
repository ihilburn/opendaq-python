// These were included by default after creating the windows form display.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// Included MccDaq to use the Universal Library functions.
using MccDaq;

// Included this since this program uses threads.
using System.Threading;

// Included this since this program writes to a file.
using System.IO;

// Included this to send emails
using System.Net.Mail;


namespace USB_TC_Control
{
    public partial class USB_TC_Control : Form
    {
        public delegate void SetHeatingElementStatusButtonCallback(OvenChanStatusEnum oven_element);

        public static Color ElementOnColor = Color.Red;
        public static Color ElementOffColor = Color.DarkGray;

        // The number of thermocouple input channels, set to 8 by default.
        public static int CHANCOUNT = 8;

        // The max. number of thermocouple input channels supported.
        public const int MAX_CHANCOUNT = 8;

        // Give the device a name. Valid names are USB-TC and TC.
        public const string DEVICE = "TC";
        
        // Set default temperature scale to degree Celsius.
        public static TempScale my_TempScale = TempScale.Celsius;

        // This stores the number given to the USB-TC board. The number
        // given is usually 0 by default.
        public static int BoardNum;

        // This is a copy of the TempData[] array in the degree celsius
        // scale. This is the array used for logging the temperature only.
        public static float[] LogData = new float[MAX_CHANCOUNT] ;
                
        // This is a global variable used to check if the thermocouples' 
        // readings are being displayed. 
        public static int read_check = 0;

        // This is a global variable used to set oven state by button clicks.
        public static int oven_check = 0;

        // This is a global variable used in graphing to check the oven state.
        public static int graph_check = 2;

        // This is a global variable used to set oven state by button clicks.
        public static int log_check = 0;

        // This is a global variable used in updating the temperature log.
        public static int log_state = 0;

        // This is a global variable to check if temp_read_thread is started. The
        // log_thread starts with the temp_read_thread.
        public static int temp_read_check = 0;

        // This is a global variable to check if oven_control_thread is started.
        public static int oven_start_check = 0;

        // This variable is used in updating the Warning box
        public static int warning_time = 20;

        // This variable is used as a flag to let error processor know if an 
        // error window is already open.
        public static Boolean error_window_open = false;

        // This variable is used as a flag for aborting the oven code
        // If true, the code will call a method to stop all of the threads and 
        // end the program; if false, the code will continue it's execution
        public static bool abort_oven_program = false;

        // Global boolean flag to let the code know that the method used to
        // close the program and stop all of the threads has been called already.
        public static bool close_program_inprogress = false;

        // This variable is used to make sure that all checkboxes are not
        // unchecked during an oven run (flying blind)
        public bool not_showing_temp = false;

        // An enumerator 
        public enum OvenChanStatusEnum
        { 
            all_off = 0x00,
            oven_on = 0x01,
            oven_off = 0x02,
            inner = 0x04,
            SZ = 0x08,
            outer = 0x10,
        }

        public enum AirChanStatusEnum
        {
            air_off = 0x00,
            low_n2 = 0x01,
            high_n2 = 0x02,
            air = 0x04
        }

        // This thread handles the reading and displaying of thermocouples.
        private Thread temp_read_thread;

        // This thread handles the oven heating coils and gas cooling valves.
        private Thread oven_control_thread;

        // This thread handles the temperature and error logging during the
        // heating process.
        private Thread log_thread;

        // String variable used for complete file directory and name.
        public string directory = "";

        // String variable used for file folder.
        public string filepath = "";

        // String variable used for file name.
        public string filename = "";

        // DateTime object used to log the date and time logging started.
        public DateTime date_time = new DateTime();

        // String variable used to to format date_time to a string.
        public string format = " M_d_yy @ h.mm tt";

        // String variable that stores the choice of cooling (air / nitrogen).
        public string CoolingMethod = "";

        public static AirChanStatusEnum air_status = AirChanStatusEnum.air_off;
        public static OvenChanStatusEnum oven_status = OvenChanStatusEnum.all_off;

        // Regular expression to check email addresses
        public const string permissive_email_reg_ex = 
                        @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+" + 
                        @"(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)" +
                        @"*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+" + 
                        @"[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        // Variables that are used for logging and keeping track of oven state.
        public bool HeatingStarted = false;
        public bool HeatingStopped = false;
        public bool MaxTempReached = false;
        public bool CoolingStarted = false;
        public bool IsCriticalError = false;
        public bool OvenRunDone = false;

        public bool HeatingStarted_logged = false;
        public bool HeatingStopped_logged = false;
        public bool MaxTempReached_logged = false;
        public bool CoolingStarted_logged = false;
        public bool IsCriticalError_logged = false;
        public bool OvenRunDone_logged = false;

        public string CriticalErrorMessage = "";
        
        // Variables that are used by the is_parameters_changed() function
        public string User_copy = "";
        public string UserEmail_copy = "";
        public string UserPhone_copy = "";
        public string BatchCode_copy = "";
        public string MaxTemp_copy = "";
        public string HoldTime_copy = "";
        public string CoolMethod_copy = "";
        public string directory_copy = "";
        public string OvenName_copy = "";
        public string SwitchTemp_copy = "";
        public string OvenStartTime_copy = "";
        public string TimeElapsed_copy = "";
        public string HoldStartTime_copy = "";
        public string HoldTimeRemaining_copy = "";
        public string CoolStartTime_copy = "";
        public string CoolTimeElapsed_copy = "";

        // These variables are for the email function
        public SmtpClient client = new SmtpClient();
        public MailMessage message = new MailMessage();
        
        public System.Net.NetworkCredential SMTP_Creds = new
            System.Net.NetworkCredential("lab.demagnetization.oven@gmail.com",
            "wholegrain");

        // Variable to keep track of oven run time
        DateTime Start_Oven_Time = new DateTime();

        // Variable to keep track of heating time
        DateTime Heat_Time = new DateTime();
            
        // Variable to keep track of the TC read time
        DateTime Start_TC_Time = new DateTime();

        // A delegate used to the update the temperature readouts.
        public delegate void Update_Temperature(float[] TempData, float[] TempData_C);

        // A delegate used to the update the WARNING box.
        public delegate void Update_Warning(string Warning, int ErrorCode);

        // A delegate used to the reset the checkboxes.
        public delegate void Reset_Checkboxes();

        //A delegate used to trigger the close oven program process;
        public delegate void AbortOvenCode();
        
        // Necessary constructor for the program to load properly.
        public USB_TC_Control()
        {
            InitializeComponent();

            SetHeatingElementStatusButtonCallback SetHeatingElementStatus;
        }

        public void SetHeatingElementStatus(OvenChanStatusEnum oven_element)
        {
            bool outer_status = false;
            bool sz_status = false;
            bool inner_status = false;
            
            switch (oven_element)
            {
                case OvenChanStatusEnum.all_off:
                case OvenChanStatusEnum.oven_off:
                    outer_status = false;
                    sz_status = false;
                    inner_status = false;
                    break;
                
                case OvenChanStatusEnum.inner:
                    inner_status = true;
                    break;

                case OvenChanStatusEnum.outer:
                    outer_status = true;
                    break;

                case OvenChanStatusEnum.SZ:
                    sz_status = true;
                    break;

                case OvenChanStatusEnum.oven_on:
                    outer_status = true;
                    sz_status = true;
                    inner_status = true;
                    break;

                default:
                    //exit function
                    return;
            }

            if (outer_status) 
                SetOuterIndicatorToOn();
            else
                SetOuterIndicatorToOff();

            if (inner_status) 
                SetInnerIndicatorToOn();
            else
                SetInnerIndicatorToOff();

            if (sz_status) 
                SetSZIndicatorToOn();
            else
                SetSZIndicatorToOff();


        }

        private void SetOuterIndicatorToOn()
        {
            if (btnOuter_HeatingElementStatus.InvokeRequired)
            {
                btnOuter_HeatingElementStatus.BeginInvoke(
                    ((Action)(() => btnOuter_HeatingElementStatus.BackColor = ElementOnColor)));
            }
            else
            {
                btnOuter_HeatingElementStatus.BackColor = ElementOnColor;
            }
        }

        private void SetOuterIndicatorToOff()
        {
            if (btnOuter_HeatingElementStatus.InvokeRequired)
            {
                btnOuter_HeatingElementStatus.BeginInvoke(
                    ((Action)(() => btnOuter_HeatingElementStatus.BackColor = ElementOffColor)));
            }
            else
            {
                btnOuter_HeatingElementStatus.BackColor = ElementOffColor;
            }
        }

        private void SetInnerIndicatorToOn()
        {
            if (btnOuter_HeatingElementStatus.InvokeRequired)
            {
                btnInner_HeatingElementStatus.BeginInvoke(
                    ((Action)(() => btnInner_HeatingElementStatus.BackColor = ElementOnColor)));
            }
            else
            {
                btnInner_HeatingElementStatus.BackColor = ElementOnColor;
            }
        }

        private void SetInnerIndicatorToOff()
        {
            if (btnInner_HeatingElementStatus.InvokeRequired)
            {
                btnInner_HeatingElementStatus.BeginInvoke(
                    ((Action)(() => btnInner_HeatingElementStatus.BackColor = ElementOffColor)));
            }
            else
            {
                btnInner_HeatingElementStatus.BackColor = ElementOffColor;
            }
        }

        private void SetSZIndicatorToOn()
        {
            if (btnSZ_HeatingElementStatus.InvokeRequired)
            {
                btnSZ_HeatingElementStatus.BeginInvoke(
                    ((Action)(() => btnSZ_HeatingElementStatus.BackColor = ElementOnColor)));
            }
            else
            {
                btnSZ_HeatingElementStatus.BackColor = ElementOnColor;
            }
        }

        private void SetSZIndicatorToOff()
        {
            if (btnSZ_HeatingElementStatus.InvokeRequired)
            {
                btnSZ_HeatingElementStatus.BeginInvoke(
                    ((Action)(() => btnSZ_HeatingElementStatus.BackColor = ElementOffColor)));
            }
            else
            {
                btnSZ_HeatingElementStatus.BackColor = ElementOffColor;
            }
        }

        // This function starts when the program is started. It calls the  
        // function that detects the USB-TC device and finds the board by its 
        // number. The threads are started in this function.
        private void USB_TC_Control_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult user_resp = System.Windows.Forms.DialogResult.None;

            do
            {

                // Locate the USB-TC and give it a number.
                BoardNum = GetBoardNum(DEVICE, BNum);

                if (BoardNum == -1)
                {
                    user_resp =
                        MessageBox.Show(String.Format("No USB-{0} detected! "
                                        + "Click 'Cancel' to exit out of the oven program.",
                                        DEVICE),
                                        "Comm Error",
                                        MessageBoxButtons.RetryCancel);

                    // Throw an error message if no USB-TC device is found. It is
                    // recommended to first locate and caliberate the board in
                    // InstaCal before use.

                    //Give user the option to exit from the code in the modal error message window.
                    if (user_resp == System.Windows.Forms.DialogResult.Cancel)
                    {
                        StopThreads_AndCloseOvenProgram();
                        return;
                    }
                }
                else
                {
                    // Set default value for the button press detection variables.
                    read_check = 0;
                    oven_check = 0;
                    log_check = 0;
                }

            } while (user_resp == System.Windows.Forms.DialogResult.Retry);


            // Detect number of connected thermocouples
            TC_finder();


            // Default batch code
            BatchCode.Text = "_";

            // Default user
            User.Text = "User";

            // Default email
            UserEmail.Text = "user@emailaddress.com";

            // Default phone
            UserPhone.Text = "111-222-3333";

            // Default max. temp
            MaxTemp.Text = "";

            // Default hold time
            HoldTime.Text = "";

            // Default cooling method is Nitrogen
            CoolMethod.SelectedIndex = 1;

            // Setup graph
            TempGraph.ChartAreas[0].BackColor = Color.Black;
            TempGraph.ChartAreas[0].AxisX.MajorGrid.LineColor =
                Color.DarkSlateGray;
            TempGraph.ChartAreas[0].AxisY.MajorGrid.LineColor =
                Color.DarkSlateGray;

            TempGraph.ChartAreas[0].AxisX.Minimum = 0;
            TempGraph.ChartAreas[0].AxisY.Minimum = 0;

            TempGraph.Series["Tray Outer"].Points.AddXY(0, 0);
            TempGraph.Series["Tray SZ"].Points.AddXY(0, 0);
            TempGraph.Series["Tray Inner"].Points.AddXY(0, 0);
            TempGraph.Series["Outer"].Points.AddXY(0, 0);
            TempGraph.Series["Inner"].Points.AddXY(0, 0);
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
        public static int GetBoardNum(string dev, TextBox BNum)
        {
            for (int BoardNum = 0; BoardNum < 99; BoardNum++)
            {
                MccDaq.MccBoard daq = new MccDaq.MccBoard(BoardNum);

                if (daq.BoardName.Contains(dev))
                {
                    BNum.Text = " " + BoardNum.ToString();
                    
                    return BoardNum;
                }
            }
            return -1;
        }
        
        // This function checks for any error by checking the value of an
        // ErrorInfo object. If there is an error, it displays the error 
        // message in a message box and returns 1. If there is no error, the
        // function just returns 0.
        public static int IsError(ErrorInfo e)
        {
            if (abort_oven_program) return 1;

            while (error_window_open)
            {
                Thread.Sleep(20);
            }

            if (abort_oven_program) return 1;
            
            if (e.Value != 0)
            {
                error_window_open = true;
                MessageBox.Show(e.Message);
                error_window_open = false;
                return 1;
            }
            return 0;
        }

        // This function checks for any error by checking the value of an
        // ErrorInfo object. If there is an error, it displays the error 
        // message in a message box and returns 1. If there is no error, the
        // function just returns 0.
        public static int IsError(Object sender, ErrorInfo e, Boolean allow_user_to_exit_program)
        {
            if (allow_user_to_exit_program)
            {
                return IsError_WithExitOption(sender, e);
            }
            else
            {
                return IsError(e);
            }
        }

        // This function checks for any error by checking the value of an
        // ErrorInfo object. If there is an error, it displays the error 
        // message in a message box and returns 1. If there is no error, the
        // function just returns 0.
        public static int IsError_WithExitOption(object sender, ErrorInfo e)
        {
            if (abort_oven_program) return 1;
            
            while (error_window_open)
            {
                Thread.Sleep(20);
            }

            if (abort_oven_program) return 1;

            try
            {
                USB_TC_Control oven_form = (USB_TC_Control)sender;

                AbortOvenCode abortcode_del = new AbortOvenCode(oven_form.StopThreads_AndCloseOvenProgram);

                if (e.Value != 0)
                {
                    System.Windows.Forms.DialogResult user_resp = DialogResult.None;

                    error_window_open = true;
                    user_resp = MessageBox.Show(e.Message + Environment.NewLine +
                                                "Click 'Cancel' to exit out of the oven program",
                                                "Error",
                                                MessageBoxButtons.OKCancel);
                    
                    if (user_resp == DialogResult.Cancel)
                    {
                        abort_oven_program = true;
                        
                        //Set check flags for the three threads (Oven PID Control, Read Temp, Log Temp)
                        //To -1 to end those three threads
                        oven_check = -1;
                        read_check = -1;
                        log_check = -1;

                        //Pause 100 msecs
                        Thread.Sleep(100);

                        //Invoke the close/abort program delegate
                        oven_form.Invoke(abortcode_del);
                    }

                    error_window_open = false;

                    return 1;
                }
            }
            catch
            {
                //Call the single argument error handler
                IsError(e);
            }
            return 0;
        }

        // This function is called when the Read TC button in the GUI is
        // clicked. It sets the value of the global variable check to 1, and
        // changes the Read TC button color to Green and resets the color of
        // the Stop TC button to default, so that the user knows which button
        // was last clicked.
        private void ReadTCbutton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(WarningBox.Text))
            {
                if (not_showing_temp == true)
                    resetcheckboxes();
                update_warning("", 0);
            }

            if (temp_read_check == 0 && oven_start_check == 0)
                resetboxes();

            if (checkparameters("TC") != 0)
            {
                SaveDir.BackColor = Color.MistyRose;
                MessageBox.Show("Check and complete highlighted thermocouple" +
                    " read parameters!", "   ERROR!");
                return;
            }

            if (String.IsNullOrWhiteSpace(directory))
            {
                SaveDir.BackColor = Color.MistyRose;

                MessageBox.Show("Please choose a directory to save log file in!"
                + "\nAlso add other (optional) parameters, like the batch " +
                "code and email address...", "   " + "ERROR!");

                return;
            }
            else
                SaveDir.BackColor = Color.White;

            if (is_parameters_changed())
            {
                update_directory();
                MessageBox.Show("Please double check all values, just to be " +
            "sure. Click the Read TC button when ready.", "   Check Values");
                
                copy_parameters();
                return;
            }

            User.ReadOnly = true;
            User.BorderStyle = BorderStyle.FixedSingle;
            BatchCode.ReadOnly = true;
            BatchCode.BorderStyle = BorderStyle.FixedSingle;
            OvenName.ReadOnly = true;
            OvenName.BorderStyle = BorderStyle.FixedSingle;
            UserEmail.Enabled = false;
            UserPhone.Enabled = false;
            SwitchTemp.Enabled = false;
            MaxTemp.Enabled = false;
            HoldTime.Enabled = false;
            CoolMethod.Enabled = false;
            
            UserPhone.BackColor = Color.White;
            
            // Create and start the threads when the Read TC button is clicked
            // for the first time.
            if (temp_read_check == 0)
            {
                copy_parameters();

                temp_read_thread = new Thread(new ThreadStart(TC_reader));
                temp_read_thread.Start();
                log_thread = new Thread(new ThreadStart(update_log));
                log_thread.Start();
               
                temp_read_check = 1;
            }

            if (is_parameters_changed())
                update_directory();

            if (read_check != 1)
                Start_TC_Time = DateTime.Now;

            read_check = 1;

            warning_time = 20;

            ReadTCbutton.BackColor = Color.GreenYellow;
            StopTCbutton.BackColor = default(Color);
        }

        // This function is called when the Stop TC button in the GUI is
        // clicked. It sets the value of the global variable check to 2, and
        // changes the Stop TC button color to Green and resets the color of
        // the Read TC button to default, so that the user knows which button 
        // was last clicked.
        private void StopTCbutton_Click(object sender, EventArgs e)
        {
            if (oven_check == 0)
            {
                UserEmail.Enabled = true;
                UserPhone.Enabled = true;
                MaxTemp.Enabled = true;
                HoldTime.Enabled = true;
                CoolMethod.Enabled = true;
                SwitchTemp.Enabled = true;
            }

            read_check = 0;

            ReadTCbutton.BackColor = default(Color);
            
            for (int i = 0; i < MAX_CHANCOUNT; i++)
            {
                this.Controls["Ch" + i.ToString("#0")].ForeColor = Color.Gray;
            }
            
            if (temp_read_check != 0)
                StopTCbutton.BackColor = Color.GreenYellow;            
        }

        // This function is called when the Start Oven button in the GUI is
        // clicked. It sets the value of the global variable check to 3, and
        // changes the Start Oven button color to Green.
        private void StartOvenButton_Click_1(object sender, EventArgs e)
        {
            UserEmail.Enabled = true;
            UserPhone.Enabled = true;
            MaxTemp.Enabled = true;
            HoldTime.Enabled = true;
            CoolMethod.Enabled = true;
            SwitchTemp.Enabled = true;
            
            if (checkparameters("oven") != 0)
            {
                if (String.IsNullOrWhiteSpace(directory))
                    SaveDir.BackColor = Color.MistyRose;
                else
                    SaveDir.BackColor = Color.White;

                MessageBox.Show("Check and complete highlighted oven run" +
                    " parameters!", "   ERROR!");
                return;
            }
            
            if (String.IsNullOrWhiteSpace(directory))
            {
                SaveDir.BackColor = Color.MistyRose;
                MessageBox.Show("Please choose a directory to save log " +
                    "file in!", "   " + "ERROR!");
                return;
            }
            else
                SaveDir.BackColor = Color.White;

            if (is_parameters_changed() || 
                (temp_read_check == 1 && oven_start_check == 0)) 
            {
                update_directory();
                MessageBox.Show("Please double check all values, just to be " +
            "sure. Click the Start Oven button when ready.", "   Check Values");
                    
                copy_parameters();
                oven_start_check = 1;
                return;
            }

            User.ReadOnly = true;
            User.BorderStyle = BorderStyle.FixedSingle;
            UserEmail.ReadOnly = true;
            UserEmail.BorderStyle = BorderStyle.FixedSingle;
            UserPhone.ReadOnly = true;
            UserPhone.BorderStyle = BorderStyle.FixedSingle;
            UserPhone.BackColor = Color.White;
            BatchCode.ReadOnly = true;
            BatchCode.BorderStyle = BorderStyle.FixedSingle;
            MaxTemp.ReadOnly = true;
            MaxTemp.BorderStyle = BorderStyle.FixedSingle;
            HoldTime.ReadOnly = true;
            HoldTime.BorderStyle = BorderStyle.FixedSingle;
            OvenName.ReadOnly = true;
            OvenName.BorderStyle = BorderStyle.FixedSingle;
            CoolMethod.Enabled = false;
            SwitchTemp.ReadOnly = true;
            SwitchTemp.BorderStyle = BorderStyle.FixedSingle;
            
            if (is_parameters_changed())
                update_directory();

            if (oven_check != 1)
            {
                Start_Oven_Time = DateTime.Now;
                OvenStartTime.Text = Start_Oven_Time.ToString("T");
                graph_check++;
            }
            
            copy_parameters();

            if (read_check == 0)
            {
                read_check = 1;
                ReadTCbutton.BackColor = Color.GreenYellow;
                StopTCbutton.BackColor = default(Color);
            }

            int count = 0;

            for (int i = 0; i < MAX_CHANCOUNT; i++)
            {
                if (((CheckBox)this.Controls["Ch" +
                    i.ToString("#0") + "Box"]).Enabled == true &&
                    ((CheckBox)this.Controls["Ch" +
                    i.ToString("#0") + "Box"]).Checked == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                not_showing_temp = true;
            }

            if (not_showing_temp == true)
            {
                resetcheckboxes();
                not_showing_temp = false;
            }

            // Start thread once the Start Oven button is clicked for the
            // first time.
            if (oven_start_check == 1)
            {
                oven_control_thread = new Thread(new ThreadStart(PID_control));
                oven_control_thread.Start();
                oven_start_check = 2;
                
                copy_parameters();

                if (temp_read_check == 0)
                {
                    temp_read_thread = new Thread(new ThreadStart(TC_reader));
                    log_thread = new Thread(new ThreadStart(update_log));
                    temp_read_thread.Start();
                    log_thread.Start();

                    temp_read_check = 1;

                    read_check = 1;
                    ReadTCbutton.BackColor = Color.GreenYellow;
                    StopTCbutton.BackColor = default(Color);
                }
            }

            oven_check = 1;
           
            StopOvenButton.BackColor = default(Color);
            StartOvenButton.BackColor = Color.GreenYellow;
        } 

        // This function is called when the Start Oven button in the GUI is
        // clicked. It sets the value of the global variable check to 3, and
        // changes the Start Oven button color to Green.
        private void StopOvenButton_Click(object sender, EventArgs e)
        {
            User.ReadOnly = false;
            User.BorderStyle = BorderStyle.Fixed3D;
            OvenName.ReadOnly = false;
            OvenName.BorderStyle = BorderStyle.Fixed3D;
            UserEmail.ReadOnly = false;
            UserEmail.BorderStyle = BorderStyle.Fixed3D;
            UserPhone.ReadOnly = false;
            UserPhone.BorderStyle = BorderStyle.Fixed3D;
            UserPhone.BackColor = Color.White;
            BatchCode.ReadOnly = false;
            BatchCode.BorderStyle = BorderStyle.Fixed3D;
            MaxTemp.ReadOnly = false;
            MaxTemp.BorderStyle = BorderStyle.Fixed3D;
            HoldTime.ReadOnly = false;
            HoldTime.BorderStyle = BorderStyle.Fixed3D;
            CoolMethod.Enabled = true;

            if (oven_check != 0)
            {
                DialogResult dialogResult = MessageBox.Show("WARNING! This is" +
                    " will stop the oven run process and shut down all coils!" +
                    " Are you sure you want to stop the oven run?", "   " +
                    "STOP OVEN WARNING", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    oven_check = 0;
                    graph_check++;
                    StartOvenButton.BackColor = default(Color);

                    if (oven_start_check != 0)
                        StopOvenButton.BackColor = Color.GreenYellow;

                    HeatingStopped = true;
                    HeatingStarted = false;
                    HeatingStarted_logged = false;
                }
                else
                    return;
            }
            return;
        }

        // This function is called when the degree Celsius button in the GUI
        // is clicked. It changes the button color to Green and resets the color
        // of the other temperature scale buttons, so that the user knows which 
        // temperature scale is currently being displayed.
        private void Celsius_Click(object sender, EventArgs e)
        {
            my_TempScale = TempScale.Celsius;
            Celsius.BackColor = Color.GreenYellow;
            Fahrenheit.BackColor = default(Color);
            Kelvin.BackColor = default(Color);
        }

        // This function is called when the degree Fahrenheit button in the GUI
        // is clicked. It changes the button color to Green and resets the color
        // of the other temperature scale buttons, so that the user knows which 
        // temperature scale is currently being displayed.
        private void Fahrenheit_Click(object sender, EventArgs e)
        {
            my_TempScale = TempScale.Fahrenheit;
            Celsius.BackColor = default(Color);
            Fahrenheit.BackColor = Color.GreenYellow;
            Kelvin.BackColor = default(Color);
        }
        
        // This function is called when the Kelvin scale button in the GUI is
        // clicked. It changes the button color to Green and resets the color
        // of the other temperature scale buttons, so that the user knows which 
        // temperature scale is currently being displayed.
        private void Kelvin_Click(object sender, EventArgs e)
        {
            my_TempScale = TempScale.Kelvin;
            Celsius.BackColor = default(Color);
            Fahrenheit.BackColor = default(Color);
            Kelvin.BackColor = Color.GreenYellow;
        }

        // This function is used to open a directory to which the user wants to
        // save the log file.
        private void OpenDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "Select a location in which the temperature log" +
                " files will be saved.";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveDir.BackColor = Color.White;

                filepath = fbd.SelectedPath;

                if (BatchCode.Text == "_" ||
                    String.IsNullOrWhiteSpace(BatchCode.Text))
                    BatchCode.Text = User.Text;

                date_time = DateTime.Now;

                filename = String.Join("", User.Text, "_", BatchCode.Text, "",
                    date_time.ToString(format), ".txt");

                directory = Path.Combine(filepath, filename);

                SaveDir.Text = directory;
            }    
        }

        // This function fill in values for the oven run parameters. It is
        // useful for testing purposes.
        private void TestMode_Click(object sender, EventArgs e)
        {
            resetboxes();
            User.Text = "hgolash";
            UserEmail.Text = "hgolash@caltech.edu, harrygolash@gmail.com";
            UserPhone.Text = "626-354-3753";
            OvenName.Text = "Lowenstam";
            BatchCode.Text = "SSR1";
            MaxTemp.Text = "300";
            HoldTime.Text = "30";
            UserPhone.BackColor = Color.White;
            CoolMethod.SelectedIndex = 1;
            SwitchTemp.Text = "150";
        }

        // This function is called when the cooling method is changed.
        private void CoolMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CoolMethod.SelectedIndex == 1)
            {
                SwitchTemp.Enabled = true;
                SwitchTemp.ReadOnly = false;
                SwitchTemp.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                SwitchTemp.ReadOnly = true;
                SwitchTemp.BackColor = Color.White;
                SwitchTemp.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        // This function copies the values of the oven run parameters that are 
        // set when starting the oven or thermocouple reader for the first time.
        private void copy_parameters()
        {
            User_copy = User.Text;
            BatchCode_copy = BatchCode.Text;
            UserEmail_copy = UserEmail.Text;
            UserPhone_copy = UserPhone.Text;
            MaxTemp_copy = MaxTemp.Text;
            HoldTime_copy = HoldTime.Text;
            CoolMethod_copy = CoolMethod.SelectedIndex.ToString();
            directory_copy = directory;
            OvenName_copy = OvenName.Text;
            SwitchTemp_copy = SwitchTemp.Text;
            OvenStartTime_copy = OvenStartTime.Text;
            TimeElapsed_copy = TimeElapsed.Text;
            HoldStartTime_copy = HoldStartTime.Text;
            HoldTimeRemaining_copy = HoldTimeRemaining.Text;
            CoolStartTime_copy = CoolStartTime.Text;
            CoolTimeElapsed_copy = CoolTimeElapsed.Text;
        }

        // This function checks to see if the user changed any of the 
        // parameters after stopping the oven or the thermocouple reader.
        private bool is_parameters_changed()
        {
            if (User_copy != User.Text ||
            UserEmail_copy != UserEmail.Text ||
            BatchCode_copy != BatchCode.Text ||
            UserPhone_copy != UserPhone.Text ||
            MaxTemp_copy != MaxTemp.Text ||
            HoldTime_copy != HoldTime.Text ||
            CoolMethod_copy != CoolMethod.SelectedIndex.ToString() ||
            directory_copy != directory ||
            SwitchTemp_copy != SwitchTemp.Text ||
            OvenName_copy != OvenName.Text)
            {
                if (BatchCode_copy != BatchCode.Text)
                    log_state = 0;
                return true;
            }
                
            return false;
        }

        // This function resets the user input boxes' colors to white
        private void resetboxes()
        {
            User.BackColor = Color.White;
            UserEmail.BackColor = Color.White;
            BatchCode.BackColor = Color.White;
            MaxTemp.BackColor = Color.White;
            HoldTime.BackColor = Color.White;
            CoolMethod.BackColor = Color.White;
            SaveDir.BackColor = Color.White;
            OvenName.BackColor = Color.White;
            SwitchTemp.BackColor = Color.White;
        }

        // This function updates the filename in case the user sets the
        // directory before setting the batch name
        private bool update_directory()
        {
            string temp_filename = "";
            string temp_directory = "";
            
            DateTime date_time_copy = DateTime.Now;

            temp_filename = String.Join("", User.Text, "_", BatchCode.Text, "",
                    date_time_copy.ToString(format), ".txt");

            temp_directory = Path.Combine(filepath, temp_filename);

            if (SaveDir.Text != temp_directory)
            {
                directory = temp_directory;
                SaveDir.Text = directory;
                return true;
            }
            return false;
        }

        // This function checks to see if the values for the oven run parameters
        // are valid and useable.
        private int checkparameters(string str)
        {
            if(!(str == "TC" || str == "oven"))
            {
                MessageBox.Show("checkparameters() has an error!", "   ERROR!");
                return 1;
            }
            
            int check = 0;

            if (String.IsNullOrWhiteSpace(User.Text) || User.Text == "User") 
            {
                User.BackColor = Color.MistyRose;
                check++;
            }
            else    
               User.BackColor = Color.White;

            if (String.IsNullOrWhiteSpace(BatchCode.Text) ||
                    BatchCode.Text == "_" || BatchCode.Text ==
                    User.Text)
            {
                BatchCode.BackColor = Color.MistyRose;
                check++;
            }
            else
                BatchCode.BackColor = Color.White;
           
            if (String.IsNullOrWhiteSpace(OvenName.Text))
            {
                OvenName.BackColor = Color.MistyRose;
                check++;
            }
            else
                OvenName.BackColor = Color.White;

            if (str == "oven")
            {
                try
                {
                    String[] delimeters = new String[]{",",";"};
                    String[] email_addresses = UserEmail.Text.Split(delimeters,
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

                    if (UserEmail.Text == "user@emailaddress.com")
                        throw new Exception();
                    UserEmail.BackColor = Color.White;
                }
                catch
                {
                    UserEmail.BackColor = Color.MistyRose;
                    check++;
                }

                try
                {
                    if (String.IsNullOrWhiteSpace(MaxTemp.Text))
                    {
                        MaxTemp.BackColor = Color.MistyRose;
                        check++;
                    }
                    else if (Convert.ToInt32(MaxTemp.Text) > 800 ||
                        Convert.ToInt32(MaxTemp.Text) < 20)
                    {
                        MaxTemp.BackColor = Color.MistyRose;
                        check++;
                    }
                    else
                    {
                        if (Convert.ToInt32(MaxTemp.Text) >= 150 && 
                            CoolMethod.SelectedIndex == 0)
                            MessageBox.Show("Recommend using Nitrogen (N2) " +
                        "cooling when heating samples to 150 °C and above.",
                                "   SUGGESTION");
                        MaxTemp.BackColor = Color.White;
                    }
                }
                catch
                {
                    MaxTemp.BackColor = Color.MistyRose;
                    check++;
                }

                try
                {
                    if (String.IsNullOrWhiteSpace(HoldTime.Text))
                    {
                        HoldTime.BackColor = Color.MistyRose;
                        check++;
                    }
                    else if (Convert.ToInt32(HoldTime.Text) > 100 ||
                        Convert.ToInt32(HoldTime.Text) < 5)
                    {
                        HoldTime.BackColor = Color.MistyRose;
                        check++;
                    }
                    else
                        HoldTime.BackColor = Color.White;
                }
                catch
                {
                    HoldTime.BackColor = Color.MistyRose;
                    check++;
                }

                if (CoolMethod.SelectedItem == null)
                {
                    CoolMethod.BackColor = Color.MistyRose;
                    check++;
                }
                else if (CoolMethod.SelectedIndex == 1)
                {
                    SwitchTemp.Enabled = true;
                    try
                    {
                        if (String.IsNullOrWhiteSpace(SwitchTemp.Text))
                        {
                            SwitchTemp.BackColor = Color.MistyRose;
                            check++;
                        }
                        else if (Convert.ToInt32(SwitchTemp.Text) > 500 ||
                        Convert.ToInt32(HoldTime.Text) < 5)
                        {
                            SwitchTemp.BackColor = Color.MistyRose;
                            check++;
                        }
                        else
                            SwitchTemp.BackColor = Color.White;
                    }
                    catch
                    {
                        SwitchTemp.BackColor = Color.MistyRose;
                        check++;
                    }
                    CoolMethod.BackColor = Color.White;
                }
                else if (CoolMethod.SelectedIndex == 0)
                {
                    SwitchTemp.Text = "";
                    SwitchTemp.BackColor = Color.White;
                    SwitchTemp.Enabled = false;
                    CoolMethod.BackColor = Color.White;
                }
                else
                    CoolMethod.BackColor = Color.White;
            }

            return check;
        }

        // This function is used to send out the oven notification emails.
        private void SendEmail(string subject, string body)
        {
            try
            {
                // SMPT host setup
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = SMTP_Creds;
                client.EnableSsl = true;

                string temp = UserEmail.Text.Replace(" ", string.Empty);
                temp = temp.Replace(";", ",");

                MailAddress from =
                    new MailAddress("lab.demagnetization.oven@gmail.com");

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


        // This function changes CHANCOUNT, by detecting the actual number of
        // working thermocouple inputs connected to the USB-TC board.
        private void TC_finder()
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

                        ((CheckBox)this.Controls["Ch" + 
                            i.ToString("#0") + "Box"]).Checked = false;

                        ((CheckBox)this.Controls["Ch" +
                            i.ToString("#0") + "Box"]).Enabled = false;
                    }

                    else
                    {
                        ((CheckBox)this.Controls["Ch" + 
                            i.ToString("#0") + "Box"]).Checked = true;
                    }
                }

                NumThermocouples.Text = " " + CHANCOUNT.ToString();

                return;
            }

            catch
            {
                MessageBox.Show("TC_finder has an error!", "   " + "ERROR!");
                return;
            }
        }

        // This function updates the  values and colors for the temperature 
        // readout and the times elapsed for the oven run. It uses a for loop to
        // set the values of the textboxes that shows the temperature values. It
        // shows an error using a message box.
        public void update_temp(float[] TempData, float[] TempData_C)
        {
            if (abort_oven_program)
            {
                StopThreads_AndCloseOvenProgram();
                return;
            }

            float Outer = 0;
            float Inner = 0;
            float Tray_Outer = 0;
            float Tray_SZ = 0;
            float Tray_Inner = 0;
            double Time;
            
            System.TimeSpan timespan = new System.TimeSpan();

            try
            {
                if (TempData[0] == 1234)
                { 
                    for (int i = 0; i < MAX_CHANCOUNT; i++)
                    {
                        this.Controls["Ch" + i.ToString("#0")].ForeColor =
                                Color.Gray;
                    }
                    return;
                }

                for (int i = 0; i < MAX_CHANCOUNT; i++)
                {
                    if (TempData[i] != -9999)
                    {
                        this.Controls["Ch" + i.ToString("#0")].Text =
                            (" " + (int)(TempData[i])).ToString();
                        this.Controls["Ch" + i.ToString("#0")].ForeColor =
                                Color.LawnGreen;
                    }
                    else 
                    {
                        this.Controls["Ch" + i.ToString("#0")].Text =
                                " ***";
                        this.Controls["Ch" + i.ToString("#0")].ForeColor =
                                Color.Gray;
                    }

                    if (TempData_C[i] == -9999)
                        TempData_C[i] = 0;

                    if (i == 0 || i == 5)
                    {
                        Tray_Outer += TempData_C[i];
                        if (TempData_C[0] == 0 || TempData_C[5] == 0)
                        {
                            Tray_Outer *= 2;
                        }
                    }
                    else if (i == 1 || i == 4)
                    {
                        Tray_SZ += TempData_C[i];
                        if (TempData_C[1] == 0 || TempData_C[4] == 0)
                        {
                            Tray_SZ *= 2;
                        }
                    }
                    else if (i == 2 || i == 3)
                    {
                        Tray_Inner += TempData_C[i];
                        if (TempData_C[2] == 0 || TempData_C[3] == 0)
                        {
                            Tray_Inner *= 2;
                        }
                    }
                    else if (i == 6)
                        Outer = TempData_C[i];
                    else if (i == 7)
                        Inner = TempData_C[i];
                }

                // Graphing section:
                if (temp_read_check == 1)
                {
                    if (graph_check == 3)
                    {
                        foreach (var series in TempGraph.Series)
                        {
                            series.Points.Clear();
                        }
                        graph_check = 1;
                    }

                    if (oven_check == 1)
                        timespan = DateTime.Now.Subtract(Start_Oven_Time);
                    else
                        timespan = DateTime.Now.Subtract(date_time);

                    if (oven_check == 1)
                        TimeElapsed.Text = timespan.ToString(@"h\:mm\:ss");
                    
                    Time = timespan.TotalMinutes;
                    
                    TempGraph.Series["Tray Outer"].Points.AddXY(Time, 
                        (Tray_Outer)/2);
                    TempGraph.Series["Tray SZ"].Points.AddXY(Time, 
                        (Tray_SZ)/2);
                    TempGraph.Series["Tray Inner"].Points.AddXY(Time, 
                        (Tray_Inner)/2);
                    TempGraph.Series["Outer"].Points.AddXY(Time, Outer);
                    TempGraph.Series["Inner"].Points.AddXY(Time, Inner);
                }

                return;
            }

            catch 
            {
                MessageBox.Show("update_temp has an error!", "   " + "ERROR!");
                return;
            }            
        }

        // This function updates the WARNING box.
        public void update_warning(string Warning, int ErrorCode)
        {
            if (abort_oven_program)
            {
                StopThreads_AndCloseOvenProgram();
                return;
            }

            try
            {

                WarningBox.Text = Warning;
                switch (ErrorCode)
                { 
                    case 0:
                        ReadTCbutton.BackColor = Color.GreenYellow;
                        StopTCbutton.BackColor = default(Color);
                        for (int i = 0; i < MAX_CHANCOUNT; i++)
                        {
                            if (this.Controls["Ch" + i.ToString("#0")].ForeColor
                                == Color.Gray)
                            {
                                this.Controls["Ch" + i.ToString("#0")].ForeColor
                                    = Color.LawnGreen;
                            }
                        }
                        break;
                    case 1:
                        ReadTCbutton.BackColor = default(Color);
                        StopTCbutton.BackColor = Color.Red;
                        for (int i = 0; i < MAX_CHANCOUNT; i++)
                        {
                            this.Controls["Ch" + i.ToString("#0")].ForeColor =
                                Color.Gray;
                        }
                        break;
                    default:
                        break;
                }
                return;
            }

            catch
            {
                MessageBox.Show("update_warning has an error!", "   " + 
                    "ERROR!");
                return;
            }
        }

        // This function updates the temperature log text file. It returns if
        // the number of channels is invalid. Otherwise, it uses a while loop to
        // output temperature readings to a .txt file in the same directory as
        // the program folder. It shows an error if there is an open channel.
        public void update_log()
        {
            MccBoard daq = new MccDaq.MccBoard(BoardNum);
            MccDaq.ErrorInfo RetVal;
            string subject = "";
            string body = "";

            string separator = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                            "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                            "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                            "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

             while (log_check == 0)
            {
                if (log_state == 0)
                {
                    log_state = 1;

                    string[] text1 = {"Temperature and events log file.", " ",
                                    "Created on " + 
                                    date_time.ToString(format),
                                    " ","Temperature read approx. every 5 "+
                                    "seconds in degrees Celsius.", " "," "};

                    System.IO.File.WriteAllLines(directory, text1);

                    string text2 = "Time\t\t\t";

                    for (int i = 0; i < MAX_CHANCOUNT; i++)
                    {
                        text2 += String.Format("Ch{0}", i) + "\t\t";
                    }

                    using (StreamWriter sw = File.AppendText(directory))
                    {
                        sw.WriteLine(text2);
                        sw.WriteLine(" ");
                    }
                }

                if (HeatingStarted && !HeatingStarted_logged)
                {
                    string temp = DateTime.Now.ToString("T") + "\t\t" +
                            "HEATING STARTED!";

                    using (StreamWriter sw = File.AppendText(directory))
                    {
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                        sw.WriteLine(temp);
                        sw.WriteLine("\t\t\tUser: " + User_copy);
                        sw.WriteLine("\t\t\tBatch code: " + BatchCode_copy);
                        sw.WriteLine("\t\t\tMax. Temperature: " + MaxTemp_copy);
                        sw.WriteLine("\t\t\tHold Time: " + HoldTime_copy);
                        sw.WriteLine("\t\t\tCooling method: " + CoolMethod_copy);
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                    }

                    subject = "Heating has started on " + OvenName_copy +
                        " oven!";

                    body = User_copy + ",\n\n" + subject +
                        "\n\nParameters are: \nBatch Code: " + BatchCode_copy +
                        "\nMax.Temp: " + MaxTemp_copy +
                        "\nHold Time: " + HoldTime_copy +
                        "\nCooling Method: " + CoolMethod_copy +
                        "\nTime: " + OvenStartTime_copy;
                    SendEmail(subject, body);
                    HeatingStarted_logged = true;
                }

                else if (HeatingStopped && !HeatingStopped_logged)
                {
                    string temp = DateTime.Now.ToString("T") + "\t\t" +
                            "HEATING STOPPED!";

                    using (StreamWriter sw = File.AppendText(directory))
                    {
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                        sw.WriteLine(temp);
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                    }

                    subject = "Heating has stopped on " + OvenName_copy +
                        " oven!";
                    body = User_copy + ",\n\n" + subject;
                    SendEmail(subject, body);
                    HeatingStopped_logged = true;
                }

                else if (MaxTempReached && !MaxTempReached_logged)
                {
                    string temp = DateTime.Now.ToString("T") + "\t\t" +
                            "MAX. TEMP REACHED!";

                    using (StreamWriter sw = File.AppendText(directory))
                    {
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                        sw.WriteLine(temp);
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                    }

                    subject = "Max. temp reached on " + OvenName_copy +
                        " oven!";
                    body = User_copy + ",\n\n" + subject;
                    SendEmail(subject, body);
                    MaxTempReached_logged = true;
                }

                else if (CoolingStarted && !CoolingStarted_logged)
                {
                    string temp = DateTime.Now.ToString("T") + "\t\t" +
                            "COOLING STARTED!";

                    using (StreamWriter sw = File.AppendText(directory))
                    {
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                        sw.WriteLine(temp);
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                    }

                    subject = "Cooling has started on " + OvenName_copy +
                        " oven!";
                    body = User_copy + ",\n\n" + subject;
                    SendEmail(subject, body);
                    CoolingStarted_logged = true;
                }

                else if (IsCriticalError && !IsCriticalError_logged)
                {
                    string temp = DateTime.Now.ToString("T") + "\t\t" +
                            "CRITICAL ERROR!";

                    using (StreamWriter sw = File.AppendText(directory))
                    {
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                        sw.WriteLine(temp);
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                    }

                    subject = "Critical error on " + OvenName_copy + " oven!";
                    body = User_copy + ",\n\n" + subject + "\n" +
                        CriticalErrorMessage;
                    SendEmail(subject, body);
                    IsCriticalError_logged = true;
                }

                else if (OvenRunDone && !OvenRunDone_logged)
                {
                    string temp = DateTime.Now.ToString("T") + "\t\t" +
                            "OVEN RUN COMPLETE!";

                    using (StreamWriter sw = File.AppendText(directory))
                    {
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                        sw.WriteLine(temp);
                        sw.WriteLine("\t\t\tUser: " + User_copy);
                        sw.WriteLine("\t\t\tBatch code: " + BatchCode_copy);
                        sw.WriteLine("\t\t\tMax. Temperature: " + MaxTemp_copy);
                        sw.WriteLine("\t\t\tHold Time: " + HoldTime_copy);
                        sw.WriteLine("\t\t\tCooling method: " + CoolMethod_copy);
                        sw.WriteLine(" ");
                        sw.WriteLine(separator);
                        sw.WriteLine(" ");
                    }

                    subject = "Oven run completed on " + OvenName_copy + " oven!";
                    body = User_copy + ",\n\n" + subject + "\n";
                    SendEmail(subject, body);
                    OvenRunDone_logged = true;
                }

                string text3 = DateTime.Now.ToString("T") + "\t\t";

                for (int i = 0; i < MAX_CHANCOUNT; i++)
                {
                    RetVal = daq.TIn(i, TempScale.Celsius,
                        out LogData[i], ThermocoupleOptions.Filter);
                    IsError(this, RetVal, true);

                    if (RetVal.Value != 0)
                        text3 += "***\t\t";
                    else
                        text3 += LogData[i].ToString() + "\t\t";
                }

                

                using (StreamWriter sw = File.AppendText(directory))
                {
                    sw.WriteLine(text3);
                }

                Thread.Sleep(4800);
            }
        }

        // This function reads and displays the temperature inputs by detecting
        // the buttons clicked, by detecting the value of the variable "check".
        // It runs a while loop to keep checking for button presses, and using
        // Thread.Sleep(), it checks buttons, reads and displays temperatures in
        // the selected scale at a set frequency (2 Hz). The loop ends when the
        // value of check is set to -1 by clicking the Exit button. Temperatures
        // are read using the TInScan function, and an array is passed to the 
        // update_temp() function to display them using a delegate.
        private void TC_reader()
        {
           while (read_check != -1)
           {
                int count = 0;
                float[] TempData = new float[MAX_CHANCOUNT];
                float[] TempData_C = new float[MAX_CHANCOUNT];
                    
                MccBoard daq = new MccDaq.MccBoard(BoardNum);
                MccDaq.ErrorInfo RetVal;

                Update_Temperature update_temp_del = new 
                    Update_Temperature(update_temp);

                Update_Warning update_warning_del = new 
                    Update_Warning(update_warning);

                Reset_Checkboxes resetcheckboxes_del = new
                    Reset_Checkboxes(resetcheckboxes);

                for (int i = 0; i < MAX_CHANCOUNT; i++)
                {
                    if (((CheckBox)this.Controls["Ch" +
                        i.ToString("#0") + "Box"]).Enabled == true &&
                        ((CheckBox)this.Controls["Ch" +
                        i.ToString("#0") + "Box"]).Checked == true)
                    {
                        count++;
                    }
                }

                if (count == 0)
                {
                    TempData = new float[MAX_CHANCOUNT] {-9999, -9999,
                        -9999, -9999, -9999, -9999, -9999, -9999};

                    Array.Copy(TempData, TempData_C, 8);

                    not_showing_temp = true;
                    Invoke(update_temp_del, TempData, TempData_C);
                }
                else
                    not_showing_temp = false;

                if (read_check == 1 && not_showing_temp == false)
                {
                    for (int i = 0; i < MAX_CHANCOUNT; i++)
                    {
                        if (((CheckBox)this.Controls["Ch" +
                            i.ToString("#0") + "Box"]).Checked == true)
                        {
                            RetVal = daq.TIn(i, TempScale.Celsius,
                                out TempData_C[i],
                                ThermocoupleOptions.Filter);
                            IsError(this,RetVal, true);
                        }
                        else
                            TempData_C[i] = -9999;
                    }
                    
                    switch(my_TempScale)
                    {
                        case TempScale.Fahrenheit:
                            for (int i = 0; i < MAX_CHANCOUNT; i++)
                            {
                                if (((CheckBox)this.Controls["Ch" +
                                    i.ToString("#0") + "Box"]).Checked == true)
                                {
                                    RetVal = daq.TIn(i, TempScale.Fahrenheit,
                                        out TempData[i],
                                        ThermocoupleOptions.Filter);
                                    IsError(this,RetVal, true);
                                }
                                else
                                    TempData[i] = -9999;
                            }
                            break;
                            
                        case TempScale.Kelvin:
                            for (int i = 0; i < MAX_CHANCOUNT; i++)
                            {
                                if (((CheckBox)this.Controls["Ch" +
                                    i.ToString("#0") + "Box"]).Checked == true)
                                {
                                    RetVal = daq.TIn(i, TempScale.Kelvin,
                                        out TempData[i],
                                        ThermocoupleOptions.Filter);
                                    IsError(this,RetVal, true);
                                }
                                else
                                    TempData[i] = -9999;
                            }
                            break;
                                                        
                        default:
                            for (int i = 0; i < MAX_CHANCOUNT; i++)
                            {
                                if (((CheckBox)this.Controls["Ch" +
                                    i.ToString("#0") + "Box"]).Checked == true)
                                {
                                    RetVal = daq.TIn(i, TempScale.Celsius,
                                        out TempData[i],
                                        ThermocoupleOptions.Filter);
                                    IsError(this,RetVal, true);
                                }
                                else
                                    TempData[i] = -9999;
                            }
                            break;
                    }

                    Invoke(update_temp_del, TempData, TempData_C);

                    Thread.Sleep(500);
                }

                if ((read_check == 0 && oven_check == 1) || 
                    (not_showing_temp == true && oven_check == 1))
                {
                    string temp = String.Format("WARNING! Temperature" +
                        " readings are no longer live during an oven run!\n" +
                        "Automatically fixing this in {0} seconds...", 
                        (int)(warning_time/2));
                    
                   
                    Invoke(update_warning_del, temp, 1);
                    
                    Thread.Sleep(500);
                    
                    warning_time--;

                    if (warning_time <= 0)
                    {
                        read_check = 1;
                        Invoke(resetcheckboxes_del);
                        not_showing_temp = false;
                        Invoke(update_warning_del, " ", 0);
                        warning_time = 20;
                    }
                }

                // Make sure that the text is grayed out when the Stop TC button
                // is in its clicked state.
                if (read_check == 0 && oven_check != 1)
                {
                    TempData[0] = 1234;
                    TempData_C[0] = 1234;
                    Invoke(update_temp_del, TempData, TempData_C);
                    Thread.Sleep(500);
                }
            }
         }

        // This is a helper function used by update_temp() to reset the 
        // temperature display checkboxes in the event that they are all
        // unchecked during an oven run.
        private void resetcheckboxes()
        {
            for (int i = 0; i < MAX_CHANCOUNT; i++)
            {
                if (((CheckBox)this.Controls["Ch" +
                    i.ToString("#0") + "Box"]).Enabled == true &&
                    ((CheckBox)this.Controls["Ch" +
                    i.ToString("#0") + "Box"]).Checked == false)
                {
                    ((CheckBox)this.Controls["Ch" +
                    i.ToString("#0") + "Box"]).Checked = true;
                }
            }
        }

        // This function turns on the heater coils according to the string
        // parameter passed to it. Usable string parameters are: "all", "0",
        // "1", "2", and "none"; "all" and "none" turn on/off all the coils,
        // while "0", "1", "2" turns on the coil corresponding to that channel
        // output from the USB-TC device.
        private void oven_channel_out(OvenChanStatusEnum state)
        {
            MccBoard daq = new MccDaq.MccBoard(BoardNum);
            MccDaq.ErrorInfo RetVal;

            for (int i = 0; i < 3; i++)
            {
                RetVal = daq.DConfigBit(DigitalPortType.AuxPort, i, 
                    DigitalPortDirection.DigitalOut);
                IsError(this,RetVal,true);
            }

            switch (state)
            {
                case OvenChanStatusEnum.oven_on:
                    for (int i = 0; i < 3; i++)
                    {
                        RetVal = daq.DBitOut(DigitalPortType.AuxPort, i, 
                            DigitalLogicState.High);
                        IsError(this,RetVal, true);
                    }
                    break;

                case OvenChanStatusEnum.oven_off:
                    for (int i = 0; i < 3; i++)
                    {
                        RetVal = daq.DBitOut(DigitalPortType.AuxPort, i,
                            DigitalLogicState.Low);
                        IsError(this,RetVal, true);
                    }
                    break;

                case OvenChanStatusEnum.all_off:
                    for (int i = 0; i < 3; i++)
                    {
                        RetVal = daq.DBitOut(DigitalPortType.AuxPort, i,
                            DigitalLogicState.Low);
                        IsError(this,RetVal, true);
                    }
                    break;

                case OvenChanStatusEnum.inner:
                    RetVal = daq.DBitOut(DigitalPortType.AuxPort, 0, 
                        DigitalLogicState.High);
                    IsError(this,RetVal, true);
                    break;

                case OvenChanStatusEnum.SZ:
                    RetVal = daq.DBitOut(DigitalPortType.AuxPort, 1, 
                        DigitalLogicState.High);
                    IsError(this,RetVal, true);
                    break;

                case OvenChanStatusEnum.outer:
                    RetVal = daq.DBitOut(DigitalPortType.AuxPort, 2, 
                        DigitalLogicState.High);
                    IsError(this,RetVal, true);
                    break;

                default:
                    RetVal = new ErrorInfo(1);
                    MessageBox.Show("oven_channel_out() called with invalid parameter!",
                        "   " + "ERROR!");
                    break;
            }

            RetVal = daq.DConfigBit(DigitalPortType.AuxPort, 0, 
                DigitalPortDirection.DigitalOut);

            if (IsError(this, RetVal, true) == 0)
            {
                oven_status = state;
                SetHeatingElementStatus(state);
            }            
        }

        private void air_channel_out(AirChanStatusEnum state)
        {
            MccBoard daq = new MccDaq.MccBoard(BoardNum);
            MccDaq.ErrorInfo RetVal;

            for (int i = 3; i < 6; i++)
            {
                RetVal = daq.DConfigBit(DigitalPortType.AuxPort, i,
                    DigitalPortDirection.DigitalOut);
                IsError(this, RetVal, true);
            }

            switch (state)
            {
                case AirChanStatusEnum.air_off:
                    for (int i = 3; i < 6; i++)
                    {
                        RetVal = daq.DBitOut(DigitalPortType.AuxPort, i,
                            DigitalLogicState.Low);
                        IsError(this, RetVal, true);
                    }
                    break;

                case AirChanStatusEnum.low_n2:
                    RetVal = daq.DBitOut(DigitalPortType.AuxPort, 5,
                        DigitalLogicState.High);
                    IsError(this, RetVal, true);
                    break;

                case AirChanStatusEnum.high_n2:
                    RetVal = daq.DBitOut(DigitalPortType.AuxPort, 3,
                        DigitalLogicState.High);
                    IsError(this, RetVal, true);
                    break;

                case AirChanStatusEnum.air:
                    RetVal = daq.DBitOut(DigitalPortType.AuxPort, 4,
                        DigitalLogicState.High);
                    IsError(this, RetVal, true);
                    break;

                default:
                    RetVal = new ErrorInfo(1);
                    MessageBox.Show("air_channel_out() called with invalid parameter!",
                        "   " + "ERROR!");
                    break;
            }

            RetVal = daq.DConfigBit(DigitalPortType.AuxPort, 0,
                DigitalPortDirection.DigitalOut);

            if (IsError(this, RetVal, true) == 0)
            {
                air_status = state;
            }
        }


        // Fun function to test the channel_out() function. Makes the coils or
        // lights flash on/off.
        private void coils_dance()
        {
            oven_channel_out(OvenChanStatusEnum.oven_on);
            if (oven_check != 1)
                return;
            Thread.Sleep(300);

            oven_channel_out(OvenChanStatusEnum.oven_off);
            if (oven_check != 1)
                return;
            Thread.Sleep(300);

            oven_channel_out(OvenChanStatusEnum.inner);
            oven_channel_out(OvenChanStatusEnum.outer);
            if (oven_check != 1)
                return;
            Thread.Sleep(300);

            oven_channel_out(OvenChanStatusEnum.oven_off);
            oven_channel_out(OvenChanStatusEnum.SZ);
            if (oven_check != 1)
                return;
            Thread.Sleep(300);

            oven_channel_out(OvenChanStatusEnum.oven_off);
            if (oven_check != 1)
                return;
            Thread.Sleep(300);

            oven_channel_out(OvenChanStatusEnum.inner);
            if (oven_check != 1)
                return;
            Thread.Sleep(200);
            
            oven_channel_out(OvenChanStatusEnum.oven_off);
            oven_channel_out(OvenChanStatusEnum.SZ);
            if (oven_check != 1)
                return;
            Thread.Sleep(200);
            
            oven_channel_out(OvenChanStatusEnum.oven_off);
            oven_channel_out(OvenChanStatusEnum.outer);
            if (oven_check != 1)
                return;
            Thread.Sleep(200);
            
            oven_channel_out(OvenChanStatusEnum.oven_off);
            oven_channel_out(OvenChanStatusEnum.SZ);
            if (oven_check != 1)
                return;
            Thread.Sleep(200);
            
            oven_channel_out(OvenChanStatusEnum.oven_off);
            oven_channel_out(OvenChanStatusEnum.inner);
            if (oven_check != 1)
                return;
            Thread.Sleep(200);
        }

        // This function implements PID control in the oven, by using three
        // USB-TC output channels. It calls the functions channel_out() to 
        // control the heater coils according to the known setup.
        private void PID_control()
        {
            MccBoard daq = new MccDaq.MccBoard(BoardNum);
            MccDaq.ErrorInfo RetVal;int count = 0;

            float[] PID_LogData = new float[MAX_CHANCOUNT] {0, 0, 0, 0, 0, 0, 0 ,0};;

            int PID_max_temp = Convert.ToInt32(MaxTemp.Text);

            float current_error = 0; // difference between current and target
                                     // temperatures

            float prev_error = PID_max_temp;    // the last recorded error;
                                                // initialized to the max temp.
            
            float time_slice = 1;   // any non-zero value, so that we don't have
                                    // a divide-by-zero error
             
            float derivative_component = 0; // error change time rate
            float integral_component = 0; // total accumulated error

            bool fill_n2_check = false;

            oven_channel_out(OvenChanStatusEnum.all_off);   // start with all oven channels off
            air_channel_out(AirChanStatusEnum.air_off);     //Start with air switched off

            while (oven_check == 1)
            {
                if (CoolMethod_copy == "1")
                {
                    // Flow high pressure nitrogen for like 10 seconds to fill
                    // the oven cavity with nitrogen
                    if (fill_n2_check == false)
                    {
                        air_channel_out(AirChanStatusEnum.high_n2);
                        Thread.Sleep(1000);
                        count++;
                        if (count >= 10)
                        {
                            fill_n2_check = true;
                            count = 0;
                        }
                        else
                            continue;
                    }

                    // Low pressure N2 trickle begins
                    air_channel_out(AirChanStatusEnum.low_n2);
                }


                // CONTROL HERE !!! ///////////// HEATING BEGINS !!! //////////

                Heat_Time = DateTime.Now;

                if (PID_LogData.Max() <= (0.80 * Convert.ToInt32(MaxTemp.Text)))
                {
                    oven_channel_out(OvenChanStatusEnum.oven_on);

                    if (HeatingStarted == false)        // Reach 80% temp. with
                    {                                   // just direct heating
                        HeatingStarted = true;
                        HeatingStopped = false;
                        HeatingStopped_logged = false;
                    }
                }
                else if (MaxTempReached == false)
                {
                    // The deivative component is degrees / second error change
                    // rate. Let's attempt to keep it at error * 0.01 for now

                    if (derivative_component < current_error * 0.01)
                        oven_channel_out(OvenChanStatusEnum.oven_on);
                    else
                        oven_channel_out(OvenChanStatusEnum.oven_off);

                    if (current_error <= 0)
                    {
                        MaxTempReached = true;
                    }

                }
                else
                    oven_channel_out(OvenChanStatusEnum.oven_off);

                ///////////////////////////////////////////////////////////////

                
                for (int i = 0; i < MAX_CHANCOUNT; i++)
                {
                    RetVal = daq.TIn(i, TempScale.Celsius,
                        out PID_LogData[i], ThermocoupleOptions.Filter);
                                        
                    if (RetVal.Value != 0)
                        PID_LogData[i] = 0;
                }

                current_error = PID_max_temp - PID_LogData.Max();
                time_slice = (float)DateTime.Now.Subtract(Heat_Time).TotalSeconds;

                integral_component += current_error;

                derivative_component = (prev_error - current_error) /
                    (time_slice);

                prev_error = current_error;

                count++;                
            }

            oven_channel_out(OvenChanStatusEnum.oven_off);   
        }

        //Event handler for the 'Exit' button
        private void ButtonExit_Click(object sender, EventArgs e)
        {
            StopThreads_AndCloseOvenProgram();
        }

        // This function closes the window and exits the program. It checks
        // if the threads are in the stopped state, and if not it stops them by
        // the value of check to -1. It then changes the color of the Stop TC
        // button to red to notify the user, and again checks to see if the
        // threads are stopped and aborted. If not, it counts to 10 and aborts
        // the thread using Thread.Abort() before closing.
        public void StopThreads_AndCloseOvenProgram()
        {
            if (close_program_inprogress) return;

            close_program_inprogress = true;
            
            if (oven_start_check != 0 || temp_read_check != 0)
            {
                DialogResult dialogResult = MessageBox.Show("WARNING! This is" +
                        " will exit the program and shut down all ongoing processes"
                        + " Are you sure you want to exit?", "   " +
                        "Exit Application?", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No)
                {
                    abort_oven_program = false;
                    close_program_inprogress = false;
                    return;
                }
            }
            
            if (temp_read_check == 1 && read_check != 0)
            {
                this.StopTCbutton.Text = "Stopping";
                this.StopTCbutton.BackColor = Color.Firebrick;
            }

            if (oven_start_check == 1 && oven_check != 0)
            {
                this.StopOvenButton.Text = "Stopping";
                this.StopOvenButton.BackColor = Color.Firebrick;
            }

            this.Refresh();

            Thread.Sleep(1000);

            if ((temp_read_thread != null && temp_read_thread.ThreadState !=
                ThreadState.Stopped) || (oven_control_thread != null &&
                oven_control_thread.ThreadState != ThreadState.Stopped) ||
                (log_thread != null && log_thread.ThreadState
                != ThreadState.Stopped))
            {
                read_check = -1;
                oven_check = -1;
                log_check = -1;
            }


            int counter = 0;

            while ((temp_read_thread != null && temp_read_thread.ThreadState !=
                ThreadState.Stopped && temp_read_thread.ThreadState !=
                ThreadState.Aborted) || (oven_control_thread != null &&
                   oven_control_thread.ThreadState != ThreadState.Stopped &&
                   oven_control_thread.ThreadState != ThreadState.Aborted) ||
                   (log_thread != null && log_thread.ThreadState
                   != ThreadState.Stopped && log_thread.ThreadState !=
                   ThreadState.Aborted))
            {
                counter++;

                if (counter > 10)
                {
                    if (temp_read_thread != null)
                        temp_read_thread.Abort();
                    if (oven_control_thread != null)
                        oven_control_thread.Abort();
                    if (log_thread != null)
                        log_thread.Abort();
                }

                Thread.Sleep(100);
            }

            if (air_status != AirChanStatusEnum.air_off)
            {
                air_channel_out(AirChanStatusEnum.air_off);
            }

            this.Close();
        }

        private void TestEmailButton_Click(object sender, EventArgs e)
        {
            SendEmail("This is a test email",
                "This should be in the email body");
        }

        private void ForceAir_Click(object sender, EventArgs e)
        {
            if (air_status != AirChanStatusEnum.air)
            {
                air_channel_out(AirChanStatusEnum.air);
                ForceAir.Text = "Stop Air";
            }
            else
            {
                air_channel_out(AirChanStatusEnum.air_off);
                ForceAir.Text = "Force Air";
            }
        }

        private void ForceN2_Click(object sender, EventArgs e)
        {
            if (air_status != AirChanStatusEnum.high_n2)
            {
                air_channel_out(AirChanStatusEnum.high_n2);
                ForceN2.Text = "Stop N2";
            }
            else
            {
                air_channel_out(AirChanStatusEnum.air_off);
                ForceN2.Text = "Force N2";
            }

        }
    }
}
