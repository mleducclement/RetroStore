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
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) {
            _logger = logger;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(string jsonProduct, int count) {

            var product = JsonSerializer.Deserialize<Product>(jsonProduct);

            if (product == null) {
                return NotFound();
            }

            CartItemView cartItem = new CartItemView
            {
                Product = product,
                Count = count
            };

            // Check if there's a cart in the session. Create it if it doesn't exist
            if (HttpContext.Session.Get<List<CartItemView>>("cart") == null) {
                HttpContext.Session.Set("cart", new List<CartItemView>());
            }

            // Retrieve the cart from the session.
            List<CartItemView> cart = HttpContext.Session.Get<List<CartItemView>>("cart");

            // AddToCart the product to the cart.
            cart.Add(cartItem);

            HttpContext.Session.Set("cart", cart);

            // Redirect to the shopping cart page.
            return RedirectToAction("Index", "ShoppingCart");
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