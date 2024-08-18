using AutoMapper;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Fences.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        
        [HttpGet]
        public async Task<IActionResult> AddOrUpdateJob(int ? id = null)
        {
            var fenceTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Płot", Value = "Płot" },
                new SelectListItem { Text = "Wiata", Value = "Wiata" }
            };
            
            ViewBag.FenceTypes = new SelectList(fenceTypes, "Value", "Text");

            if(id.HasValue)
            {
                var jobVm = await _jobService.GetJobAsync(x => x.Id == id);
                ViewBag.ActionType = "Edit";
                return View(Mapper.Map<AddOrUpdateJobVm>(jobVm));
            }
            ViewBag.ActionType = "Add";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Details (int id)
        {
            var jobVm = await _jobService.GetJobsAsync(x => x.Id == id);
            return View(jobVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateJob(AddOrUpdateJobVm jobVm)
        {
            if (jobVm.Id == null)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return View("Error");
                }

                jobVm.UserId = user.Id;
                jobVm.RegistrationDate = DateTime.Now;
                ModelState.Remove("UserId");
            }

            if (ModelState.IsValid)
            {
                await _jobService.AddOrUpdateJobAsync(jobVm);
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> DeleteJob(JobVm jobVm)
        {
            if (jobVm != null)
            {
                await _jobService.DeleteJobAsync(jobVm);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
