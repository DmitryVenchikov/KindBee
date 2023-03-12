using MailKit.Net.Smtp;
using MimeKit;

namespace SendLib
{
    public class Sender
    {
        public static string From { get; set; }
        public static string To { get; set; }

        public static string FromName { get; set; }
        public static string SMTPHost { get; set; }
        public static int SMTPPort { get; set; }

        public static async Task SendEmailAsync(string email, string subject, string message)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта меда", "venchikovdmitri@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 25, false);
                await client.AuthenticateAsync("venchikovdmitri@mail.ru", "vh23PFDP1zLUet2DBS1R");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}