using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.ClaimProvider
{
	public class UserClaimProvider(UserManager<AppUser> userManager) : IClaimsTransformation
	{
		public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
		{
			var identityUser = principal.Identity as ClaimsIdentity;

			var currentUser = await userManager.FindByNameAsync(identityUser!.Name!);

			if (String.IsNullOrEmpty(currentUser!.City))
			{
				return principal;

			}

			if (principal.HasClaim(x => x.Type != "city"))
			{
				Claim cityClaim = new Claim("city", currentUser.City);
				identityUser.AddClaim(cityClaim);
			}
			return principal;

		}
	}
}
