using Demo.DAL.Models;
using Demo.PL.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;


namespace Demo.PL.Helper
{
	public   class EmailSettings :ImailSettings
	{
		private MailSettings _options;

		//public static void SendEmail(Email email)
		//{
		//	//using google smtp settings
		//	var client = new SmtpClient("smtp.gmail.com", 587);
		//	client.EnableSsl = true; //Make Email Encrypted
		//	client.Credentials = new NetworkCredential("ziadbahaa41@gmail.com", "rlkcojfutxbghfyl");
		//	client.Send("ziadbahaa41@gmail.com",email.Recipients,email.Subject,email.Body);	

		//}
		public EmailSettings(IOptions<MailSettings> options)
		{
			_options = options.Value;
		}
		public void SendMail(Email email)
		{
			var mail = new MimeMessage
			{
				Sender = MailboxAddress.Parse(_options.Email),
				Subject = email.Subject
			};

			mail.To.Add(MailboxAddress.Parse(email.Recipients));
			mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

			var builder = new BodyBuilder();
			builder.TextBody = email.Body;
			mail.Body = builder.ToMessageBody();

			using var smtp = new SmtpClient();

			smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);

			smtp.Authenticate(_options.Email, _options.Password);
			smtp.Send(mail);

			smtp.Disconnect(true);


		}
	}
}
