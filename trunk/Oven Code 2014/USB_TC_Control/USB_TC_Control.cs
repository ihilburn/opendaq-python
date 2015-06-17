// These were included by default after creating the windows form display.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// Included MccDaq to use the Universal Library functions.
using MccDaq;

// Included this since this program uses threads.
using System.Threading;

namespace USB_TC_Control
{
    public partial class USB_TC_Control : Form
    {
        // The number of channels to be read.
        public const int CHANCOUNT = 4;

        // Start at channel 0 of the USB-TC board.
        public const int FIRSTCHANNEL = 0;
        
        // This is the last channel to be read.
        public static int LASTCHANNEL = 3;
        
        // Give the device a name. Valid names are USB-TC and TC.
        public const string DEVICE = "TC";
        
        // Set default temperature scale to degree Celsius.
        public static string T_Scale = "C";

        // This stores the number given to the USB-TC board.
        public static int BoardNum;
                
        // This is a global variable used to detect button presses. Its value
        // is read by multiple threads.
        public static int check = 0;

        // This thread detects if buttons are clicked or not.
        private Thread TC_read_Thread;

        // A boolean value to see if the user has made any changes.
        private bool user_change;

        // A delegate used to the update the GUI.
        public delegate void Update_Display(float[] TempData);


        public USB_TC_Control()
        {
            InitializeComponent();
        }

        // This function starts when the program is started. It calls the  
        // function that detects the USB-TC device and finds the board by its 
        // number. The threads are started in this function.
        private void USB_TC_Control_Load(object sender, EventArgs e)
        {
            
            // Locate the USB-TC and give it a number.
            BoardNum = GetBoardNum(DEVICE, BNum);

            // Set the starting value of the boolean variable to true.
            user_change = true;

            if (BoardNum == -1)
                MessageBox.Show(String.Format("No USB-{0} detected!", DEVICE));
                // Throw an error message if no USB-TC device is found. It is
                // recommended to first locate and caliberate the board in
                // InstaCal before use.
            else
            {
                // Set the default value for the button press detection variable.
                check = 0;

                // Create and start the threads.
                TC_read_Thread = new Thread(new ThreadStart(TC_reader));

                // Run the functions on the new threads.
                TC_read_Thread.Start();
            }
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
                    BNum.Text = BoardNum.ToString();
                    
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
            if (e.Value != 0)
            {
                MessageBox.Show(e.Message);
                return 1;
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
            check = 1;
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
            check = 2;
            ReadTCbutton.BackColor = default(Color);
            StopTCbutton.BackColor = Color.GreenYellow;
        }

        // This function is called when the degree Celsius button in the GUI
        // is clicked. It changes the button color to Green and resets the color
        // of the other temperature scale buttons, so that the user knows which 
        // temperature scale is currently being displayed.
        private void Celsius_Click(object sender, EventArgs e)
        {
            T_Scale = "C";
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
            T_Scale = "F";
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
            T_Scale = "K";
            Celsius.BackColor = default(Color);
            Fahrenheit.BackColor = default(Color);
            Kelvin.BackColor = Color.GreenYellow;
        }

        // This function is called when the textbox holding the value of the 
        // number of channels is changed by the user. The default value is set
        // to 4 channels. If nothing is changed, the function returns. If an
        // invalid number of channels is chosen, the number of channels to to
        // be read is reset to 4, and an error message is displayed.
        private void NumChannels_TextChanged(object sender, EventArgs e)
        {
            if (!user_change) return;

            try
            {
                if ((Convert.ToInt32(NumChannels.Text) < 0) || (Convert.ToInt32
                    (NumChannels.Text) > 7))
                    throw new Exception(); 

                LASTCHANNEL = Convert.ToInt32(NumChannels.Text) - 1;
            }
           
            catch
            {
                MessageBox.Show("Invalid Number of Temperature Input Channels.\n\nValue must be between 1 and 8.");
                LASTCHANNEL = 3;
                user_change = false;
                this.NumChannels.Text = Convert.ToString(LASTCHANNEL + 1);
                user_change = true; 
            }
           
            Ch0.Clear();
            Ch1.Clear();
            Ch2.Clear();
            Ch3.Clear();
            Ch4.Clear();
            Ch5.Clear();
            Ch6.Clear();
            Ch7.Clear();

            // Reset the default value of the boolean variable user_change.
            user_change = true;
        }

        // This function updates the GUI. It returns if the number of channels
        // is invalid. Otherwise, it uses a for loop to set the values of the
        // textboxes that shows the temperature values. It shows an error if
        // there is an open channel.
        public void update_gui(float[] TempData)
        {

            try
            {
                if ((LASTCHANNEL < 0) || (LASTCHANNEL > 7)) return;

                for (int i = 0; i <= LASTCHANNEL; i++)
                {
                    this.Controls["Ch" + i.ToString("#0")].Text = TempData[i].ToString();
                }
            }

            catch 
            {
                MessageBox.Show("Make sure that there are no open input channels!");
                this.NumChannels.Text = Convert.ToString(LASTCHANNEL + 1);
                return;
            }            
        }

        // This function reads and displays the temperature inputs by detecting
        // the buttons clicked, by detecting the value of the variable "check".
        // It runs a while loop to keep checking for button presses, and using
        // Thread.Sleep(), it checks buttons, reads and displays temperatures in
        // the selected scale at a set frequency (2 Hz). The loop ends when the
        // value of check is set to -1 by clicking the Exit button. Temperatures
        // are read using the TInScan function, and an array is passed to the 
        // update_gui() function to display them using a delegate.
        private void TC_reader()
        {
            while (true)
           {
                if (check == 1 || check == 3)
                {
                    MccBoard daq = new MccDaq.MccBoard(BoardNum);

                    float[] TempData = new float[CHANCOUNT];

                    MccDaq.ErrorInfo RetVal;

                    switch(T_Scale)
                    {
                        case "F":
                            RetVal = daq.TInScan(FIRSTCHANNEL, LASTCHANNEL, 
                                TempScale.Fahrenheit, out TempData[0], ThermocoupleOptions.Filter);
                            IsError(RetVal);
                            break;
                        case "K":
                            RetVal = daq.TInScan(FIRSTCHANNEL, LASTCHANNEL, 
                                TempScale.Kelvin, out TempData[0], ThermocoupleOptions.Filter);
                            IsError(RetVal);
                            break;
                        default:
                            RetVal = daq.TInScan(FIRSTCHANNEL, LASTCHANNEL, 
                                TempScale.Celsius, out TempData[0], ThermocoupleOptions.Filter);
                            IsError(RetVal);
                            break;
                    }

                    Update_Display update_del = new Update_Display(update_gui);

                    Invoke(update_del, TempData);

                    Thread.Sleep(500);
                }

                if (check == -1)
                {
                    break;
                }                   
             }
         }

        // This function closes the window and exits the program. It first checks
        // if the threads are in the stopped state, and if not it stops them by
        // the value of check to -1. It then changes the color of the Stop TC
        // button to red to notify the user, and again checks to see if the
        // threads are stopped and aborted. If not, it counts to 10 and aborts
        // the thread using Thread.Abort() before closing.
        private void ButtonExit_Click(object sender, EventArgs e)
        {
            if (TC_read_Thread.ThreadState != ThreadState.Stopped)
            {
                check = -1;
            }

            this.StopTCbutton.Text = "Stopping";
            this.StopTCbutton.BackColor = Color.Firebrick;
            this.Refresh();
            
            int counter = 0;

            while (TC_read_Thread.ThreadState != ThreadState.Stopped &&
                   TC_read_Thread.ThreadState != ThreadState.Aborted)
            {
                counter++;

                if (counter > 10)
                {
                    TC_read_Thread.Abort();
                }

                Thread.Sleep(100);
            }            

            this.Close();
        }
    }
}
