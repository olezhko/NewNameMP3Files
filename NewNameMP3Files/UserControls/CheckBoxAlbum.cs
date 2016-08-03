using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace NewNameMP3Files.UserControls
{
    public partial class CheckBoxAlbum : UserControl
    {
        public List<string> songs = new List<string>();
        public CheckBoxAlbum(string nameOfAlbum)
        {
            InitializeComponent();
            checkBoxAlbumName.Text = nameOfAlbum;
            foreach(string song in songs)
            {
                SongsCheckedListBox.Items.Add(song);
            }
        }

        public CheckBoxAlbum()
        {
            InitializeComponent();
        }

        public List<string> GetCheckedTracksFromAlbum()
        {
            return SongsCheckedListBox.CheckedItems.Cast<string>().ToList();
        }

        private void checkBoxAlbumName_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxAlbumName.Checked == true)
            {
                for (int i = 0; i < SongsCheckedListBox.Items.Count; i++)
                {
                    SongsCheckedListBox.SetItemChecked(i, true);
                }  
            } 
            else
            {
                for (int i = 0; i < SongsCheckedListBox.Items.Count; i++)
                {
                    SongsCheckedListBox.SetItemChecked(i, false);
                }  
            }
        }
        /// <summary>
        /// Удаление элементов из списка по нажатию Del
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                SongsCheckedListBox.Items.RemoveAt(SongsCheckedListBox.SelectedIndex);
            }
        }
    }
}
