using System.Diagnostics;
using System.Threading.Tasks;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
	public class HomeController(UserManager<AppUser> userManager) : Controller
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
				TempData["SuccessMessage"] = "Kayýt baþarýlý!";
				return RedirectToAction(nameof(HomeController.SignUp));
			}

			foreach (IdentityError item in identityResult.Errors)
			{
				ModelState.AddModelError(string.Empty, item.Description);

			}


			return View();
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
