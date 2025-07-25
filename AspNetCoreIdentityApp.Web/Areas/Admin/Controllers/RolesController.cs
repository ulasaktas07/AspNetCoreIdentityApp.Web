using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Controllers
{
	[Area("Admin")]

	public class RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : Controller
	{
		public async Task<IActionResult> Index()
		{
			var roles = await roleManager.Roles.Select(r => new RoleViewModel
			{
				Id = r.Id,
				Name = r.Name!
			}).ToListAsync();

			return View(roles);
		}
		public IActionResult RoleCreate()
		{

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> RoleCreate(RoleCreateViewModel request)
		{
			var result = await roleManager.CreateAsync(new AppRole { Name = request.Name });
			if (!result.Succeeded)
			{
				ModelState.AddModelErrorList(result.Errors);
				return View();
			}

			TempData["SuccessMessage"] = "Rol başarıyla oluşturuldu.";

			return RedirectToAction(nameof(RolesController.Index));
		}

		public async Task<IActionResult> RoleUpdate(string id)
		{
			var role = await roleManager.FindByIdAsync(id);
			if (role == null)
			{
				throw new Exception("Rol bulunamadı");
			}

			return View(new RoleUpdateViewModel() { Id = role.Id, Name = role.Name! });
		}
		[HttpPost]
		public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
		{
			var role = await roleManager.FindByIdAsync(request.Id) ?? throw new Exception("Rol bulunamadı");
			role!.Name = request.Name;
			var result = await roleManager.UpdateAsync(role);

			if (!result.Succeeded)
			{
				ModelState.AddModelErrorList(result.Errors);
				return View();
			}
			ViewData["SuccessMessage"] = "Rol başarıyla güncellendi.";
			return View();
		}
		public async Task<IActionResult> RoleDelete(string id)
		{
			var roleDelete = await roleManager.FindByIdAsync(id) ?? throw new Exception("Rol bulunamadı");

			var result = await roleManager.DeleteAsync(roleDelete);
			if (!result.Succeeded)
			{
				throw new Exception(result.Errors.Select(x => x.Description).First());
			}

			TempData["SuccessMessage"] = "Rol başarıyla silindi.";

			return RedirectToAction(nameof(RolesController.Index));
		}

		public async Task<IActionResult> AssignRoleToUser(string id)
		{
			var user = await userManager.FindByIdAsync(id) ?? throw new Exception("Kullanıcı bulunamadı");

			ViewBag.userId = id;

			var roles = await roleManager.Roles.ToListAsync();

			var userRoles = await userManager.GetRolesAsync(user);

			var roleViewModelList = new List<AssignRoleToUserViewModel>();


			foreach (var role in roles)
			{
				var assignRoleToUserViewModel = new AssignRoleToUserViewModel
				{
					Id = role.Id,
					Name = role.Name!
				};
				if (userRoles.Contains(role.Name!))
				{
					assignRoleToUserViewModel.Exist = true;
				}
				roleViewModelList.Add(assignRoleToUserViewModel);

			}

			return View(roleViewModelList);
		}
		[HttpPost]
		public async Task<IActionResult> AssignRoleToUser(string userId, List<AssignRoleToUserViewModel> request)
		{

			var userToAssignRoles = await userManager.FindByIdAsync(userId);

			foreach (var role in request)
			{
				if (role.Exist)
				{
					var result = await userManager.AddToRoleAsync(userToAssignRoles!, role.Name);
				}
				else
				{
					var result = await userManager.RemoveFromRoleAsync(userToAssignRoles!, role.Name);

				}

			}
			return RedirectToAction(nameof(HomeController.UserList),"Home");

		}
	}
}
