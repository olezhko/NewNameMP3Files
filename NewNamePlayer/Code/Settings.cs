using System;
using System.Globalization;
using System.Xml.Linq;
using System.IO;

namespace NewNamePlayer
{
    public class Settings
    {
        public bool IsMaximaze;
        public double NameColumnSize;
        public double NumberColumnSize;
        public double GenreColumnSize;
        public double ArtistColumnSize;
        public double AlbumColumnSize;
        public double DurationColumnSize;
        public double PathColumnSize;
        public double BitrateColumnSize;
        public double YearColumnSize;

        public string LastFmName;
        public string LastFmPassword;
        public bool IsLastFmScrobbling;

        public string MusicLibraryDirectory;
        public int Volume;

        public int LastSongIndex;

        private const string SettingsFilename = "playerconfig.cfg";

        public Settings()
        {
            if (File.Exists(SettingsFilename))
            {
                LoadSettings();
            }
            else
            {
                LoadDefaultSettings();
                SaveSettings();
            }
        }

        public void SaveSettings()
        {
            XDocument doc = new XDocument(
                new XElement("ApplicationLayout",
                    new XElement("ColumnSize",
                        new XElement("NameColumnSize", NameColumnSize),
                        new XElement("NumberColumnSize", NumberColumnSize),
                        new XElement("GenreColumnSize", GenreColumnSize),
                        new XElement("ArtistColumnSize", ArtistColumnSize),
                        new XElement("AlbumColumnSize", AlbumColumnSize),
                        new XElement("DurationColumnSize", DurationColumnSize),
                        new XElement("PathColumnSize", PathColumnSize),
                        new XElement("BitrateColumnSize", BitrateColumnSize),
                        new XElement("YearColumnSize", YearColumnSize),
                        new XElement("IsMaximaze", IsMaximaze)),
                new XElement("LastFM",
                    new XElement("IsLastFMScrobbling", IsLastFmScrobbling),
                    new XElement("LastFMName", LastFmName),
                    new XElement("LastFMPassword", LastFmPassword)),
                new XElement("ApplicationSettings",
                    new XElement("Volume", Volume),
                    new XElement("LastSongIndex", LastSongIndex),
                    new XElement("MusicLibraryDirectory", MusicLibraryDirectory)))
                );
            doc.Save(SettingsFilename);
        }

        private void LoadSettings()
        {
            try
            {
                var doc = XDocument.Load(SettingsFilename);
                NameColumnSize = Convert.ToDouble(doc.Element("ApplicationLayout").Element("ColumnSize").Element("NameColumnSize").Value,CultureInfo.InvariantCulture);
                NumberColumnSize = Convert.ToDouble(doc.Element("ApplicationLayout").Element("ColumnSize").Element("NumberColumnSize").Value, CultureInfo.InvariantCulture);
                GenreColumnSize = Convert.ToDouble(doc.Element("ApplicationLayout").Element("ColumnSize").Element("GenreColumnSize").Value, CultureInfo.InvariantCulture);
                ArtistColumnSize = Convert.ToDouble(doc.Element("ApplicationLayout").Element("ColumnSize").Element("ArtistColumnSize").Value, CultureInfo.InvariantCulture);
                AlbumColumnSize = Convert.ToDouble(doc.Element("ApplicationLayout").Element("ColumnSize").Element("AlbumColumnSize").Value, CultureInfo.InvariantCulture);
                DurationColumnSize = Convert.ToDouble(doc.Element("ApplicationLayout").Element("ColumnSize").Element("DurationColumnSize").Value, CultureInfo.InvariantCulture);
                PathColumnSize = Convert.ToDouble(doc.Element("ApplicationLayout").Element("ColumnSize").Element("PathColumnSize").Value, CultureInfo.InvariantCulture);
                BitrateColumnSize = Convert.ToDouble(doc.Element("ApplicationLayout").Element("ColumnSize").Element("BitrateColumnSize").Value, CultureInfo.InvariantCulture);
                YearColumnSize = Convert.ToDouble(doc.Element("ApplicationLayout").Element("ColumnSize").Element("YearColumnSize").Value, CultureInfo.InvariantCulture);
                IsMaximaze = Convert.ToBoolean(doc.Element("ApplicationLayout").Element("ColumnSize").Element("IsMaximaze").Value);

                IsLastFmScrobbling = Convert.ToBoolean(doc.Element("ApplicationLayout").Element("LastFM").Element("IsLastFMScrobbling").Value);
                LastFmName = doc.Element("ApplicationLayout").Element("LastFM").Element("LastFMName").Value;
                LastFmPassword = doc.Element("ApplicationLayout").Element("LastFM").Element("LastFMPassword").Value;

                Volume = Convert.ToInt32(doc.Element("ApplicationLayout").Element("ApplicationSettings").Element("Volume").Value, CultureInfo.InvariantCulture);
                MusicLibraryDirectory = doc.Element("ApplicationLayout").Element("ApplicationSettings").Element("MusicLibraryDirectory").Value;
                LastSongIndex = Convert.ToInt32(doc.Element("ApplicationLayout").Element("ApplicationSettings").Element("LastSongIndex").Value, CultureInfo.InvariantCulture);
            }
            catch
            {
                File.Delete(SettingsFilename);
                LoadDefaultSettings();
            } 
        }

        private void LoadDefaultSettings()
        {
            NameColumnSize = System.Windows.SystemParameters.PrimaryScreenWidth / 9;
            NumberColumnSize = System.Windows.SystemParameters.PrimaryScreenWidth / 9;
            GenreColumnSize = System.Windows.SystemParameters.PrimaryScreenWidth / 9;
            ArtistColumnSize = System.Windows.SystemParameters.PrimaryScreenWidth / 9;
            AlbumColumnSize = System.Windows.SystemParameters.PrimaryScreenWidth / 9;
            DurationColumnSize = System.Windows.SystemParameters.PrimaryScreenWidth / 9;
            PathColumnSize = System.Windows.SystemParameters.PrimaryScreenWidth / 9;
            BitrateColumnSize = System.Windows.SystemParameters.PrimaryScreenWidth / 9;
            YearColumnSize = System.Windows.SystemParameters.PrimaryScreenWidth / 9;

            Volume = 100;

            LastFmName = " ";
            LastFmPassword =  " ";
            IsLastFmScrobbling = false;

            MusicLibraryDirectory = " ";
            LastSongIndex = 0;
        }
    }
}
