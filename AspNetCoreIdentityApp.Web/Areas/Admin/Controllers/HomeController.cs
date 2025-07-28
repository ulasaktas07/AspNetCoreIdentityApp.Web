using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
	public class HomeController(UserManager<AppUser> userManager) : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> UserList()
		{
			var users = await userManager.Users.ToListAsync();

			var userViewModels = users.Select(user => new UserViewModel
			{
				Id = user.Id,
				Name = user.UserName,
				Email = user.Email
			}).ToList();

			return View(userViewModels);
		}
	}
}
