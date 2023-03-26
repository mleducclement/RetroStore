using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RetroStoreWeb.Areas.Admin.Controllers {

    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminPanelController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult CreateRole() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName) {
            if (!string.IsNullOrWhiteSpace(roleName)) {
                var role = new IdentityRole(roleName);
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded) {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        public IActionResult ListRoles() {
            var roles = _roleManager.Roles;

            return View(roles);
        }
    }
}
