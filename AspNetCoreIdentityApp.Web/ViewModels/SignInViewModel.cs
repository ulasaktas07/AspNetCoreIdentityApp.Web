using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
	public class SignInViewModel
	{
		[EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi giriniz")]

		[Required(ErrorMessage = "Email alanı boş bırakılamaz")]
		[Display(Name = "Email")]
		public string? Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
		[Display(Name = "Şifre")]
		[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")] // Optional: Minimum length validation
		public string? Password { get; set; }

		[Display(Name = "Beni Hatırla")]
		public bool RememberMe { get; set; } 
	}
}
