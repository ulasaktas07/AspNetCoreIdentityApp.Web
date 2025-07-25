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

	public class RolesController( RoleManager<AppRole> roleManager) : Controller
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
				throw new Exception(result.Errors.Select(x=>x.Description).First());
			}

			TempData["SuccessMessage"] = "Rol başarıyla silindi.";

			return RedirectToAction(nameof(RolesController.Index));
		}
	}
}
