using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace USB_TC_Control
{
    static class Program
    {
        public static USB_TC_Control temp_control_form;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            temp_control_form = new USB_TC_Control();

            temp_control_form.WindowState = FormWindowState.Maximized;
            temp_control_form.FormClosed += new FormClosedEventHandler(temp_control_form_OnClosed);

            Application.Run(temp_control_form);            
        }

        public static void temp_control_form_OnClosed(Object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
