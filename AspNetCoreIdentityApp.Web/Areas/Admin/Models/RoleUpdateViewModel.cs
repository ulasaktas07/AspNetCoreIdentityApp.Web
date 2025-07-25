using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Models
{
	public class RoleUpdateViewModel
	{
		public string Id { get; set; } = null!;

		[Required(ErrorMessage = "Rol isim  boş bırakılamaz")]
		[Display(Name = "Rol Adı")]
		public string Name { get; set; } = null!;
	}
}
