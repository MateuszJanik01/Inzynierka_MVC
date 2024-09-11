using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fences.ViewModels.VM;
using Fences.Model.DataModels;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace Fences.Web.Controllers;

public class HomeController : BaseController
{
    public HomeController(IStringLocalizer localizer, ILogger logger, IMapper mapper) : base(logger, mapper, localizer) {}

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
