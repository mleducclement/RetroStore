using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RetroStore.DataAccess;
using RetroStore.Models.Models;
using RetroStore.Models.ViewModels;
using System.Data;

namespace RetroStoreWeb.Areas.Manager.Controllers {

    [Area("Manager")]
    [Authorize(Roles = "Administrator, Manager")]
    public class GenresController : Controller {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            IEnumerable<Genre> genres = await _context.Genres.ToListAsync();

            return View(genres);
        }

        // GET
        public IActionResult Create() {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre genre) {
            var duplicateGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genre.Name);

            if (duplicateGenre != null) {
                ModelState.AddModelError("Duplicate Genre", "A genre with this name already exists");

                return View();
            }

            if (ModelState.IsValid) {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                TempData["success"] = "The genre was added successfully";
                return RedirectToAction("Index");
            }


            return View();
        }

        // GET
        public async Task<IActionResult> Edit(int id) {
            if (id == 0) {
                return NotFound();
            }

            var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (existingGenre != null) {
                return View(existingGenre);
            }

            return NotFound();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Genre genre) {
            var duplicateGenre = _context.Genres.FirstOrDefault(g => g.Name == genre.Name && g.Id != genre.Id);

            if (duplicateGenre != null) {
                ModelState.AddModelError("Duplicate Genre", "A genre with this name already exists");

                return View(genre);
            }

            var genreToUpdate = await _context.Genres.FirstOrDefaultAsync(g => g.Id == genre.Id);

            if (genreToUpdate == null) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                genreToUpdate.Name = genre.Name;
                await _context.SaveChangesAsync();
                TempData["success"] = "The genre was updated successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var genres = await _context.Genres.ToListAsync();

            return Json(new { data = genres });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            var genreToDelete = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genreToDelete == null) {
                return Json(new { success = false, message = "Error while deleting genre!" });
            }

            _context.Remove(genreToDelete);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Genre deleted successfully" });
        }

        #endregion
    }
}
