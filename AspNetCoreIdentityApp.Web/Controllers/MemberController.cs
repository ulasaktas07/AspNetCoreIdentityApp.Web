using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Repository.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Service.Services;

namespace AspNetCoreIdentityApp.Web.Controllers
{
	[Authorize]
	public class MemberController(IMemberService memberService) : Controller
	{
		private string userName => User.Identity!.Name!;
		public async Task<IActionResult> Index()
		{
			var userViewModel = await memberService.GetUserByUserNameAsync(userName);

			return View(userViewModel);
		}
		public async Task Logout()
		{
			await memberService.LogoutAsync();
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
		


			if (!await memberService.CheckPasswordAsync(userName,request.PasswordOld))
			{
				ModelState.AddModelError(string.Empty, "Eski şifre yanlış. Lütfen tekrar deneyiniz.");
				return View();
			}
			var (isSuccess,erorrs) = await memberService.ChangePasswordAsync(userName,request.PasswordOld,request.PasswordNew);

			if (!isSuccess)
			{
				ModelState.AddModelErrorList(erorrs);
				return View();
			}

			TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";

			return View();
		}

		public async Task<IActionResult> UserEdit()
		{
			ViewBag.genderList =memberService.GetGenderSelectList();

			return View(await memberService.GetUserEditAsync(userName));

		}

		[HttpPost]
		public async Task<IActionResult> UserEdit(UserEditViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var (isSuccess,errorr) = await memberService.EditUserAsync(request, userName);


			if (!isSuccess)
			{
				ModelState.AddModelErrorList(errorr);
				return View();
			}
			

		

			TempData["SuccessMessage"] = "Kullanıcı bilgileri başarıyla güncellendi.";

			return View(await memberService.GetUserEditAsync(userName));
		}


		[Authorize(Policy = "RizePolicy")]
		public IActionResult RizePage()
		{
			return View();
		}

		[Authorize(Policy = "ExchangeExpirePolicy")]
		public IActionResult ExchangePage()
		{
			return View();
		}

		[Authorize(Policy = "ViolencePolicy")]
		public IActionResult ViolencePage()
		{
			return View();
		}

		public IActionResult AccessDenied(string ReturnUrl)
		{
			string empty = string.Empty;
			string message = empty;

			message= "Bu sayfaya erişim izniniz bulunmamaktadır."; 

			ViewBag.message = message;

			return View();
		}

	}
}