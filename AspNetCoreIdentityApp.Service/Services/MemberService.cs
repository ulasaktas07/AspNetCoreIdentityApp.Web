using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using AspNetCoreIdentityApp.Repository.Models;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Service.Services
{
	public class MemberService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileProvider fileProvider) : IMemberService
	{
		public async Task<(bool, IEnumerable<IdentityError>)> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
		{
			var currentUser = await userManager.FindByNameAsync(userName);
			var result = await userManager.ChangePasswordAsync(currentUser!, oldPassword, newPassword);

			if (!result.Succeeded)
			{
				return (false, result.Errors);
			}

			await userManager.UpdateSecurityStampAsync(currentUser!);
			await signInManager.SignOutAsync();
			await signInManager.PasswordSignInAsync(currentUser!, newPassword, true, false);

			return (true, Enumerable.Empty<IdentityError>());

		}

		public async Task<bool> CheckPasswordAsync(string userName, string password)
		{
			var currentUser = await userManager.FindByNameAsync(userName);

			return await userManager.CheckPasswordAsync(currentUser!, password);
		}

		public async Task<(bool, IEnumerable<IdentityError>)> EditUserAsync(UserEditViewModel request, string userName)
		{
			var currentUser = await userManager.FindByNameAsync(userName);

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

				var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userspicture").PhysicalPath!, randomFileName);

				using var stream = new FileStream(newPicturePath, FileMode.Create);

				await request.Picture.CopyToAsync(stream);

				currentUser.Picture = randomFileName;

			}
			var updateToUserResult = await userManager.UpdateAsync(currentUser!);
			if (!updateToUserResult.Succeeded)
			{
				return (false, updateToUserResult.Errors);
			}

			await userManager.UpdateSecurityStampAsync(currentUser!);
			await signInManager.SignOutAsync();

			if (request.BirthDate.HasValue)
			{
				await signInManager.SignInWithClaimsAsync(currentUser!, true, [new Claim("birthdate", currentUser.BirthDate!.Value.ToString())]);

			}
			else
			{
				await signInManager.SignInAsync(currentUser!, true);
			}

			return (true, Enumerable.Empty<IdentityError>());
		}

		public async Task<UserViewModel> GetUserByUserNameAsync(string userName)
		{

			var currentUser = await userManager.FindByNameAsync(userName);
			var userViewModel = new UserViewModel
			{
				UserName = currentUser!.UserName,
				Email = currentUser.Email,
				PhoneNumber = currentUser.PhoneNumber,
				PictureUrl = currentUser.Picture
			};
			return userViewModel;
		}

		public async Task<UserEditViewModel> GetUserEditAsync(string userName)
		{
			var currentUser = await userManager.FindByNameAsync(userName);
			return new UserEditViewModel
			{
				UserName = currentUser!.UserName!,
				Email = currentUser.Email!,
				Phone = currentUser.PhoneNumber,
				City = currentUser.City,
				BirthDate = currentUser.BirthDate,
				Gender = currentUser.Gender
			};
		}

		public async Task LogoutAsync()
		{
			await signInManager.SignOutAsync();
		}
		SelectList GetGenderSelectList()
		{
			return new SelectList(Enum.GetNames<Gender>());
		}

		SelectList IMemberService.GetGenderSelectList()
		{
			return GetGenderSelectList();
		}
	}
}
