using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AspNetCoreIdentityApp.Web.Controllers
{
	[Authorize]
	public class MemberController(SignInManager<AppUser> signInManager) : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public async Task Logout()
		{
			await signInManager.SignOutAsync();
		}
	}
}
