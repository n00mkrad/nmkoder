using Nmkoder.Extensions;
using Nmkoder.GuiHelpers;
using Nmkoder.IO;
using Nmkoder.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using ImageMagick;
using Nmkoder.UI.Tasks;
using Nmkoder.Main;
using Nmkoder.Data;
using Nmkoder.Data.Ui;

namespace Nmkoder.Forms
{
    partial class MainForm
    {
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        public void SaveConfig ()
        {
            //Logger.Log($"SaveConfig");
            ConfigParser.SaveGuiElement(mp4Faststart);
        }

        public void LoadConfig()
        {
            //Logger.Log($"SaveConfig");
            ConfigParser.LoadGuiElement(mp4Faststart);
        }
    }
}
