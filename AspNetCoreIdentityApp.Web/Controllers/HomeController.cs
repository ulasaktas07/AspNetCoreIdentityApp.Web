using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Services;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Web.Controllers
{
	public class HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
		: Controller
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

			var identityResult = await userManager.CreateAsync(new() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.PasswordConfirm!);

			if (!identityResult.Succeeded)
			{
				ModelState.AddModelErrorList([.. identityResult.Errors.Select(x => x.Description)]);
				return View();
			}

			var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());

			var user = await userManager.FindByNameAsync(request.UserName!);

			var claimResult = await userManager.AddClaimAsync(user!, exchangeExpireClaim);

			if (!claimResult.Succeeded)
			{
				ModelState.AddModelErrorList([.. claimResult.Errors.Select(x => x.Description)]);
				return View();
			}

			TempData["SuccessMessage"] = "Kayýt baþarýlý!";
			return RedirectToAction(nameof(HomeController.SignUp));



		}
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			returnUrl ??= Url.Action("Index", "Home");

			var hasUser = await userManager.FindByEmailAsync(model.Email!);
			if (hasUser == null)
			{
				ModelState.AddModelError(string.Empty, "Kullanýcý bulunamadý!");
				return View();
			}
						

			var signInResult = await signInManager.PasswordSignInAsync(hasUser, model.Password!, model.RememberMe, true);

			if (signInResult.IsLockedOut)
			{
				ModelState.AddModelError(string.Empty, "Hesabýnýz kilitlendi. Lütfen daha sonra tekrar deneyiniz.");
				return View();
			}

			if (!signInResult.Succeeded)
			{
				ModelState.AddModelErrorList([$"Kullanýcý bulunamadý!", $"Baþarýsýz giriþ sayýsý={await userManager.GetAccessFailedCountAsync(hasUser)}"]); 
				return View();

			}


			if (hasUser.BirthDate.HasValue)
			{
				await signInManager.SignInWithClaimsAsync(hasUser, model.RememberMe, [new Claim("birthdate", hasUser.BirthDate.Value.ToString())]);
			}


			return Redirect(returnUrl!);


		}

		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
		{
			var hasUser = await userManager.FindByEmailAsync(request.Email!);
			if (hasUser == null)
			{
				ModelState.AddModelError(string.Empty, "Kullanýcý bulunamadý!");
				return View();
			}

			string passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(hasUser);

			var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, token = passwordResetToken }, Request.Scheme);

			await emailService.SendResetPasswordEmail(passwordResetLink!, hasUser.Email!);


			TempData["SuccessMessage"] = "Þifre yenileme baðlantýsý e-posta adresinize gönderildi. Lütfen kontrol ediniz.";

			return RedirectToAction(nameof(ForgetPassword));

		}


		public IActionResult ResetPassword(string userId, string token)
		{
			TempData["UserId"] = userId;
			TempData["Token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
		{
			var userId = TempData["UserId"];
			var token = TempData["Token"];
			if (userId == null || token == null)
			{
				throw new Exception("Bir hata meydana geldi!");
			}

			var hasUser = await userManager.FindByIdAsync(userId.ToString()!);

			if (hasUser == null)
			{
				ModelState.AddModelError(string.Empty, "Kullanýcý bulunamadý!");
				return View();
			}
			var identityResult = await userManager.ResetPasswordAsync(hasUser, token.ToString()!, request.Password!);

			if (identityResult.Succeeded)
			{
				TempData["SuccessMessage"] = "Þifreniz baþarýyla yenilendi!";
				return RedirectToAction(nameof(HomeController.SignIn));
			}

			else
			{
				ModelState.AddModelErrorList([.. identityResult.Errors.Select(x => x.Description)]);

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
