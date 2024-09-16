using AutoMapper;
using Fences.DAL.EF;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace Fences.Services.ConcreteServices
{
    public class EmailService : BaseService, IEmailSender, IEmailService
    {
        private readonly SmtpOptions _smtpOptions;

        public EmailService(IOptions<SmtpOptions> smtpOptions, ApplicationDbContext dbContext, IMapper mapper, ILogger<EmailService> logger)
            : base(dbContext, mapper, logger)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                using (var client = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port))
                {
                    client.Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password);
                    client.EnableSsl = _smtpOptions.EnableSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpOptions.Username),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(email);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("Wystąpił problem z wysłaniem wiadomości.", ex);
            }
        }

        public async Task SendEmailAsync(ContactForm contactForm)
        {
            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpOptions.Username),
                    Subject = "Kontakt ze strony Fences.com",
                    Body = $"Treść wiadomości:\n{contactForm.Content}\n\nKontakt zwrotny: {contactForm.Email}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(_smtpOptions.Username);
                mailMessage.ReplyToList.Add(new MailAddress(contactForm.Email));

                using (SmtpClient smtpClient = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password);
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
