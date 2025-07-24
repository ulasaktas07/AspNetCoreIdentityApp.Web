using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
	public class PasswordChangeViewModel
	{
		[DataType(DataType.Password)]

		[Required(ErrorMessage = "Eski Şifre alanı boş bırakılamaz")]

		[Display(Name = "Eski Şifre")]
		[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")] // Optional: Minimum length validation
		public string PasswordOld { get; set; }=null!;



		[DataType(DataType.Password)]

		[Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]

		[Display(Name = "Yeni Şifre")]
		public string PasswordNew { get; set; } = null!;



		[DataType(DataType.Password)]

		[Compare(nameof(PasswordNew), ErrorMessage = "Şifreler eşleşmiyor. Lütfen tekrar deneyiniz.")]

		[Required(ErrorMessage = "Yeni Şifre Tekrar alanı boş bırakılamaz")]

		[Display(Name = "Yeni Şifre Tekrar")]
		public string PasswordNewConfirm { get; set; } = null!;

	}
}
