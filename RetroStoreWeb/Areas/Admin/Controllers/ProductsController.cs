﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RetroStore.DataAccess;
using RetroStore.Models.Models;
using RetroStore.Models.ViewModels;
using RetroStore.Utility.ExtensionMethods;

namespace RetroStoreWeb.Areas.Admin.Controllers {

    [Area("Admin")]
    public class ProductsController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index() {
            IEnumerable<Product> products = await _context.Products.ToListAsync();

            return View(products);
        }

        // GET
        public async Task<IActionResult> Create() {
            IEnumerable<Genre> genres = await _context.Genres.ToListAsync();

            ProductView productView = new ProductView
            {
                Product = new(),
                GenreListItems = genres.Select(genre => new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString()
                })
            };

            return View(productView);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductView productView, IFormFile? file) {
            var duplicateProduct = _context.Products.FirstOrDefault(p => p.Name == productView.Product.Name && p.Id != productView.Product.Id);

            if (duplicateProduct != null) {
                ModelState.AddModelError("Duplicate Product", "A product with this name already exists");

                return View(productView);
            }

            IEnumerable<Genre> genres = await _context.Genres.ToListAsync();
            productView.GenreListItems = genres.Select(genre => new SelectListItem
            {
                Text = genre.Name,
                Value = genre.Id.ToString()
            });

            if (file == null) {
                ModelState.AddModelError("FileURL", "Please upload a file");
            }

            if (ModelState.IsValid) {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null) {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(wwwRootPath, @"assets\img\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (productView.Product.ImageUrl != null) {
                        var oldImagePath = Path.Combine(wwwRootPath, productView.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath)) {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + extension), FileMode.Create)) {
                        file.CopyTo(fileStream);
                    }

                    productView.Product.ImageUrl = @"\assets\img\products\" + fileName + extension;
                }

                await _context.Products.AddAsync(productView.Product);
                await _context.SaveChangesAsync();
                TempData["success"] = "The product was updated successfully";
                return RedirectToAction("Index");
            }

            return View(productView);
        }

        // GET
        public async Task<IActionResult> Edit(int id) {

            IEnumerable<Genre> genres = await _context.Genres.ToListAsync();

            ProductView productView = new ProductView
            {
                Product = new(),
                GenreListItems = genres.Select(genre => new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString()
                })
            };

            if (id == 0) {
                return NotFound();
            }

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct != null) {
                productView.Product = existingProduct;
                return View(productView);
            }

            return NotFound();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductView productView, IFormFile? file) {
            var duplicateProduct = _context.Products.FirstOrDefault(p => p.Name == productView.Product.Name && p.Id != productView.Product.Id);

            if (duplicateProduct != null) {
                ModelState.AddModelError("Duplicate Product", "A product with this name already exists");

                return View(productView);
            }

            var originalProduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == productView.Product.Id);

            if (originalProduct == null) {
                return NotFound();
            }

            var productToUpdate = await _context.Products.FirstOrDefaultAsync(p => p.Id == productView.Product.Id);

            if (productToUpdate == null) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null) {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(wwwRootPath, @"assets\img\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (productView.Product.ImageUrl != null) {
                        var oldImagePath = Path.Combine(wwwRootPath, productView.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath)) {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + extension), FileMode.Create)) {
                        file.CopyTo(fileStream);
                    }

                    productView.Product.ImageUrl = @$"\assets\img\products{fileName}{extension}";
                }

                productToUpdate.Name = productView.Product.Name;
                productToUpdate.Description = productView.Product.Description;
                productToUpdate.GenreId = productView.Product.GenreId;
                productToUpdate.ListPrice = productView.Product.ListPrice;
                productToUpdate.Price = productView.Product.Price;
                productToUpdate.ReleaseDate = productView.Product.ReleaseDate;
                productToUpdate.Developer = productView.Product.Developer;
                productToUpdate.Publisher = productView.Product.Publisher;
                productToUpdate.Platform = productView.Product.Platform;

                Console.WriteLine(originalProduct.Equals(productToUpdate));

                if (!originalProduct.Equals(productToUpdate)) {
                    await _context.SaveChangesAsync();
                    TempData["success"] = "The product was updated successfully";
                }

                return RedirectToAction("Index");
            }

            return View(productView);
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var products = await _context.Products
                .Include(p => p.Genre)
                .ToListAsync();

            foreach (var product in products) {
                product.PlatformName = product.Platform.GetDescription();
                product.ReleaseDateWithoutTime = product.ReleaseDate.ToLongDateString();
            }

            return Json(new { data = products });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            var productToDelete = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productToDelete == null) {
                return Json(new { success = false, message = "Error while deleting product!" });
            }

            _context.Remove(productToDelete);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Product deleted successfully" });
        }

        #endregion
    }
}
