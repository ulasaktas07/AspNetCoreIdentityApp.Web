using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
	[Authorize]
	public class MemberController(SignInManager<AppUser> signInManager,UserManager<AppUser> userManager) : Controller
	{
		public async Task<IActionResult> Index()
		{
			var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);
			var userViewModel = new UserViewModel
			{
				UserName = currentUser!.UserName,
				Email = currentUser.Email,
				PhoneNumber = currentUser.PhoneNumber
			};
			return View(userViewModel);
		}
		public async Task Logout()
		{
			await signInManager.SignOutAsync();
		}
	}
}
