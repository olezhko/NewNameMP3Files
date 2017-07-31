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


        public void ClearAll()
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<MusicLibraryContext>(new DropCreateDatabaseIfModelChanges<MusicLibraryContext>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
