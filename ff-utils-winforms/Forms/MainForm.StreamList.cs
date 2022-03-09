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
using Nmkoder.IO;
using Stream = Nmkoder.Data.Streams.Stream;
using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.Media;

namespace Nmkoder.Forms
{
    partial class MainForm
    {
        public ListView streamList { get { return streamListView; } }
        public TextBox streamDetailsBox { get { return streamDetails; } }
        public ComboBox trackListDefaultAudioBox { get { return trackListDefaultAudio; } }
        public ComboBox trackListDefaultSubsBox { get { return trackListDefaultSubs; } }

        public void RefreshStreamListUi()
        {
            string note = "Manual track selection is not available in Batch Processing Mode.";

            if (RunTask.currentFileListMode == RunTask.FileListMode.Batch)
                formatInfo.Text = note;
            else if (formatInfo.Text == note)
                formatInfo.Text = "";
        }

        private void streamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTrackListBtnsState();

            if (streamList.SelectedItems.Count < 1)
            {
                streamDetails.Text = "";
                return;
            }

            StreamListEntry entry = (StreamListEntry)streamList.SelectedItems[0].Tag;
            streamDetails.Text = TrackList.GetStreamDetails(entry.Stream, entry.MediaFile);
        }

        public void UpdateTrackListBtnsState()
        {
            bool upDownBtnsEnabled = streamList.SelectedItems.Count == 1;
            if (trackListMoveUpBtn.Visible != upDownBtnsEnabled) trackListMoveUpBtn.Visible = upDownBtnsEnabled;
            if (trackListMoveDownBtn.Visible != upDownBtnsEnabled) trackListMoveDownBtn.Visible = upDownBtnsEnabled;

            bool extractBtnEnabled = streamList.SelectedItems.Count > 0 && ((StreamListEntry)streamList.SelectedItems[0].Tag).Stream.Type == Stream.StreamType.Attachment;
            if (trackListExtractTracksBtn.Visible != extractBtnEnabled) trackListExtractTracksBtn.Visible = extractBtnEnabled;
        }

        public bool ignoreStreamListCheck;

        private void streamList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ignoreStreamListCheck)
                return;

            if (e.NewValue != e.CurrentValue)
                this.BeginInvoke((MethodInvoker)(() => OnCheckedStreamsChange()));
        }

        public void OnCheckedStreamsChange()
        {
            UpdateDefaultStreamsUi();
            QuickConvertUi.LoadMetadataGrid();
        }

        public void UpdateDefaultStreamsUi()
        {
            var checkedStreamEntries = streamList.CheckedItems.Cast<ListViewItem>().Select(x => (StreamListEntry)x.Tag);
            List<StreamListEntry> v = checkedStreamEntries.Where(x => (x.Stream.Type == Stream.StreamType.Video)).ToList();
            List<StreamListEntry> a = checkedStreamEntries.Where(x => (x.Stream.Type == Stream.StreamType.Audio)).ToList();
            List<StreamListEntry> s = checkedStreamEntries.Where(x => (x.Stream.Type == Stream.StreamType.Subtitle)).ToList();

            trackListDefaultAudio.Enabled = a != null && a.Count > 0;
            trackListDefaultSubs.Enabled = s != null && s.Count > 0;

            trackListDefaultAudio.Items.Clear();

            bool zeroIdx = Config.GetBool(Config.Key.UseZeroIndexedStreams);

            for (int i = 0; i < a.Count; i++)
            {
                var stream = (AudioStream)a[i].Stream;
                List<string> items = new List<string>();
                items.Add($"#{(zeroIdx ? i : i + 1).ToString().PadLeft(2, '0')}");
                items.Add(stream.Language.ToUpper().Trunc(6));
                items.Add(stream.Title.Trunc(22));
                trackListDefaultAudio.Items.Add(string.Join(" - ", items.Where(x => !string.IsNullOrWhiteSpace(x))));
            }

            if (a.Count > 0)
                trackListDefaultAudio.SelectedIndex = 0;

            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].Stream.IsDefault)
                {
                    trackListDefaultAudio.SelectedIndex = i;
                    break;
                }
            }

            trackListDefaultSubs.Items.Clear();
            trackListDefaultSubs.Items.Add($"None");

            for (int i = 0; i < s.Count; i++)
            {
                var stream = (SubtitleStream)s[i].Stream;
                List<string> items = new List<string>();
                items.Add($"#{(zeroIdx ? i : i + 1).ToString().PadLeft(2, '0')}");
                items.Add(stream.Language.ToUpper().Trunc(6));
                items.Add(stream.Title.Trunc(18));
                trackListDefaultSubs.Items.Add($"{string.Join(" - ", items.Where(x => !string.IsNullOrWhiteSpace(x)))} ({Aliases.GetNicerCodecName(stream.Codec).Trunc(10)})");
            }

            if (s.Count > 0)
                trackListDefaultSubs.SelectedIndex = 0;

            for (int i = 0; i < s.Count; i++)
            {
                if (s[i].Stream.IsDefault)
                {
                    trackListDefaultSubs.SelectedIndex = i + 1; // Increment by 1 to account for the "None" entry
                    break;
                }
            }
        }

        private void streamList_Leave(object sender, EventArgs e)
        {
            QuickConvertUi.LoadMetadataGrid();
        }

        #region Move Buttons

        private void trackListMoveUpBtn_Click(object sender, EventArgs e)
        {
            UiUtils.MoveListViewItem(streamList, UiUtils.MoveDirection.Up);
        }

        private void trackListMoveDownBtn_Click(object sender, EventArgs e)
        {
            UiUtils.MoveListViewItem(streamList, UiUtils.MoveDirection.Down);
        }

        #endregion

        #region Auto-Check

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

        private void checkFirstTrackOfEachTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.CheckFirstOfEachType();
        }

        private void checkFirstTrackOfEachLanguagePerTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.CheckFirstOfEachLangOfEachType();
        }

        #endregion

        #region Sort

        private void trackListSortTracksBtn_Click(object sender, EventArgs e)
        {
            sortTracksContextMenu.Show(Cursor.Position);
        }

        private void sortTracksByLanguageAZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.SortTracks(TrackList.TrackSort.Language, false);
        }

        private void sortTracksByLanguageZAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.SortTracks(TrackList.TrackSort.Language, true);
        }

        private void sortTracksByTitleAZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.SortTracks(TrackList.TrackSort.Title, false);
        }

        private void sortTracksByTitleZAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.SortTracks(TrackList.TrackSort.Title, true);
        }

        private void sortTracksByCodecAZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.SortTracks(TrackList.TrackSort.Codec, false);
        }

        private void sortTracksByCodecZAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrackList.SortTracks(TrackList.TrackSort.Codec, true);
        }

        #endregion
    }
}
