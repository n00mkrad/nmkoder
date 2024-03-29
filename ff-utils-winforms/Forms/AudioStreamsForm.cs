﻿using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.UI;
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

namespace Nmkoder.Forms
{
    public partial class AudioStreamsForm : Form
    {
        private MediaFile current;
        private int baseBitrate;

        public List<AudioConfigurationEntry> ConfigurationEntries { get; set; } = new List<AudioConfigurationEntry>();

        public AudioStreamsForm(MediaFile mf, int baseBr)
        {
            current = mf;
            baseBitrate = baseBr;
            InitializeComponent();
        }

        private void PromptForm_Load(object sender, EventArgs e)
        {

        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                DataGridViewRow row = grid.Rows[i];
                int ch = row.Cells[3].Value.ToString().GetInt().Clamp(1, 24);
                int kbps = row.Cells[4].Value.ToString().GetInt().Clamp(8, 8192);
                ConfigurationEntries.Add(new AudioConfigurationEntry(i, ch, kbps));
            }

            DialogResult = DialogResult.OK;
            Close();
            Program.mainForm.BringToFront();
        }

        private void AudioStreamsForm_Shown(object sender, EventArgs e)
        {
            grid.Columns.Clear();
            grid.Columns.Add("0", "Track");
            grid.Columns.Add("1", "Title");
            grid.Columns.Add("2", "Lang");
            grid.Columns.Add("3", "Channels");
            grid.Columns.Add("4", "Bitrate (Kbps)");

            grid.Rows.Clear();

            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            grid.Columns[0].ReadOnly = true;
            grid.Columns[1].ReadOnly = true;
            grid.Columns[2].ReadOnly = true;
            grid.Columns[0].FillWeight = 10;
            grid.Columns[1].FillWeight = 45;
            grid.Columns[2].FillWeight = 10;
            grid.Columns[3].FillWeight = 15;
            grid.Columns[4].FillWeight = 20;

            List<ListViewItem> streamItems = Program.mainForm.streamList.Items.Cast<ListViewItem>().ToList();
            List<AudioStream> audStreams = streamItems.Select(x => (StreamListEntry)x.Tag).Where(x => x.Stream.Type == Stream.StreamType.Audio).Select(x => (AudioStream)x.Stream).ToList();

            List<AudioConfigurationEntry> currentEntries = TrackList.currentAudioConfig?.GetConfig(current);

            for (int i = 0; i < audStreams.Count; i++)
            {
                AudioStream s = audStreams[i];
                int br = (baseBitrate * MiscUtils.GetAudioBitrateMultiplier(s.Channels)).RoundToInt();
                string title = string.IsNullOrWhiteSpace(s.Title) ? "None" : s.Title.Trunc(35);
                int newIdx = -1;

                if(currentEntries == null)
                    newIdx = grid.Rows.Add($"#{i + 1}", title, s.Language.ToUpper(), s.Channels, br);
                else
                    newIdx = grid.Rows.Add($"#{i + 1}", title, s.Language.ToUpper(), currentEntries[i].ChannelCount, currentEntries[i].BitrateKbps);

                grid.Rows[newIdx].Visible = Program.mainForm.streamList.Items[s.Index].Checked;
            }

        }
    }
}
