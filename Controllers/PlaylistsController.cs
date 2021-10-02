using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Models;
using MusicStore.ViewModel;

namespace MusicStore.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PlaylistsController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult AddSong(int id)
        {
            ViewBag.pid = id;
            var songs = _context.Songs.ToList();
            List<SongPlaylist> songplaylists=new List<SongPlaylist>();
            foreach (var item in _context.SongPlaylists.ToList())
            {
                if (item.PlaylistId == id)
                {
                    songplaylists.Add(item);
                }
            }
            List<Song> songsToDisplay = new List<Song>();
            foreach(var song in songs)
            {
                Boolean isIn = true;
                foreach(var songplaylist in songplaylists)
                {
                    if (song.Id == songplaylist.SongId)
                    {
                        isIn = false;
                    }
                }
                if(!songsToDisplay.Contains(song) && isIn)
                {
                    System.Diagnostics.Debug.Print(" - - - - - - - -  "+song.Id);
                    songsToDisplay.Add(song);
                }
            }
            


            return View(songsToDisplay);
        }
        public async Task<IActionResult> AddSongToPlaylist(int id,string pid)
        {
            //System.Diagnostics.Debug.Print("---------------------------------------------" + id + "---------" + pid);
            var _playlist=await _context.Playlists.FindAsync(Convert.ToInt32(pid));
            //PlaylistViewModel obj = new PlaylistViewModel();
            SongPlaylist sp = new SongPlaylist()
            {
                Playlist = _playlist,
                Song =await _context.Songs.FindAsync(id)
            };
            _context.SongPlaylists.Add(sp);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = pid });
        }
        public async Task<IActionResult> RemoveSongFromPlaylist(int id, string pid)
        {
            //System.Diagnostics.Debug.Print("---------------------------------------------" + id + "---------" + pid);
            int playlistid = Convert.ToInt32(pid);
            var _songplaylist = (from m in _context.SongPlaylists
                                 where (m.SongId == id && m.PlaylistId == playlistid)
                                 select m).ToList();

            _context.SongPlaylists.Remove(_songplaylist[0]);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = pid });
        }
        // GET: Playlists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Playlists.ToListAsync());
        }

        // GET: Playlists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spls = await _context.SongPlaylists.ToListAsync();
            PlaylistViewModel obj = new PlaylistViewModel();
            // obj.songs = await _context.SongPlaylists.ToListAsync();
            var pl = await _context.Playlists.FindAsync(id);
            List<Song> songsInPlaylist = new List<Song>();
            foreach (var item in spls)
            {
                if(item.Playlist == pl)
                {
                    var sta = await  _context.Songs.FindAsync(item.SongId);
                    songsInPlaylist.Add(sta);
                }
            }
            obj.songs = songsInPlaylist;
            obj.playlist = pl;
            ViewBag.pid = pl.Id;
            return View(obj);
        }

        // GET: Playlists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Playlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Details,Thumbnail")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(playlist.Thumbnail.FileName);
                string extension = Path.GetExtension(playlist.Thumbnail.FileName);
                playlist.ThumbnailPath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string sspath1 = Path.Combine(wwwRootPath + "/thumbnails/", fileName);
                using (var fileStream = new FileStream(sspath1, FileMode.Create))
                {
                    await playlist.Thumbnail.CopyToAsync(fileStream);
                }
                _context.Add(playlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(playlist);
        }

        // GET: Playlists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        // POST: Playlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Details,Thumbnail")] Playlist playlist)
        {
            if (id != playlist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Playlist oldPlaylist= await _context.Playlists
             .FirstOrDefaultAsync(m => m.Id == id);
                    if (oldPlaylist != null)
                    {
                        _context.Entry(oldPlaylist).State = EntityState.Detached;
                    }
                    //////////////////////////////////////////////////////////////////////////
                    var oldpath = Path.Combine(_hostEnvironment.WebRootPath, "thumbnails", oldPlaylist.ThumbnailPath);
                    if (System.IO.File.Exists(oldpath))
                        System.IO.File.Delete(oldpath);

                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    string fileName = Path.GetFileNameWithoutExtension(playlist.Thumbnail.FileName);
                    string extension = Path.GetExtension(playlist.Thumbnail.FileName);
                    playlist.ThumbnailPath = fileName = fileName + DateTime.Now.ToString("yymmoldfff") + extension;
                    string sspath1 = Path.Combine(wwwRootPath + "/thumbnails/", fileName);
                    using (var fileStream = new FileStream(sspath1, FileMode.Create))
                    {
                        await playlist.Thumbnail.CopyToAsync(fileStream);
                    }

                    _context.Update(playlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistExists(playlist.Id))
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
            return View(playlist);
        }

        // GET: Playlists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlist == null)
            {
                return NotFound();
            }

            return View(playlist);
        }

        // POST: Playlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistExists(int id)
        {
            return _context.Playlists.Any(e => e.Id == id);
        }
    }
}
