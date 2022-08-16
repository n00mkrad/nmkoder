using Nmkoder.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.UI
{
    class UiUtils
    {
        public enum MessageType { Message, Warning, Error };

        public static DialogResult ShowMessageBox (string text, MessageType type = MessageType.Message)
        {
            MessageBoxIcon icon = MessageBoxIcon.Information;
            if (type == MessageType.Warning) icon = MessageBoxIcon.Warning;
            else if (type == MessageType.Error) icon = MessageBoxIcon.Error;

            MessageForm form = new MessageForm(text, $"Nmkoder - {type}");
            form.ShowDialog();
            return DialogResult.OK;
        }

        public static DialogResult ShowMessageBox(string text, string title, MessageBoxButtons btns)
        {
            MessageForm form = new MessageForm(text, title, btns);
            return form.ShowDialog();
        }

        public enum MoveDirection { Up = -1, Down = 1 };

        public static void MoveListViewItem(ListView listView, MoveDirection direction)
        {
            if (listView.SelectedItems.Count != 1)
                return;

            ListViewItem selected = listView.SelectedItems[0];
            int index = selected.Index;
            int count = listView.Items.Count;

            if (direction == MoveDirection.Up)
            {
                if (index == 0)
                {
                    listView.Items.Remove(selected);
                    listView.Items.Insert(count - 1, selected);
                }
                else
                {
                    listView.Items.Remove(selected);
                    listView.Items.Insert(index - 1, selected);
                }
            }
            else
            {
                if (index == count - 1)
                {
                    listView.Items.Remove(selected);
                    listView.Items.Insert(0, selected);
                }
                else
                {
                    listView.Items.Remove(selected);
                    listView.Items.Insert(index + 1, selected);
                }
            }
        }
    }
}
