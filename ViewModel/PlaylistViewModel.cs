using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.ViewModel
{
    public class PlaylistViewModel
    {
        public PlaylistViewModel()
        {
            songs = new List<Song>();
        }
        public Playlist playlist { get; set; }

        public IList<Song> songs { get; set; }
    }
}
