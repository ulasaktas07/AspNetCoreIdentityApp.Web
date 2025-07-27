using AspNetCoreIdentityApp.Web.ClaimProvider;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.OptionsModels;
using AspNetCoreIdentityApp.Web.PermissionsRoot;
using AspNetCoreIdentityApp.Web.Requirements;
using AspNetCoreIdentityApp.Web.Seeds;
using AspNetCoreIdentityApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
	options.ValidationInterval = TimeSpan.FromMinutes(30);
});

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.AddIdentityWithExt();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();

builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();

builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();

builder.Services.AddAuthorizationBuilder()
	.AddPolicy("RizePolicy", policy =>
	{
		policy.RequireClaim("city", "Rize");
	})
	.AddPolicy("ExchangeExpirePolicy", policy =>
	{
		policy.AddRequirements(new ExchangeExpireRequirement());
	}).AddPolicy("ViolencePolicy", policy =>
	{
		policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 });
	}).AddPolicy("OrderPermissionReadAndDelete", policy =>
	{
		policy.RequireClaim("permission", Permissions.Order.Read);
		policy.RequireClaim("permission", Permissions.Order.Delete);
		policy.RequireClaim("permission", Permissions.Stock.Delete);

	})
	.AddPolicy("Permissions.Order.Read", policy =>
	{
		policy.RequireClaim("permission", Permissions.Order.Read);

	}).AddPolicy("Permissions.Order.Delete", policy =>
	{
		policy.RequireClaim("permission", Permissions.Order.Delete);
	}).AddPolicy("Permissions.Stock.Delete", policy =>
	{
	
		policy.RequireClaim("permission", Permissions.Stock.Delete);
	});

builder.Services.ConfigureApplicationCookie(opt =>
{
	var cookieBuilder = new CookieBuilder
	{
		Name = "AppCookie"
	};
	opt.LoginPath = new PathString("/Home/SignIn");
	opt.LogoutPath = new PathString("/Member/Logout");
	opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
	opt.Cookie = cookieBuilder;
	opt.ExpireTimeSpan = TimeSpan.FromDays(60);
	opt.SlidingExpiration = true;

});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

	await PermissionSeed.Seed(roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();


app.MapControllerRoute(
	  name: "areas",
	  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();


app.Run();
