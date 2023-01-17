namespace RTCV.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.Launcher.Components;

#pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    internal partial class GamesPanel : Form
    {

        public static GamesPanel gpForm
        {
            get
            {
                if (_gpForm == null)
                    _gpForm = new GamesPanel();

                return _gpForm;
            }
        }
        public static GamesPanel _gpForm = null;

        public GamesPanel()
        {
            InitializeComponent();
        }

        private void WebPanel_Load(object sender, EventArgs e)
        {

        }


    }
}
