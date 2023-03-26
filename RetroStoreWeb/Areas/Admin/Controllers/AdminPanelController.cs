using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroStore.Models.ViewModels;

namespace RetroStoreWeb.Areas.Admin.Controllers {

    [Area("Admin")]
    public class AdminPanelController : Controller {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminPanelController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() {
            return View();
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

        public async Task<IActionResult> DeleteRole(string roleId) {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null) {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoleConfirmed(string roleId) {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null) {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded) {
                return RedirectToAction(nameof(ListRoles));
            }

            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(role);
        }

        public IActionResult ListRoles() {
            var roles = _roleManager.Roles;

            return View(roles);
        }

        public async Task<IActionResult> ListUsers() {

            var users = await _userManager.Users.ToListAsync();
            var userRoleViews = new List<UserRoleView>();

            foreach (var user in users) {
                var roles = await _userManager.GetRolesAsync(user);
                userRoleViews.Add(new UserRoleView
                {
                    User = user,
                    Roles = roles
                });
            }

            return View(userRoleViews);
        }

        public async Task<IActionResult> EditUserRoles(string userId) {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            var userRoleView = new UserRoleView
            {
                User = user,
                Roles = userRoles,
                AllRoles = allRoles
            };

            if (allRoles.Count == 0) {
                RedirectToAction("Error");
            }

            return View(userRoleView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> selectedRoles) {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = selectedRoles.Except(userRoles).ToList();
            var rolesToRemove = userRoles.Except(selectedRoles).ToList();

            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            return RedirectToAction("ListUsers");
        }
    }
}
