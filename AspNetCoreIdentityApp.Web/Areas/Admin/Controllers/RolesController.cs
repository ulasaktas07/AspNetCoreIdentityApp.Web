using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Controllers
{
	[Area("Admin")]

	public class RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : Controller
	{
		public IActionResult Index()
		{

			return View();
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
			return RedirectToAction(nameof(RolesController.Index));
		}
	}
}
