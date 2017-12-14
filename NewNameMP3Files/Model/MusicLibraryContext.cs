using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicLibrary;

namespace NewNameMP3Files.Model
{
    public class MusicLibraryContext : DbContext
    {
        public MusicLibraryContext():base("MusicDb")
        { }

        public DbSet<Song> Songs { get; set; }
        public DbSet<FolderItem> Folders { get; set; }

        public void ClearAllSongs()
        {
            Songs.RemoveRange(Songs);
            SaveChanges();
        }

        public void AddSong(Song item)
        {
            Songs.Add(item);
        }

        internal void RemoveSong(Song dbSong)
        {
            Songs.Remove(dbSong);
        }

        internal void RemoveSongs(IEnumerable<Song> dbSong)
        {
            Songs.RemoveRange(dbSong);
        }


        public void AddFolder(FolderItem item)
        {
            Folders.Add(item);
        }

        internal void RemoveSong(FolderItem item)
        {
            Folders.Remove(item);
        }

        internal void RemoveSongs(IEnumerable<FolderItem> items)
        {
            Folders.RemoveRange(items);
        }

        public void ClearAllFolders()
        {
            Folders.RemoveRange(Folders);
            SaveChanges();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<MusicLibraryContext>(new DropCreateDatabaseIfModelChanges<MusicLibraryContext>());
            base.OnModelCreating(modelBuilder);
        }
    }


    public class FolderItem
    {
        public int Id { get; set; }

        public string Name;
        public DateTime LastModified;
    }
}
