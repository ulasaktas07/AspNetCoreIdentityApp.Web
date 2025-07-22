using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreIdentityApp.Web.Extensions
{
	public static class ModelStateExtensions
	{
		public static void AddModelErrorList(this ModelStateDictionary modelState, List<string> errors)
		{
			errors.ForEach(error =>
			{
				modelState.AddModelError(string.Empty, error);
			});
		}
	}
}
