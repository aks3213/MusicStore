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
    public class SongController : Controller
    {
        // GET: SongController
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public SongController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;

        }
        public ActionResult Index(String id)
        {
            //myname.Equals(Enum.Parse(Name.John))
            var songs = from m in _context.Songs select m;
            if (!string.IsNullOrEmpty(id))
            {
                songs = songs.Where(s => s.Details.Contains(id) || s.Name.Contains(id));
            }
            return View(songs);
        }

        // GET: SongController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (id == null)
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

        // GET: SongController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SongController/Create
        //[Bind("Id,Name,Details,Genre,ScreenShot1,ScreenShot2,ScreenShot3,ScreenShot4,MovieFile")] Movie movie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Name,Details,Genre,Language,Thumbnail,SongFile")] Song song)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(song.Thumbnail.FileName);
                string extension = Path.GetExtension(song.Thumbnail.FileName);
                song.ThumbnailPath= fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string sspath1 = Path.Combine(wwwRootPath + "/thumbnails/", fileName);
                using (var fileStream = new FileStream(sspath1, FileMode.Create))
                {
                    await song.Thumbnail.CopyToAsync(fileStream); 
                }

                fileName = Path.GetFileNameWithoutExtension(song.SongFile.FileName);
                extension = Path.GetExtension(song.SongFile.FileName);
                song.SongPath= fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string moviepath = Path.Combine(wwwRootPath + "/songs/", fileName);
                using (var fileStream = new FileStream(moviepath, FileMode.Create))
                {
                    await song.SongFile.CopyToAsync(fileStream);
                }

                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(song);
        }

        // GET: SongController/Edit/5
        public async Task<ActionResult> Edit(int id)
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
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Details,Genre,Language,Thumbnail,SongFile")] Song song)
        {
            try
            {
                Song oldSong = await _context.Songs
             .FirstOrDefaultAsync(m => m.Id == id);
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
            return RedirectToAction(nameof(Index));
        }

        // GET: SongController/Delete/5
        public async Task<ActionResult> Delete(int id)
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
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var song = await _context.Songs.FindAsync(id);

            var thumbnailPath = Path.Combine(_hostEnvironment.WebRootPath, "thumbnails", song.ThumbnailPath);
            if (System.IO.File.Exists(thumbnailPath))
                System.IO.File.Delete(thumbnailPath);

            var songpath = Path.Combine(_hostEnvironment.WebRootPath, "songs", song.SongPath);
            if (System.IO.File.Exists(songpath))
                System.IO.File.Delete(songpath);

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}
