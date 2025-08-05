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
        public bool result;

        public RTCOpenForm()
        {
            InitializeComponent();

            result = false;
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
