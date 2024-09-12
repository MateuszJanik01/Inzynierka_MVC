using AutoMapper;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Fences.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

public class ContactFormController : BaseController
{
    private readonly IEmailService _emailService;

    public ContactFormController(IStringLocalizer localizer, ILogger logger, IMapper mapper, IEmailService emailService)
        : base(logger, mapper, localizer)
    {
        _emailService = emailService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendEmail(ContactForm contactForm)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Home/Index.cshtml", contactForm);
        }

        try
        {
            await _emailService.SendEmailAsync(contactForm);
            TempData["SuccessMessage"] = "Wiadomość została pomyślnie wysłana.";
            //return View("~/Views/Home/Index.cshtml");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            TempData["ErrorMessage"] = "Wystąpił problem z wysłaniem wiadomości. Spróbuj ponownie później.";
        }

        return View("~/Views/Home/Index.cshtml", contactForm);
    }
}
