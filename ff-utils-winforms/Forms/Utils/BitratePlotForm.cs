using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.Forms.Utils
{
    public partial class BitratePlotForm : Form
    {
        private Dictionary<long, long> secondsDict = new Dictionary<long, long>();

        public BitratePlotForm(Dictionary<long, long> seconds)
        {
            secondsDict = seconds;
            InitializeComponent();
        }

        private void BitratePlotForm_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Series s = chart.Series[0];

            foreach(var pair in secondsDict)
            {
                int kbps = BitratePlottingUtils.BitsToKbytes(pair.Value);
                s.Points.AddXY(pair.Key, kbps);
            }
        }

        private void chart_DoubleClick(object sender, EventArgs e)
        {
            chart.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chart.ChartAreas[0].AxisY.ScaleView.ZoomReset();
        }

        private void chart_SelectionRangeChanging(object sender, System.Windows.Forms.DataVisualization.Charting.CursorEventArgs e)
        {
            var pointStart = chart.Series[0].Points.OrderBy(x => Math.Abs(x.XValue - e.NewSelectionStart)).FirstOrDefault();
            var pointEnd = chart.Series[0].Points.OrderBy(x => Math.Abs(x.XValue - e.NewSelectionEnd)).FirstOrDefault();

            if (pointStart == null || pointEnd == null)
                return;

            TimeSpan spanStart = new TimeSpan(0, 0, (int)pointStart.XValue);
            TimeSpan spanEnd = new TimeSpan(0, 0, (int)pointEnd.XValue);

            bool backwards = pointStart.XValue > pointEnd.XValue;
            var selectedPoints = backwards ? chart.Series[0].Points.Where(x => x.XValue <= e.NewSelectionStart && x.XValue >= e.NewSelectionEnd).ToList() : chart.Series[0].Points.Where(x => x.XValue >= e.NewSelectionStart && x.XValue <= e.NewSelectionEnd).ToList();
            List<int> selectedBitrates = selectedPoints.Select(x => (int)x.YValues[0]).ToList();

            if (selectedBitrates.Count == 0)
            {
                infoLabel.Text = "";
                return;
            }

            if(selectedBitrates.Count == 1)
            {
                infoLabel.Text = $"Selected {spanStart.ToString(@"hh\:mm\:ss")} - Bitrate: {KbitsFromPoint(pointStart)}k";
                return;
            }

            infoLabel.Text = $"Selected {spanStart.ToString(@"hh\:mm\:ss")} to {spanEnd.ToString(@"hh\:mm\:ss")} ({selectedBitrates.Count} Samples) - " +
                $"Start: {KbitsFromPoint(pointStart)}k - End: {KbitsFromPoint(pointEnd)}k - " +
                $"Min: {selectedBitrates.Min()}k - Max: {selectedBitrates.Max()}k - Average: {selectedBitrates.Average()}k";
        }

        private int KbitsFromPoint(System.Windows.Forms.DataVisualization.Charting.DataPoint point)
        {
            return ((float)point.YValues[0]).RoundToInt();
        }
    }
}
