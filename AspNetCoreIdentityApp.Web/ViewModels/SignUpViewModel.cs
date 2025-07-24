using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
	public class SignUpViewModel
	{
	
		[Required(ErrorMessage ="Kullanıcı Ad alanı boş bırakılamaz")]
		[Display(Name ="Kullanıcı Adı")]
		public string UserName { get; set; } = null!;


		[EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi giriniz")]
		[Required(ErrorMessage = "Email alanı boş bırakılamaz")]
		[Display(Name = "Email")]
		public string Email { get; set; } = null!;


		[Display(Name = "Telefon Numarası")]
		public string? Phone { get; set; }


		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
		[Display(Name = "Şifre")]
		[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")] // Optional: Minimum length validation
		public string Password { get; set; } = null!;



		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor. Lütfen tekrar deneyiniz.")]
		[Required(ErrorMessage = "Şifre Tekrar alanı boş bırakılamaz")]
		[Display(Name = "Şifre Tekrar")]
		[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")] // Optional: Minimum length validation

		public string PasswordConfirm { get; set; } = null!;
	}
}
