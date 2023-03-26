using Microsoft.AspNetCore.Mvc;
using RetroStore.Models.ViewModels;
using RetroStore.Utility.ExtensionMethods;

namespace RetroStoreWeb.Areas.Customer.Controllers {
    public class ShoppingCartController : Controller {

        [Area("Customer")]
        public IActionResult Index() {

            if (HttpContext.Session.Get<List<CartItemView>>("cart") == null) {
                HttpContext.Session.Set("cart", new List<CartItemView>());
            }

            List<CartItemView> cart = HttpContext.Session.Get<List<CartItemView>>("cart");

            ViewBag.TotalPrice = cart.Sum(p => p.Product.Price);

            return View(cart);
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

            return RedirectToAction("Index");
        }
    }
}
