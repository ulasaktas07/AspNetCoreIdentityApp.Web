using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
	public class SignInViewModel
	{
		[EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi giriniz")]

		[Required(ErrorMessage = "Email alanı boş bırakılamaz")]
		[Display(Name = "Email")]
		public string? Email { get; set; }


		[Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
		[Display(Name = "Şifre")]
		public string? Password { get; set; }

		[Display(Name = "Beni Hatırla")]
		public bool RememberMe { get; set; } 
	}
}
