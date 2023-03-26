using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroStore.DataAccess;
using RetroStore.Models.Models;
using RetroStore.Models.ViewModels;
using RetroStore.Utility.ExtensionMethods;
using System.Diagnostics;
using System.Text.Json;

namespace RetroStoreWeb.Areas.Customer.Controllers {

    [Area("Customer")]
    public class HomeController : Controller {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            IEnumerable<Product> products = await _context.Products
                .Include(p => p.Genre)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Details(int id) {
            var product = await _context.Products
                .Include(p => p.Genre)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) {
                return View("Error");
            }

            CartItemView cart = new CartItemView()
            {
                Count = 1,
                Product = product
            };

            return View(cart);
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}