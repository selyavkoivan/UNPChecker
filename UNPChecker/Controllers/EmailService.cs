using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using UNPChecker.Models;

public class EmailService
{
    public static void SendEmail(List<UNP> users)
    {
        foreach (var user in users)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("fanfiction", "fanfictionteamof@gmail.com"));
            emailMessage.Subject = "Ваш статус";
            emailMessage.To.Add(new MailboxAddress("", user.Email));

            string text;
            try
            {
                var json = new WebClient().DownloadString(
                    $"http://www.portal.nalog.gov.by/grp/getData?unp={user.UNPCode}&charset=UTF-8&type=json");
                text = "Вы найдены в базе gov by и локальной базе";
            }
            catch (WebException)
            {
                text = "Вы не найдены в базе gov by, но есть локальной базе";
            }
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = text
            };

            var adminEmail = Email.getAdminEmail();
        
            using var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(adminEmail.emailAddress, adminEmail.password);
            client.Send(emailMessage);

            client.Disconnect(true);
        }
    }
}