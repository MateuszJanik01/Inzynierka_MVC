using AutoMapper;
using Fences.DAL.EF;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Fences.Services.ConcreteServices
{
    public class EmailService : BaseService, IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration, ApplicationDbContext dbContext, IMapper mapper, ILogger<EmailService> logger)
            : base(dbContext, mapper, logger)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(ContactForm contactForm)
        {
            var Username = _configuration.GetValue<string>("EmailConfig:UserName");
            var Password = _configuration.GetValue<string>("EmailConfig:Password");
            var Host = _configuration.GetValue<string>("EmailConfig:Host");
            var Port = _configuration.GetValue<int>("EmailConfig:Port");
            var FromEmail = _configuration.GetValue<string>("EmailConfig:FromEmail") ?? throw new ArgumentNullException("FromEmail nie może być null.");

            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(FromEmail),
                    Subject = "Kontakt ze strony Fences.com",
                    Body = $"Treść wiadomości:\n{contactForm.Content}\n\nKontakt zwrotny: {contactForm.Email}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(FromEmail);
                mailMessage.ReplyToList.Add(new MailAddress(contactForm.Email));

                using (SmtpClient smtpClient = new SmtpClient(Host, Port))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(Username, Password);
                    smtpClient.EnableSsl = true;

                    
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("Wystąpił problem z wysłaniem wiadomości.", ex);
            }
        }
    }

}
