using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public enum Genre
    {
        Disco,
        Classical,
        Rock,
        Jazz,
        Pop,
        Rap
    }
    public enum Language
    {
        Hindi,
        English,
        Tamil,
        Other
    }
    public class Song
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter the name for the song")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please details about the song")]
        public string Details { set; get; }

        [Required(ErrorMessage = "Please enter genre of the song")]
        public Genre Genre { set; get; }

        [Required(ErrorMessage = "Please enter the language for the song")]
        public Language Language { set; get; }

        public string ThumbnailPath { get; set; }
        [NotMapped]
        public IFormFile Thumbnail { get; set; }

        public string SongPath { get; set; }
        [NotMapped]
        public IFormFile SongFile { get; set; }

        public int AlbumId{ get; set; }
        public IList<SongPlaylist> SongPlaylists { get; set; }

        public static implicit operator Song(Album v)
        {
            throw new NotImplementedException();
        }
    }
}
