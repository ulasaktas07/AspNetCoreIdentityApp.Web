using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Web.TagHelpers
{
	public class UserRoleNamesTagHelper(UserManager<AppUser> userManager):TagHelper
	{
		public string UserId { get; set; } = null!;	
		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			var user = await userManager.FindByIdAsync(UserId);

			var roles = await userManager.GetRolesAsync(user!);

			var stringBuilder = new StringBuilder();

			roles.ToList().ForEach(role =>
			{
				stringBuilder.Append(@$"<span class=""badge text-bg-secondary mx-1"">{role.ToLower()}</span>");
			});

			output.Content.SetHtmlContent(stringBuilder.ToString());

		}
	}
}
