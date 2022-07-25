using Core.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace Core.Services;

public class GmailEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public GmailEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendEmailAsync(string email, string subject, string message)
    {
       try
       {
           var emailMessage = new MimeMessage();
           emailMessage.From.Add(MailboxAddress.Parse(_configuration["Email:Login"]));
           emailMessage.To.Add(MailboxAddress.Parse(email));
           emailMessage.Subject = subject;
           emailMessage.Body = new TextPart(TextFormat.Plain) {Text = message};

           using var smtp = new SmtpClient();
           await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
           await smtp.AuthenticateAsync(_configuration["Email:Login"], _configuration["Email:Password"]);
           await smtp.SendAsync(emailMessage);
           await smtp.DisconnectAsync(true);

           return true;
       }
       catch
       {
           return false;
       }
    }
}