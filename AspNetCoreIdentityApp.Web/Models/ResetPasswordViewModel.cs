using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models
{
	public class ResetPasswordViewModel
	{
		[EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi giriniz")]

		[Required(ErrorMessage = "Email alanı boş bırakılamaz")]
		[Display(Name = "Email")]
		public string? Email { get; set; }

	}
}
