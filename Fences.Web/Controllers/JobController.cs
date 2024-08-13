using AutoMapper;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Fences.Web.Controllers
{
    [Authorize (Roles = "Admin, User")]
    public class JobController : BaseController 
    {
        private readonly IJobService _jobService;
        private readonly UserManager<User> _userManager;

        public JobController(IJobService jobService, UserManager<User> userManager, IStringLocalizer localizer, ILogger logger, IMapper mapper) : base(logger, mapper, localizer)
        {
            _jobService = jobService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("Error");
            }
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return View(await _jobService.GetJobsAsync());
            }
            else if (await _userManager.IsInRoleAsync(user, "User"))
            {
                return View(await _jobService.GetJobsAsync(x => x.UserId == user.Id));
            }
            else
            {
                return View("Error");
            }
        }
    }
}
