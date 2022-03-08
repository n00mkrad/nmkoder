using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.Forms
{
    public partial class TrimForm : Form
    {
        public TrimSettings NewTrimSettings { get; set; }

        private long originalDuration;
        private TrimSettings loadedSettings;
        private bool ready = false;
        private bool ignoreNextEvent = false;

        public TrimForm(long originalDurationMs, TrimSettings savedTrim = null)
        {
            originalDuration = originalDurationMs;
            loadedSettings = savedTrim;

            InitializeComponent();
        }

        public class TrimSettings
        {
            public enum Mode { TimeKeyframe, TimeExact, FrameNumbers }
            public Mode TrimMode { get; set; }
            public long StartTime { get; set; } // Trim start time in ms (or as frame number)
            public long Duration { get; set; } // Trim duration in ms (or as frame number)
            public long EndTime { get; set; } // Trim end time in ms (or as frame number)
            public bool IsUnset { get { return (EndTime == StartTime) || (Duration == 0); } }
            public string StartArg { get { return TrimMode == Mode.FrameNumbers ? $"select=\"gte(n\\, {StartTime})\"" : $"-ss {GetTimeString(TimeSpan.FromMilliseconds(StartTime))}"; } }
            public string DurationArg { get { return TrimMode == Mode.FrameNumbers ? $"-vframes {Duration}" : $"-t {GetTimeString(TimeSpan.FromMilliseconds(Duration))}"; } }

            public override string ToString()
            {
                string mode = TrimMode.ToString().Replace("TimeKeyframe", "Time").Replace("TimeExact", "Time").Replace("FrameNumbers", "Frames");

                if (TrimMode != Mode.FrameNumbers)
                    return $"{mode} - From {GetTimeString(TimeSpan.FromMilliseconds(StartTime))} to {GetTimeString(TimeSpan.FromMilliseconds(EndTime))} ({GetTimeString(TimeSpan.FromMilliseconds(Duration))})";
                else
                    return $"{mode} - From #{StartTime} to #{EndTime} ({Duration} Frames)";
            }
        }

        private void TrimForm_Load(object sender, EventArgs e)
        {
            if (loadedSettings != null)
            {
                trimMode.SelectedIndex = (int)loadedSettings.TrimMode;

                startBox.Text = IsFrameMode() ? loadedSettings.StartTime.ToString() : GetTimeString(TimeSpan.FromMilliseconds(loadedSettings.StartTime));
                endBox.Text = IsFrameMode() ? loadedSettings.EndTime.ToString() : GetTimeString(TimeSpan.FromMilliseconds(loadedSettings.EndTime));
            }
            else
            {
                trimMode.SelectedIndex = 0;

                startBox.Text = GetTimeString(TimeSpan.FromMilliseconds(0));

                if (originalDuration > 0)
                    endBox.Text = GetTimeString(TimeSpan.FromMilliseconds(originalDuration));
                else
                    endBox.Text = GetTimeString(TimeSpan.FromMilliseconds(0));
            }

            ready = true;
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset ()
        {
            ignoreNextEvent = true;

            startBox.Text = "";
            durationBox.Text = "";
            endBox.Text = "";

            TrimForm_Load(null, null);
            ignoreNextEvent = false;
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            TimeSpan start = new TimeSpan();
            TimeSpan end = new TimeSpan();

            bool parseSuccess = IsFrameMode() ? Regex.IsMatch(startBox.Text, @"^\d+$") && Regex.IsMatch(endBox.Text, @"^\d+$") : TryParseTime(startBox.Text, out start) && TryParseTime(endBox.Text, out end);

            if (parseSuccess)
            {
                TrimSettings trimSettings = new TrimSettings()
                {
                    TrimMode = (TrimSettings.Mode)trimMode.SelectedIndex,
                    StartTime = IsFrameMode() ? startBox.Text.GetLong() : (long)start.TotalMilliseconds,
                    Duration = IsFrameMode() ? durationBox.Text.GetLong() : (long)GetDuration(start, end).TotalMilliseconds,
                    EndTime = IsFrameMode() ? endBox.Text.GetLong() : (long)end.TotalMilliseconds
                };

                NewTrimSettings = trimSettings.IsUnset ? null : trimSettings;

                DialogResult = DialogResult.OK;
                Close();
                Program.mainForm.BringToFront();
            }
            else
            {
                UiUtils.ShowMessageBox($"Invalid input.\n\n{(IsFrameMode() ? "Please enter numeric values only." : "Please use the HH:MM:SS (or HH:MM:SS:mmm format.")}", UiUtils.MessageType.Error);
            }
        }

        private bool IsFrameMode ()
        {
            return trimMode.SelectedIndex == 2;
        }

        private bool TryParseTime(string text, out TimeSpan ts)
        {
            if (text.Contains(":"))
            {
                if (!TimeSpan.TryParse(text, out ts))
                    return false;
            }
            else
            {
                ts = TimeSpan.FromSeconds(text.GetInt());
            }

            return true;
        }

        private void startBox_TextChanged(object sender, EventArgs e)
        {
            UpdateDuration();
        }

        private void endBox_TextChanged(object sender, EventArgs e)
        {
            UpdateDuration();
        }

        private void UpdateDuration()
        {
            TimeSpan start = new TimeSpan();
            TimeSpan end = new TimeSpan();
            TimeSpan duration = new TimeSpan();

            if (!IsFrameMode()) // Time mode
            {
                if (TryParseTime(startBox.Text, out start) && TryParseTime(endBox.Text, out end))
                {
                    duration = GetDuration(start, end);
                    durationBox.Text = GetTimeString(duration);
                }
                else // Frame number mode
                {
                    durationBox.Text = GetTimeString(TimeSpan.Zero);
                }
            }
            else
            {
                long startFrames = startBox.Text.GetLong();
                long endFrames = endBox.Text.GetLong();
                durationBox.Text = (endFrames - startFrames).Clamp(0, long.MaxValue).ToString();
            }

        }

        private TimeSpan GetDuration (TimeSpan start, TimeSpan end)
        {
            TimeSpan duration = end - start;

            if (duration.Ticks < 0)
                duration = TimeSpan.Zero;

            return duration;
        }

        private void SetToZero(bool frameNums)
        {
            if (frameNums)
            {
                startBox.Text = "0";
                endBox.Text = "0";
            }
            else
            {
                startBox.Text = GetTimeString(TimeSpan.FromMilliseconds(0));
                endBox.Text = GetTimeString(TimeSpan.FromMilliseconds(0));
            }
        }

        private static string GetTimeString(TimeSpan ts)
        {
            bool ms = ts.Milliseconds != 0;
            return $"{ts.Hours.ToString().PadLeft(2, '0')}:{ts.Minutes.ToString().PadLeft(2, '0')}:{ts.Seconds.ToString().PadLeft(2, '0')}{(ms ? $".{ts.Milliseconds.ToString().PadLeft(3, '0')}" : "")}";
        }

        private void trimMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsFrameMode())
            {
                SetToZero(true);
            }
            else
            {
                TimeSpan start = new TimeSpan();
                TimeSpan end = new TimeSpan();

                if (!(startBox.Text.MatchesWildcard("*:*:*") && endBox.Text.MatchesWildcard("*:*:*") && TryParseTime(startBox.Text, out start) && TryParseTime(endBox.Text, out end)))
                    Reset();
            }
        }
    }
}
