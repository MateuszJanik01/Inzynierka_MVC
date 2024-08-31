using AutoMapper;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Fences.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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


            
            var jobs = await _jobService.GetJobsAsync();
            var occupiedDates = jobs.Select(job => job.DateOfExecution.ToString("yyyy-MM-dd")).ToList();
            var disabledDates = string.Join(",", occupiedDates);

            ViewBag.DisabledDates = disabledDates;



            if (id.HasValue)
            {
                var jobVm = await _jobService.GetJobAsync(x => x.Id == id);
                ViewBag.ActionType = "Edit";
                return View(Mapper.Map<AddOrUpdateJobVm>(jobVm));
            }
            ViewBag.ActionType = "Add";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details (int id)
        {
            var job = await _jobService.GetJobAsync(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            string fullAddress = $"{job.Street} {job.Number}, {job.Town}, {job.ZipCode}";
            string apiKey = "AIzaSyDsQzCFukD06XdkwG3--IsJRV3XGvkTajA";
            ViewBag.GoogleMapsUrl = $"https://www.google.com/maps/embed/v1/place?key={apiKey}&q={Uri.EscapeDataString(fullAddress)}";

            return View(job);

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
