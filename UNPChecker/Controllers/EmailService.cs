using MailKit.Net.Smtp;
using MimeKit;
using UNPChecker.Models;

public class EmailService
{
    public static void SendEmail(List<UNP> users)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("fanfiction", "fanfictionteamof@gmail.com"));
        emailMessage.Subject = "Вы все еще в базе";
        foreach (var u in users)
        {
            emailMessage.To.Add(new MailboxAddress("", u.Email));
        }

        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = "Напоминаем что вы все еще в базе"
        };

        var adminEmail = Email.getAdminEmail();
        
        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 465, true);
        client.Authenticate(adminEmail.emailAddress, adminEmail.password);
        client.Send(emailMessage);

        client.Disconnect(true);
    }
}