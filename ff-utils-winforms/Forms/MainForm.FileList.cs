using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageMagick;
using Nmkoder.UI.Tasks;
using Nmkoder.Main;
using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Properties;
using System;
using Nmkoder.UI;

namespace Nmkoder.Forms
{
    partial class MainForm
    {
        public ListView fileListBox { get { return fileList; } }
        public ComboBox fileListModeBox { get { return fileListMode; } }
        public Label FileCountLabel { get { return fileCountLabel; } }

    }
}
