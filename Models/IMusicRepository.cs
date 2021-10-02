using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public interface IMusicRepository
    {
        Song GetSong(int Id);
        IEnumerable<Song> GetAllSongs();
        Song AddSong(Song Song);
        Song UpdateSong(Song SongChanges);
        Song DeleteSong(int Id);

        Album GetAlbum(int Id);
        IEnumerable<Album> GetAllAlbums();
        Album AddAlbum(Album Album);
        Album UpdateAlbum(Album AlbumChanges);
        Album DeleteAlbum(int Id);

        Playlist GetPlaylist(int Id);
        IEnumerable<Playlist> GetAllPlaylists();
        Playlist AddPlaylist(Playlist Playlist);
        Playlist UpdatePlaylist(Playlist PlaylistChanges);
        Playlist DeletePlaylistm(int Id);
    }
}
