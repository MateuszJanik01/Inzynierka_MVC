using AutoMapper;
using Fences.Model.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Net.Mail;
using System.Net;
using Fences.Services.Interfaces;

namespace Fences.Web.Controllers
{
    public class ContactFormController : BaseController
    {
        private readonly IEmailSender _emailSender;

        public ContactFormController(IEmailSender emailSender, IStringLocalizer localizer, ILogger logger, IMapper mapper) : base(logger, mapper, localizer) 
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> Send(ContactForm model)
        {
            if (ModelState.IsValid)
            {
                // Przygotuj wiadomość e-mail
                string subject = $"Wiadomość od {model.Name}";
                string message = $"Imię i Nazwisko: {model.Name}<br>Email: {model.Email}<br>Treść: {model.Content}";

                // Wyślij e-mail
                await _emailSender.SendEmailAsync("janikmateusz01@gmail.com", subject, message);

                TempData["SuccessMessage"] = "Wiadomość została wysłana!";
                return RedirectToAction("Index", "Home");
            }

            return View("Error");
        }
    }
}
