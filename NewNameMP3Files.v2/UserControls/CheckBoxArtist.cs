using System.Windows.Forms;
using System.Collections.Generic;
using NewNameMP3Files.UserControls;
using NewNameMP3Files.Code;

namespace NewNameMP3Files
{
    public partial class CheckBoxArtist : UserControl
    {
        public List<CheckBoxAlbum> _listAlbums;
        public CheckBoxArtist(string name)
        {
            artistCheckBox.Text = name;
            _listAlbums = new List<CheckBoxAlbum>();
            InitializeComponent();
        }

        public CheckBoxArtist()
        {
            _listAlbums = new List<CheckBoxAlbum>();
            InitializeComponent();
        }

        private void artistCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            foreach (CheckBoxAlbum album in _listAlbums)
            {   
                if (artistCheckBox.Checked == true)
                {
                    album.checkBoxAlbumName.Checked = true;
                    for (int i = 0; i < album.checkedListBox1.Items.Count; i++)
                    {
                        album.checkedListBox1.SetItemChecked(i, true);
                    } 
                } 
                else
                {
                    album.checkBoxAlbumName.Checked = false;
                    for (int i = 0; i < album.checkedListBox1.Items.Count; i++)
                    {
                        album.checkedListBox1.SetItemChecked(i, false);
                    } 
                }
            }
        }

        public List<string> GetCheckedFiles()
        {
            List<string> checkedList = new List<string>();
            foreach (CheckBoxAlbum album in _listAlbums)
            {
                foreach (string checkedBox in album.checkedListBox1.CheckedItems)
                {
                    checkedList.Add(checkedBox);
                }
            }
            return checkedList;
        }

        internal void AddNewAlbum(List<Album> list)
        {        
            _listAlbums = new List<CheckBoxAlbum>();
            int i = 0;
            
            foreach(Album album in list)
            {
                _listAlbums.Add(new CheckBoxAlbum());
                tableLayoutPanel2.RowCount = i + 1;
                tableLayoutPanel2.Controls.Add(_listAlbums[i], 0, i);
                _listAlbums[i].Location = new System.Drawing.Point(0, 0);
                _listAlbums[i].Dock = DockStyle.Fill;
                _listAlbums[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                _listAlbums[i].Margin = new System.Windows.Forms.Padding(4);
                _listAlbums[i].Name = album._name;
                _listAlbums[i].TabIndex = i;
                _listAlbums[i].checkBoxAlbumName.Text = album._name;
                foreach (string a in album.songs)
                {
                    _listAlbums[i].checkedListBox1.Items.Add(a);
                }
                
                _listAlbums[i].checkedListBox1.Size = new System.Drawing.Size(1000, _listAlbums[i].checkedListBox1.Items.Count * 18);
                _listAlbums[i].Size = _listAlbums[i].tableLayoutPanel2.Size;
                //tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, _listAlbums[i].Size.Height));
                i++;
            }
        }

        public List<string> getCheckedTracksFromArtist()
        {
            List<string> artist = new List<string>();
            foreach (CheckBoxAlbum album in _listAlbums)
            {
                List<string> albumList = album.getCheckedTracksFromAlbum();
                foreach (string song in albumList)
                {
                    artist.Add(song);
                }
            }
            return artist;
        }
    }
}
