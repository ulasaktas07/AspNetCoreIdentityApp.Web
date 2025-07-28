using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreIdentityApp.Service.Services
{
	public interface IMemberService
	{
		Task<UserViewModel> GetUserByUserNameAsync(string userName);
		Task LogoutAsync();
		Task<bool> CheckPasswordAsync(string userName, string password);

		Task<(bool, IEnumerable<IdentityError>)> ChangePasswordAsync(string userName, string oldPassword, string newPassword);

		Task<UserEditViewModel> GetUserEditAsync(string userName);

		SelectList GetGenderSelectList();

		Task<(bool, IEnumerable<IdentityError>)> EditUserAsync(UserEditViewModel request, string userName);

	}
}
