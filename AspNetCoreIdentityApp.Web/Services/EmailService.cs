
using AspNetCoreIdentityApp.Web.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Web.Services
{
	public class EmailService(IOptions<EmailSetting> options) : IEmailService
	{

		public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail)
		{
			var smptpClient = new SmtpClient();

			smptpClient.Host = options.Value.Host!;
			smptpClient.DeliveryMethod= SmtpDeliveryMethod.Network;
			smptpClient.UseDefaultCredentials = false;
			smptpClient.Port = 587;
			smptpClient.Credentials = new NetworkCredential(options.Value.Email!, options.Value.Password!);
			smptpClient.EnableSsl = true;

			var mailSetting = new MailMessage();

			mailSetting.From = new MailAddress(options.Value.Email!);
			mailSetting.To.Add(ToEmail);
			mailSetting.Subject = "Şifre Sıfırlama Linki";
			mailSetting.Body = $"Şifre yenileme bağlantınız: <a href='{resetPasswordEmailLink}'>Şifre Yenileme Link</a>";
			mailSetting.IsBodyHtml = true; // HTML içeriği desteklemesi için

			await smptpClient.SendMailAsync(mailSetting);

		}
	}
}
