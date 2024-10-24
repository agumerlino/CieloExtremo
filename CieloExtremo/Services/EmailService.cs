using CieloExtremo.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public interface IEmailService
{
    Task SendEmailAsync(string subject, string body);
    Task SendEmailToSellerAsync(string sellerEmail, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly string _adminEmail; // La dirección de correo a la que se enviarán los mensajes

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
        _adminEmail = "cieloextremoarg@hotmail.com"; // Dirección fija para recibir los correos
    }

    public async Task SendEmailAsync(string subject, string body)
    {
        var fromAddress = new MailAddress(_smtpSettings.UserName, "NoReply");
        var toAddress = new MailAddress(_adminEmail); // Dirección fija de destino
        var smtpClient = new SmtpClient
        {
            Host = _smtpSettings.Host,
            Port = _smtpSettings.Port,
            EnableSsl = _smtpSettings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password)
        };

        var mailMessage = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
    public async Task SendEmailToSellerAsync(string sellerEmail, string subject, string body)
    {
        var fromAddress = new MailAddress(_smtpSettings.UserName, "Venta CIELOEXTREMO");
        var toAddress = new MailAddress(sellerEmail); // Email del vendedor
        var smtpClient = new SmtpClient
        {
            Host = _smtpSettings.Host,
            Port = _smtpSettings.Port,
            EnableSsl = _smtpSettings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password)
        };

        var mailMessage = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
}