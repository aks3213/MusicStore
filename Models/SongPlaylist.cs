using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class SongPlaylist
    {
        public int SongId { get; set; }
        public Song Song { get; set; }

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
