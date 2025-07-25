using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Models
{
	public class RoleCreateViewModel
	{
		[Required(ErrorMessage = "Rol ismi  boş bırakılamaz")]
		[Display(Name = "Rol Adı")]
		public string? Name { get; set; }
	}
}
