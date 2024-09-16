using Fences.Model.DataModels;

namespace Fences.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(ContactForm contactForm);
    }
}
