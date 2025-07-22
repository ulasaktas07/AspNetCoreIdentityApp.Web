using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetCoreIdentityApp.Web.Controllers
{
	public class HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
		public IActionResult SignUp()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var identityResult = await userManager.CreateAsync(new() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.PasswordConfirm);


			if (identityResult.Succeeded)
			{
				TempData["SuccessMessage"] = "Kay�t ba�ar�l�!";
				return RedirectToAction(nameof(HomeController.SignUp));
			}

			ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

			return View();
		}
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
		{

			returnUrl = returnUrl ?? Url.Action("Index", "Home");

			var hasUser = await userManager.FindByEmailAsync(model.Email!);
			if (hasUser == null)
			{
				ModelState.AddModelError(string.Empty, "Kullan�c� bulunamad�!");
				return View();
			}

			var signInResult = await signInManager.PasswordSignInAsync(hasUser, model.Password!, model.RememberMe, true);

			if (signInResult.Succeeded)
			{
				return Redirect(returnUrl!);
			}

			if (signInResult.IsLockedOut)
			{
				ModelState.AddModelError(string.Empty, "Hesab�n�z kilitlendi. L�tfen daha sonra tekrar deneyiniz.");
				return View();
			}


			ModelState.AddModelErrorList(new List<string>() { $"Kullan�c� bulunamad�!",$"Ba�ar�s�z giri� say�s�={await userManager.GetAccessFailedCountAsync(hasUser)}" }); 

		

			return View();
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
