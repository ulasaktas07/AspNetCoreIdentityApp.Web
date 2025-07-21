using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Localization
{
	public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
	{
		public override IdentityError DuplicateUserName(string userName)
		{
			return new() {Code = nameof(DuplicateUserName), Description = $"'{userName}' kullanıcı adı daha önce alınmış." };
		}

		public override IdentityError DuplicateEmail(string email)
		{
			return new() { Code = nameof(DuplicateEmail), Description = $"'{email}' e-posta adresi daha önce alınmış." };
		}

		public override IdentityError PasswordTooShort(int length)
		{
			return new() { Code = nameof(PasswordTooShort), Description = $"Parola en az 6 karakter uzunluğunda olmalıdır." };
		}
	}
}
