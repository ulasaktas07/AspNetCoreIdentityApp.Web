using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.PermissionsRoot;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Seeds
{
	public class PermissionSeed
	{
		public static async Task Seed(RoleManager<AppRole> roleManager)
		{
			var hasBasicRole=await roleManager.RoleExistsAsync("BasicRole");

			var hasAdvanceRole=await roleManager.RoleExistsAsync("AdvanceRole");

			var hasAdminRole=await roleManager.RoleExistsAsync("AdminRole");

			if (!hasBasicRole)
			{
			
				await roleManager.CreateAsync(new AppRole() { Name="BasicRole"});

				var basicRole = (await roleManager.FindByNameAsync("BasicRole"))!;

				await AddReadPermission(basicRole, roleManager);
			}

			if (!hasAdvanceRole)
			{

				await roleManager.CreateAsync(new AppRole() { Name = "AdvanceRole" });

				var advanceRole = (await roleManager.FindByNameAsync("AdvanceRole"))!;

				await AddReadPermission(advanceRole, roleManager);
				await AddUpdateAndCreatePermission(advanceRole, roleManager);
			}

			if (!hasAdminRole)
			{
				await roleManager.CreateAsync(new AppRole() { Name = "AdminRole" });
				var adminRole = (await roleManager.FindByNameAsync("AdminRole"))!;
				await AddReadPermission(adminRole, roleManager);
				await AddUpdateAndCreatePermission(adminRole, roleManager);
				await AddDeletePermission(adminRole, roleManager);
			}

		}

		public static async Task AddReadPermission(AppRole role, RoleManager<AppRole> roleManager)
		{
			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Stock.Read));

			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Order.Read));

			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Catalog.Read));
		}


		public static async Task AddUpdateAndCreatePermission(AppRole role, RoleManager<AppRole> roleManager)
		{
			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Stock.Create));

			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Order.Create));

			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Catalog.Create));

			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Stock.Update));

			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Order.Update));

			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Catalog.Update));

		}


		public static async Task AddDeletePermission(AppRole role, RoleManager<AppRole> roleManager)
		{
			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Stock.Delete));

			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Order.Delete));

			await roleManager.AddClaimAsync(role, new Claim("Permission", PermissionsRoot.Permissions.Catalog.Delete));
		}
	}
}
