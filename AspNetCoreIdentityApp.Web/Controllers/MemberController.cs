using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
		public IActionResult PasswordChange()
		{
			
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);

			var checkOldPassword = await userManager.CheckPasswordAsync(currentUser!, request.PasswordOld);

			if (!checkOldPassword)
			{
				ModelState.AddModelError(string.Empty, "Eski şifre yanlış. Lütfen tekrar deneyiniz.");
				return View();
			}
			var result = await userManager.ChangePasswordAsync(currentUser!, request.PasswordOld, request.PasswordNew);

			if (!result.Succeeded)
			{
				ModelState.AddModelErrorList([.. result.Errors.Select(x=>x.Description)]);
				return View();
			}

			await userManager.UpdateSecurityStampAsync(currentUser!);
			await signInManager.SignOutAsync();
			await signInManager.PasswordSignInAsync(currentUser!, request.PasswordNew, true, false);

			TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";

			return View();
		}
	}
}
