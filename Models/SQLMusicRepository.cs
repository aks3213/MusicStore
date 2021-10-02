using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class SQLMusicRepository:IMusicRepository
    {
        private readonly AppDbContext context;
        public SQLMusicRepository(AppDbContext context)
        {
            this.context = context;
        }
        Song IMusicRepository.AddSong(Song song)
        {
            context.Songs.Add(song);
            context.SaveChanges();
            return song;
        }
        Song IMusicRepository.DeleteSong(int Id)
        {
            Song song = context.Songs.Find(Id);
            if (song != null)
            {
                context.Songs.Remove(song);
                context.SaveChanges();
            }
            return song;
        }
        IEnumerable<Song> IMusicRepository.GetAllSongs()
        {
            return context.Songs;
        }

        Song IMusicRepository.GetSong(int id)
        {
            return context.Songs.Find(id);
        }

        Song IMusicRepository.UpdateSong(Song songChanges)
        {
            var song = context.Songs.Attach(songChanges);
            song.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return songChanges;
        }
        //////////////////////
        Album IMusicRepository.GetAlbum(int Id)
        {
            return context.Albums.Find(Id);
        }
        Album IMusicRepository.AddAlbum(Album Album)
        {
            context.Albums.Add(Album);
            context.SaveChanges();
            return Album;
        }
        Album IMusicRepository.UpdateAlbum(Album AlbumChanges)
        {
            var song = context.Albums.Attach(AlbumChanges);
            song.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return AlbumChanges;
        }
        Album IMusicRepository.DeleteAlbum(int Id)
        {
            Album Album = context.Albums.Find(Id);
            if (Album != null)
            {
                context.Albums.Remove(Album);
                context.SaveChanges();
            }
            return Album ;
        }
        IEnumerable<Album> IMusicRepository.GetAllAlbums()
        {
            return context.Albums;
        }
        ///////////////////////
        Playlist IMusicRepository.GetPlaylist(int Id)
        {
            return context.Playlists.Find(Id);
        }
        Playlist IMusicRepository.AddPlaylist(Playlist Playlist)
        {
            context.Playlists.Add(Playlist);
            context.SaveChanges();
            return Playlist;
        }
        Playlist IMusicRepository.UpdatePlaylist(Playlist PlaylistChanges)
        {
            var song = context.Playlists.Attach(PlaylistChanges);
            song.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return PlaylistChanges;
        }
        Playlist IMusicRepository.DeletePlaylistm(int Id)
        {
            Playlist Playlist = context.Playlists.Find(Id);
            if (Playlist != null)
            {
                context.Playlists.Remove(Playlist);
                context.SaveChanges();
            }
            return Playlist;
        }
        IEnumerable<Playlist> IMusicRepository.GetAllPlaylists()
        {
            return context.Playlists;
        }

    }
}
