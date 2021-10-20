﻿using System.Collections.Generic;
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
using Nmkoder.IO;
using Stream = Nmkoder.Data.Streams.Stream;
using Nmkoder.Data.Streams;

namespace Nmkoder.Forms
{
    partial class MainForm
    {
        public TextBox streamDetailsBox { get { return streamDetails; } }
        public ComboBox trackListDefaultAudioBox { get { return trackListDefaultAudio; } }
        public ComboBox trackListDefaultSubsBox { get { return trackListDefaultSubs; } }

        public void RefreshStreamListUi()
        {
            string note = "Stream selection is not available in Batch Processing Mode.";

            if (RunTask.currentFileListMode == RunTask.FileListMode.BatchProcess)
                formatInfo.Text = note;
            else if (formatInfo.Text == note)
                formatInfo.Text = "";
        }

        private void streamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (streamList.SelectedItem == null)
                return;

            MediaStreamListEntry entry = (MediaStreamListEntry)streamList.SelectedItem;
            streamDetails.Text = TrackList.GetStreamDetails(entry.Stream, entry.MediaFile);
        }

        public void UpdateTrackListUpDownBtnsState ()
        {
            bool newState = streamList.SelectedItem != null;

            if (trackListMoveUpBtn.Visible != newState)
                trackListMoveUpBtn.Visible = newState;

            if (trackListMoveDownBtn.Visible != newState)
                trackListMoveDownBtn.Visible = newState;
        }

        public bool ignoreNextStreamListItemCheck;

        private void streamList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ignoreNextStreamListItemCheck)
            {
                ignoreNextStreamListItemCheck = false;
                return;
            }

            if(e.NewValue != e.CurrentValue)
                this.BeginInvoke((MethodInvoker)(() => OnCheckedStreamsChange()));
        }

        private void OnCheckedStreamsChange ()
        {
            UpdateDefaultStreamsUi();
        }

        public void UpdateDefaultStreamsUi ()
        {
            List<MediaStreamListEntry> v = streamList.CheckedItems.OfType<MediaStreamListEntry>().Where(x => x.Stream.Type == Stream.StreamType.Video).ToList();
            List<MediaStreamListEntry> a = streamList.CheckedItems.OfType<MediaStreamListEntry>().Where(x => x.Stream.Type == Stream.StreamType.Audio).ToList();
            List<MediaStreamListEntry> s = streamList.CheckedItems.OfType<MediaStreamListEntry>().Where(x => x.Stream.Type == Stream.StreamType.Subtitle).ToList();

            trackListDefaultAudio.Enabled = a != null && a.Count > 0;
            trackListDefaultSubs.Enabled = s != null && s.Count > 0;

            trackListDefaultAudio.Items.Clear();

            for (int i = 0; i < a.Count; i++)
            {
                string title = string.IsNullOrWhiteSpace(((AudioStream)(a[i].Stream)).Title) ? "" : $" ({((AudioStream)(a[i].Stream)).Title})";
                string lang = string.IsNullOrWhiteSpace(((AudioStream)(a[i].Stream)).Language) ? "" : $" ({((AudioStream)(a[i].Stream)).Language})";
                trackListDefaultAudio.Items.Add($"Track {i + 1}{title}{lang}");
            }

            if (a.Count > 0)
                trackListDefaultAudio.SelectedIndex = 0;

            trackListDefaultSubs.Items.Clear();
            trackListDefaultSubs.Items.Add($"None");

            for (int i = 0; i < s.Count; i++)
            {
                string title = string.IsNullOrWhiteSpace(((SubtitleStream)(s[i].Stream)).Title) ? "" : $" ({((SubtitleStream)(s[i].Stream)).Title})";
                string lang = string.IsNullOrWhiteSpace(((SubtitleStream)(s[i].Stream)).Language) ? "" : $" ({((SubtitleStream)(s[i].Stream)).Language})";
                trackListDefaultSubs.Items.Add($"Track {i + 1}{title}{lang}");
            }

            if (s.Count > 0)
                trackListDefaultSubs.SelectedIndex = 0;
        }

        private void streamList_Leave(object sender, EventArgs e)
        {
            QuickConvertUi.LoadMetadataGrid();
        }

        private void streamList_MouseDown(object sender, MouseEventArgs e)
        {
            if (streamList.IndexFromPoint(new Point(e.X, e.Y)) <= -1) // if no item was clicked
                streamList.SelectedItems.Clear();

            UpdateTrackListUpDownBtnsState();
        }

        private void trackListMoveUpBtn_Click(object sender, EventArgs e)
        {
            MoveTrackListItem(-1);
        }

        private void trackListMoveDownBtn_Click(object sender, EventArgs e)
        {
            MoveTrackListItem(1);
        }

        private void MoveTrackListItem(int direction)
        {
            if (streamList.SelectedItem == null || streamList.SelectedIndex < 0)
                return;

            int newIndex = streamList.SelectedIndex + direction;

            if (newIndex < 0 || newIndex >= streamList.Items.Count)
                return; // Index out of range - nothing to do

            bool isChecked = streamList.GetItemChecked(streamList.Items.IndexOf(streamList.SelectedItem));
            object selected = streamList.SelectedItem;

            streamList.Items.Remove(selected);
            streamList.Items.Insert(newIndex, selected);
            //RefreshIndex();
            streamList.SetSelected(newIndex, true);
            streamList.SetItemChecked(newIndex, isChecked);
            UpdateDefaultStreamsUi();
            QuickConvertUi.LoadMetadataGrid();
        }

        private void trackListCheckTracksBtn_Click(object sender, EventArgs e)
        {
            checkItemsContextMenu.Show(Cursor.Position);
        }

        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.CheckAll(true);
        }

        private void checkNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.CheckAll(false);
        }

        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.InvertSelection();
        }

        private void checkAllVideoTracksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.CheckTracksOfType(Stream.StreamType.Video);
        }

        private void checkAllAudioTracksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.CheckTracksOfType(Stream.StreamType.Audio);
        }

        private void checkAllSubtitleTracksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.CheckTracksOfType(Stream.StreamType.Subtitle);
        }
    }
}
