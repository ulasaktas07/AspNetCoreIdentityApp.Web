using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCoreIdentityApp.Web.TagHelpers
{
	public class UserPictureThumbnailTagHelper:TagHelper
	{
		public string? PictureUrl { get; set; }
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "img"; // Replaces the current tag with an <img> tag

			if (string.IsNullOrEmpty(PictureUrl))
			{
				output.Attributes.SetAttribute("src", "/userspicture/default_user_picture.jpg");
				output.Attributes.SetAttribute("height", 200);
				output.Attributes.SetAttribute("width", 200);

			}
			else
			{
				output.Attributes.SetAttribute("src", $"/userspicture/{PictureUrl}"); // Default image if PictureUrl is not set
				output.Attributes.SetAttribute("height", 200);
				output.Attributes.SetAttribute("width", 200);
			}

		}
	}
}
