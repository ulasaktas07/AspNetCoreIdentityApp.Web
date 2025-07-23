using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi giriniz")]

		[Required(ErrorMessage = "Email alanı boş bırakılamaz")]
		[Display(Name = "Email")]
		public string? Email { get; set; }

	}
}
