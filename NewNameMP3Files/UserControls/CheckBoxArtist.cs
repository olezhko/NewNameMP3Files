using System.Windows.Forms;
using System.Collections.Generic;
using NewNameMP3Files.UserControls;
using NewNameMP3Files.Code;

namespace NewNameMP3Files
{
    public partial class CheckBoxArtist : UserControl
    {
        public List<CheckBoxAlbum> _listAlbums;
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
                    for (int i = 0; i < album.SongsCheckedListBox.Items.Count; i++)
                    {
                        album.SongsCheckedListBox.SetItemChecked(i, true);
                    } 
                } 
                else
                {
                    album.checkBoxAlbumName.Checked = false;
                    for (int i = 0; i < album.SongsCheckedListBox.Items.Count; i++)
                    {
                        album.SongsCheckedListBox.SetItemChecked(i, false);
                    } 
                }
            }
        }

        public void AddNewAlbum(List<Album> list)
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
                _listAlbums[i].Margin = new System.Windows.Forms.Padding(10,0,0,0);
                _listAlbums[i].Name = album.Name;
                _listAlbums[i].checkBoxAlbumName.Text = album.Name;
                foreach (string a in album.Songs)
                {
                    _listAlbums[i].SongsCheckedListBox.Items.Add(a);
                }
                
                _listAlbums[i].SongsCheckedListBox.Size = new System.Drawing.Size(1000, _listAlbums[i].SongsCheckedListBox.Items.Count * 18);
                //_listAlbums[i].Size = _listAlbums[i].AlbumTableLayoutPanel.Size;
                ControlHeight += _listAlbums[i].AlbumTableLayoutPanel.Size.Height;
                //tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, _listAlbums[i].AlbumTableLayoutPanel.Size.Height));
                i++;
            }

            ControlHeight += 50; // for main checkbox height
        }

        public List<string> GetCheckedTracksFromArtist()
        {
            List<string> artist = new List<string>();
            foreach (CheckBoxAlbum album in _listAlbums)
            {
                List<string> albumList = album.GetCheckedTracksFromAlbum();
                artist.AddRange(albumList);
            }
            return artist;
        }

        public int ControlHeight { get; set; }
    }
}
