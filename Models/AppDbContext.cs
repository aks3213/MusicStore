using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SongPlaylist>().HasKey(sc => new { sc.SongId, sc.PlaylistId });

            modelBuilder.Entity<SongPlaylist>()
            .HasOne(sc => sc.Song)
            .WithMany(s => s.SongPlaylists)
            .HasForeignKey(sc => sc.SongId);

            modelBuilder.Entity<SongPlaylist>()
           .HasOne(sc => sc.Playlist)
           .WithMany(s => s.SongPlaylists)
           .HasForeignKey(sc => sc.PlaylistId);

            /*
             modelBuilder.Entity<Reservation>().
                    HasOne(pt => pt.Car).WithMany(pt => pt.Reservations).HasForeignKey(p => p.CarId);

            modelBuilder.Entity<Reservation>().
                    HasOne(pt => pt.Customer).WithMany(pt => pt.Reservations).HasForeignKey(p => p.CustomerId);
             */

            modelBuilder.Entity<Song>().Property(p => p.Genre).HasConversion(
                    v => v.ToString(),
                    v => (Genre)Enum.Parse(typeof(Genre), v));
            modelBuilder.Entity<Song>().Property(p => p.Language).HasConversion(
                    v => v.ToString(),
                    v => (Language)Enum.Parse(typeof(Language), v));
        }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<SongPlaylist> SongPlaylists { get; set; }
    }
}
