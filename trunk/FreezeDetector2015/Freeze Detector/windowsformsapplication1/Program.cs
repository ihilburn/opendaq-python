using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        public static FreezeDetectorForm Freeze_form;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Changed
            Freeze_form = new FreezeDetectorForm();
            Freeze_form.WindowState = FormWindowState.Maximized;
            Freeze_form.FormClosed += new FormClosedEventHandler(Freeze_form_OnClosed);
            // End Changed 
            
            Application.Run(Freeze_form);
        }

        public static void Freeze_form_OnClosed(Object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
