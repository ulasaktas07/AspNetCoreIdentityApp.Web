using AspNetCoreIdentityApp.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
	public class UserEditViewModel
	{
		[Required(ErrorMessage = "Kullanıcı Ad alanı boş bırakılamaz")]
		[Display(Name = "Kullanıcı Adı")]
		public string UserName { get; set; } = null!;


		[EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi giriniz")]
		[Required(ErrorMessage = "Email alanı boş bırakılamaz")]
		[Display(Name = "Email")]
		public string Email { get; set; } = null!;


		[Display(Name = "Telefon Numarası")]
		public string? Phone { get; set; }

		[DataType(DataType.DateTime)]
		[Display(Name = "Doğum Tarihi")]
		public DateTime? BirthDate { get; set; }

		[Display(Name = "Şehir")]
		public string? City { get; set; }

		[Display(Name = "Profil Resmi")]
		public IFormFile? Picture { get; set; }

		[Display(Name = "Cinsiyet")]
		public Gender? Gender { get; set; }

	}
}
