using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Mail
{
	public class EmailService : IEmailService
	{
		public EmailSettings EmailSettings { get; }
		public ILogger<EmailService> Logger { get; }

		public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
		{
			EmailSettings = emailSettings.Value;
			Logger = logger;
		}

		public async Task<bool> SendEmail(Email Email)
		{
			SendGridClient client = new SendGridClient(EmailSettings.APIKey);
			string subject = Email.Subject;
			EmailAddress to = new EmailAddress(Email.To);
			EmailAddress from = new EmailAddress
			{
				Email = EmailSettings.FromAddress,
				Name = EmailSettings.FromName
			};
			string emailBody = Email.Body;
			SendGridMessage mailObject = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
			Response response = await client.SendEmailAsync(mailObject);
			

			if (response.StatusCode == HttpStatusCode.Accepted || 
				response.StatusCode == HttpStatusCode.OK)
			{
				Logger.LogInformation("Email Sent.");
				return true;
			}
				

			Logger.LogError("Email sending failed.");

			return false;

		}
	}
}
