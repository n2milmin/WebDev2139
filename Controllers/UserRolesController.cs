using Lab2.Areas.ProjectManagement.Models;
using Lab2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
	//[Authorize(Roles = "SuperAdmin, Admin")]
	public class UserRolesController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		
		public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

        private async Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser u)
        {
            return new List<string>(await _userManager.GetRolesAsync(u));
        }



        [HttpGet]
		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync();
			var userRolesViewModel = new List<UserRoleViewModel>();

			foreach (ApplicationUser u in users) 
			{
				var thisViewModel = new UserRoleViewModel();
				thisViewModel.UserId = u.Id;
				thisViewModel.FirstName = u.FirstName;
				thisViewModel.LastName = u.LastName;
				thisViewModel.Email = u.Email;
				thisViewModel.Roles = await GetUserRolesAsync(u);
				userRolesViewModel.Add(thisViewModel);
			}
			
			return View(userRolesViewModel);
		}



		[HttpGet]
		public async Task<IActionResult> Manage(string userId)
		{
			ViewBag.UserId = userId;
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				ViewBag.ErrorMessage = $"User with ID = {userId} cannot be found.";
				return View("NotFound");
			}

			ViewBag.UserName = user.UserName;
			var model = new List<ManageUserRolesViewModel>();
			
			foreach (var role in _roleManager.Roles)
			{
				var userRolesViewModel = new ManageUserRolesViewModel
				{
					RolesId = role.Id,
					RoleName = role.Name,
				};

				if (await _userManager.IsInRoleAsync(user, role.Name))
				{
					userRolesViewModel.Selected = true;
				}
				else
				{
					userRolesViewModel.Selected = false;
				}

				model.Add(userRolesViewModel);
			}

			return View(model);
		}



		[HttpPost]
		//[Authorize(Roles = "SuperAdmin, Admin")]
		public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return View();
			}

			var roles = await _userManager.GetRolesAsync(user);
			var result = await _userManager.RemoveFromRolesAsync(user, roles);
			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Cannot remove user from roles");
				return View(model);
			}

			result = await _userManager
					.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Cannot add user to roles.");
				return View(model);
			}

			return RedirectToAction("Index");
		}
	}
}
