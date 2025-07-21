using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{
	public class PasswordValidator : IPasswordValidator<AppUser>
	{
		public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
		{
			var errors = new List<IdentityError>();
			if (password!.ToLower().Contains(user.UserName!.ToLower()))
			{
				errors.Add(new()
				{ Code = "PasswordContainsUserName", Description = "Şifre kullanıcı adını içeremez." });
			}
			if (password!.ToLower().StartsWith("0123"))
			{
				errors.Add(new()
				{ Code = "PasswordStartsWith0123", Description = "Şifre 0123 ile başlayamaz." });

			}

			if (password!.ToLower().Contains("1234") || password!.ToLower().Contains("2345") || password!.ToLower().Contains("3456") || password!.ToLower().Contains("4567") || password!.ToLower().Contains("5678") || password!.ToLower().Contains("6789"))
			{
				errors.Add(new()
				{ Code = "PasswordContainsSequentialNumbers", Description = "Şifre ardışık sayılar içeremez." });

			}
			if (errors.Any())
			{
				return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
			}
			return Task.FromResult(IdentityResult.Success);
		}
	}
}
