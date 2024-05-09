using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
	public class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			//Mail Server : Gmail
			var client = new SmtpClient(host: "smtp.gmail.com", port: 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential(userName:"ahmeedsleem8@gmail.com",password:"bqifghjtaujhfzado");

			client.Send("ahmeedsleem8@gmail.com",email.Recipients,email.Subject,email.Body);
		}
			
	}
}

