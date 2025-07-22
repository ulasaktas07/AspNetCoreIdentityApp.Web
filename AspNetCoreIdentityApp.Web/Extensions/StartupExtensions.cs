using AspNetCoreIdentityApp.Web.CustomValidations;
using AspNetCoreIdentityApp.Web.Localization;
using AspNetCoreIdentityApp.Web.Models;

namespace AspNetCoreIdentityApp.Web.Extensions
{
	public static class StartupExtensions
	{
		public static void AddIdentityWithExt(this IServiceCollection services)
		{
			services.AddIdentity<AppUser, AppRole>(options =>
			{
				options.User.RequireUniqueEmail = true;// Require unique email addresses
				options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+?";// Allowed characters in username
				options.Password.RequiredLength = 6; // Minimum length of the password
				options.Password.RequireNonAlphanumeric = false; // Require at least one non-alphanumeric character
				options.Password.RequireLowercase = true; // Require at least one lowercase letter
				options.Password.RequireUppercase = false; // Require at least one uppercase letter
				options.Password.RequireDigit = false; // Require at least one digit


				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);// Default lockout time span
				options.Lockout.MaxFailedAccessAttempts = 3;// Maximum number of failed attempts before lockout



			}).AddPasswordValidator<PasswordValidator>().AddUserValidator<UserValidator>().AddErrorDescriber<LocalizationIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>();//
		}
	}
}
