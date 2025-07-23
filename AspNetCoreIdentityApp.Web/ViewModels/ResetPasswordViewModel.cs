using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
	public class ResetPasswordViewModel
	{
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]

		[Display(Name = "Yeni Şifre")]
		public string? Password { get; set; }


		[DataType(DataType.Password)]

		[Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor. Lütfen tekrar deneyiniz.")]

		[Required(ErrorMessage = "Şifre Tekrar alanı boş bırakılamaz")]

		[Display(Name = "Yeni Şifre Tekrar")]
		public string? PasswordConfirm { get; set; }
	}
}
