using Fences.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fences.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(ContactForm contactForm);
    }
}
