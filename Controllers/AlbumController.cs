using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AlbumController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;


        }
        public ActionResult AddSong(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddSong([Bind("Id,Name,Details,Genre,Language,AlbumId,Thumbnail,SongFile")] Song song)
        {
            if (ModelState.IsValid)
            {
                //Album album= _context.Albums.FirstOrDefault(m => m.Id == id);
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(song.Thumbnail.FileName);
                string extension = Path.GetExtension(song.Thumbnail.FileName);
                song.ThumbnailPath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string sspath1 = Path.Combine(wwwRootPath + "/thumbnails/", fileName);
                using (var fileStream = new FileStream(sspath1, FileMode.Create))
                {
                    await song.Thumbnail.CopyToAsync(fileStream);
                }

                fileName = Path.GetFileNameWithoutExtension(song.SongFile.FileName);
                extension = Path.GetExtension(song.SongFile.FileName);
                song.SongPath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string moviepath = Path.Combine(wwwRootPath + "/songs/", fileName);
                using (var fileStream = new FileStream(moviepath, FileMode.Create))
                {
                    await song.SongFile.CopyToAsync(fileStream);
                }

                _context.Add(song);
                await _context.SaveChangesAsync();
                string Url = "~/Album/Details/" + song.AlbumId.ToString();
                return Redirect(Url);
            }
            return View(song);
        }
        public async Task<ActionResult> DetailsSong(int id)
        {
            var song = await _context.Songs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }
        public async Task<ActionResult> EditSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            return View(song);
        }

        // POST: SongController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSong(int id, [Bind("Id,Name,Details,Genre,Language,AlbumId,Thumbnail,SongFile")] Song song)
        {

            int idOfAlbum;
            try
            {
                Song oldSong = await _context.Songs
             .FirstOrDefaultAsync(m => m.Id == id);
                idOfAlbum = song.AlbumId;
                if (oldSong != null)
                {
                    _context.Entry(oldSong).State = EntityState.Detached;
                }
                //////////////////////////////////////////////////////////////////////////
                var oldpath = Path.Combine(_hostEnvironment.WebRootPath, "thumbnails", oldSong.ThumbnailPath);
                if (System.IO.File.Exists(oldpath))
                    System.IO.File.Delete(oldpath);

                var oldsongpath = Path.Combine(_hostEnvironment.WebRootPath, "songs", oldSong.SongPath);
                if (System.IO.File.Exists(oldsongpath))
                    System.IO.File.Delete(oldsongpath);
                ////////////////////////////////////////////////////////////
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(song.Thumbnail.FileName);
                string extension = Path.GetExtension(song.Thumbnail.FileName);
                song.ThumbnailPath = fileName = fileName + DateTime.Now.ToString("yymmoldfff") + extension;
                string sspath1 = Path.Combine(wwwRootPath + "/thumbnails/", fileName);
                using (var fileStream = new FileStream(sspath1, FileMode.Create))
                {
                    await song.Thumbnail.CopyToAsync(fileStream);
                }

                fileName = Path.GetFileNameWithoutExtension(song.SongFile.FileName);
                extension = Path.GetExtension(song.SongFile.FileName);
                song.SongPath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string moviepath = Path.Combine(wwwRootPath + "/songs/", fileName);
                using (var fileStream = new FileStream(moviepath, FileMode.Create))
                {
                    await song.SongFile.CopyToAsync(fileStream);
                }
                _context.Update(song);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(song.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            string Url = "~/Album/Details/" + idOfAlbum.ToString();
            return Redirect(Url);
        }

        // GET: SongController/Delete/5
        public async Task<ActionResult> DeleteSong(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: SongController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteSong(int id, IFormCollection collection)
        {
            var song = await _context.Songs.FindAsync(id);
            int idOfAlbum = song.AlbumId;
            var thumbnailPath = Path.Combine(_hostEnvironment.WebRootPath, "thumbnails", song.ThumbnailPath);
            if (System.IO.File.Exists(thumbnailPath))
                System.IO.File.Delete(thumbnailPath);

            var songpath = Path.Combine(_hostEnvironment.WebRootPath, "songs", song.SongPath);
            if (System.IO.File.Exists(songpath))
                System.IO.File.Delete(songpath);

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            string Url = "~/Album/Details/" + idOfAlbum.ToString();
            return Redirect(Url);
        }
        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Index()
        {
            var albums = from m in _context.Albums select m;
            return View(albums);
        }
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var album = await _context.Albums.FirstOrDefaultAsync(m => m.Id == id);
            var song = (from songs in _context.Songs where songs.AlbumId == id select songs);//from cust in db.Customers where cust.City == "London" select cust;
            album.Song = song.ToList();
            if (album == null) 
            {
                return NotFound();
            }

            return View(album);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: SongController/Create
        //[Bind("Id,Name,Details,Genre,ScreenShot1,ScreenShot2,ScreenShot3,ScreenShot4,MovieFile")] Movie movie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Name,Details,Thumbnail")] Album album)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(album.Thumbnail.FileName);
                string extension = Path.GetExtension(album.Thumbnail.FileName);
                album.ThumbnailPath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string sspath1 = Path.Combine(wwwRootPath + "/thumbnails/", fileName);
                using (var fileStream = new FileStream(sspath1, FileMode.Create))
                {
                    await album.Thumbnail.CopyToAsync(fileStream);
                }

                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Details,Thumbnail")] Album album)
        {
            try
            {
                Album oldAlbum = await _context.Albums
             .FirstOrDefaultAsync(m => m.Id == id);
                if (oldAlbum != null)
                {
                    _context.Entry(oldAlbum).State = EntityState.Detached;
                }
                //////////////////////////////////////////////////////////////////////////
                var oldpath = Path.Combine(_hostEnvironment.WebRootPath, "thumbnails", oldAlbum.ThumbnailPath);
                if (System.IO.File.Exists(oldpath))
                    System.IO.File.Delete(oldpath);

                ////////////////////////////////////////////////////////////
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(album.Thumbnail.FileName);
                string extension = Path.GetExtension(album.Thumbnail.FileName);
                album.ThumbnailPath = fileName = fileName + DateTime.Now.ToString("yymmoldfff") + extension;
                string sspath1 = Path.Combine(wwwRootPath + "/thumbnails/", fileName);
                using (var fileStream = new FileStream(sspath1, FileMode.Create))
                {
                    await album.Thumbnail.CopyToAsync(fileStream);
                }

                _context.Update(album);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(album.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: SongController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var album = await _context.Albums.FindAsync(id);

            var thumbnailPath = Path.Combine(_hostEnvironment.WebRootPath, "thumbnails", album.ThumbnailPath);
            if (System.IO.File.Exists(thumbnailPath))
                System.IO.File.Delete(thumbnailPath);

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }
    }
}
