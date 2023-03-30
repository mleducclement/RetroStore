using Microsoft.AspNetCore.Mvc;
using RetroStore.Models.Models;
using RetroStore.Models.ViewModels;
using RetroStore.Utility.ExtensionMethods;
using System.Text.Json;

namespace RetroStoreWeb.Areas.Customer.Controllers {

    [Area("Customer")]
    public class ShoppingCartController : Controller {

        public IActionResult Index() {

            if (HttpContext.Session.Get<List<CartItemView>>("cart") == null) {
                HttpContext.Session.Set("cart", new List<CartItemView>());
            }

            List<CartItemView> cart = HttpContext.Session.Get<List<CartItemView>>("cart");

            // ViewBag is tempdata storage for ONE request
            ViewBag.TotalPrice = cart.Sum(p => p.Product.Price * p.Count);

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string jsonProduct, int count) {

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
            var existingCartItem = cart.FirstOrDefault(item => item.Product.Id == cartItem.Product.Id);

            if (existingCartItem != null) {
                existingCartItem.Count += count;
            }
            else {
                cart.Add(cartItem);
            }

            HttpContext.Session.Set("cart", cart);

            // Redirect to the shopping cart page.
            return RedirectToAction("Index", "ShoppingCart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productId) {
            List<CartItemView> cart = HttpContext.Session.Get<List<CartItemView>>("cart");

            int index = cart.FindIndex(p => p.Product.Id == productId);

            if (index != -1) {
                cart.RemoveAt(index);
                HttpContext.Session.Set("cart", cart);
            }

            Console.WriteLine("HERE");

            return RedirectToAction("Index");
        }
    }
}
