using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.Launcher
{
    public partial class RTCOpenForm : Form
    {
        public string newEmu;

        public RTCOpenForm(string newString)
        {
            InitializeComponent();

            newEmu = newString;

            // This kind of sucks but filestub doesn't actually connect until we load something, so we can't swap to it.
            // Still need to find out a way to check if it is already on filestub to avoid the opposite problem
            if (newEmu.ToUpper().Contains("FILESTUB"))
            {
                label1.Text = "An instance of RTC is already running. Unfortunately, this implementation you selected cannot be swapped to.\r\n\r\nPlease save any work, shut down RTC, and then select this implementation again.";
                button1.Hide();
                button2.Text = "OK";
            }
            else
            {
                label1.Text = "An instance of RTC is already running. By continuing, the current emulator will be closed before opening the new emulator.\r\n\r\nAre you sure you want to continue?";
                button1.Show();
                button2.Text = "No";
            }

        }

        private void RTCOpenForm_Load(object sender, EventArgs e)
        {

        }

        private void buildContextMenu1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}
