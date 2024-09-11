using AutoMapper;
using Fences.DAL.EF;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fences.Services.ConcreteServices
{
    public class EmailSenderService : BaseService, IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;
        public EmailSenderService(IOptions<SmtpSettings> smtpSettings, ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpClient = new SmtpClient(_smtpSettings.Host)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.UserName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
