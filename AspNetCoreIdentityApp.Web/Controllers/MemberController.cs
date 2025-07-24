using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Web.Controllers
{
	[Authorize]
	public class MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,IFileProvider fileProvider) : Controller
	{
		public async Task<IActionResult> Index()
		{
			var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);
			var userViewModel = new UserViewModel
			{
				UserName = currentUser!.UserName,
				Email = currentUser.Email,
				PhoneNumber = currentUser.PhoneNumber,
				PictureUrl=currentUser.Picture
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
				ModelState.AddModelErrorList(result.Errors);
				return View();
			}

			await userManager.UpdateSecurityStampAsync(currentUser!);
			await signInManager.SignOutAsync();
			await signInManager.PasswordSignInAsync(currentUser!, request.PasswordNew, true, false);

			TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";

			return View();
		}

		public async Task<IActionResult> UserEdit()
		{
			ViewBag.genderList = new SelectList(Enum.GetNames<Gender>());
			var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);
			var userEditViewModel = new UserEditViewModel
			{
				UserName = currentUser!.UserName!,
				Email = currentUser.Email!,
				Phone = currentUser.PhoneNumber,
				City = currentUser.City,
				BirthDate = currentUser.BirthDate,
				Gender = currentUser.Gender
			};
			return View(userEditViewModel);

		}

		[HttpPost]
		public async Task<IActionResult> UserEdit(UserEditViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);

			currentUser!.UserName = request.UserName;
			currentUser.Email = request.Email;
			currentUser.PhoneNumber = request.Phone;
			currentUser.City = request.City;
			currentUser.BirthDate = request.BirthDate;
			currentUser.Gender = request.Gender;



			if (request.Picture != null && request.Picture.Length > 0)
			{
				var wwwrootFolder = fileProvider.GetDirectoryContents("wwwroot");
				var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Picture.FileName)}";

				var newPicturePath = Path.Combine(wwwrootFolder.First(x=>x.Name=="userspicture").PhysicalPath!, randomFileName);
				
				using var stream = new FileStream(newPicturePath, FileMode.Create);

				await request.Picture.CopyToAsync(stream);

				currentUser.Picture = randomFileName;


			}
			var updateToUserResult= await userManager.UpdateAsync(currentUser!);

			if (!updateToUserResult.Succeeded)
			{
				ModelState.AddModelErrorList(updateToUserResult.Errors);
				return View();
			}

			await userManager.UpdateSecurityStampAsync(currentUser!);
			await signInManager.SignOutAsync();
			await signInManager.SignInAsync(currentUser!, true);

			TempData["SuccessMessage"] = "Kullanıcı bilgileri başarıyla güncellendi.";

			var userEditViewModel = new UserEditViewModel
			{
				UserName = currentUser.UserName!,
				Email = currentUser.Email!,
				Phone = currentUser.PhoneNumber,
				City = currentUser.City,
				BirthDate = currentUser.BirthDate,
				Gender = currentUser.Gender,

			};

			return View(userEditViewModel);
		}
	}
}
