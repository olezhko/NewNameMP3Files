using System.Windows.Forms;
using System.Collections.Generic;

namespace NewNameMP3Files.UserControls
{
    public partial class CheckBoxAlbum : UserControl
    {
        public List<string> songs = new List<string>();
        public CheckBoxAlbum(string nameOfAlbum)
        {
            checkBoxAlbumName.Text = nameOfAlbum;
            InitializeComponent();
            foreach(string song in songs)
            {
                checkedListBox1.Items.Add(song);
            }
        }

        public CheckBoxAlbum()
        {
            InitializeComponent();
        }

        public List<string> getCheckedTracksFromAlbum()
        {
            List<string> album = new List<string>();
            foreach (string checkedBox in checkedListBox1.CheckedItems)
            {
                album.Add(checkedBox);
            }
            return album;
        }

        private void checkBoxAlbumName_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxAlbumName.Checked == true)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }  
            } 
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
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
                checkedListBox1.Items.RemoveAt(checkedListBox1.SelectedIndex);
            }
        }
    }
}
