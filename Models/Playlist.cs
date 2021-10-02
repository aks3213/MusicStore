using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the name for the playlist")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please details about the playlist")]
        public string Details { set; get; }

        public string ThumbnailPath { get; set; }
        [NotMapped]
        public IFormFile Thumbnail { get; set; }
        //songs
        public IList<SongPlaylist> SongPlaylists { get; set; }
    }
}
