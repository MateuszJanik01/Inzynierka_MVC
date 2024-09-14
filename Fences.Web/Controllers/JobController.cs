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
    [Authorize(Roles = "Admin, User")]

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
        public async Task<IActionResult> AddJob()
        {
            var fenceTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Ogrodzenie Betonowe", Value = "Ogrodzenie Betonowe" },
                new SelectListItem { Text = "Ogrodzenie Panelowe", Value = "Ogrodzenie Panelowe" },
                new SelectListItem { Text = "Wiata", Value = "Wiata" }
            };

            ViewBag.FenceTypes = new SelectList(fenceTypes, "Value", "Text");


            var jobs = await _jobService.GetJobsAsync();
            var occupiedDates = jobs.Select(job => job.DateOfExecution.ToString("yyyy-MM-dd")).ToList();
            var disabledDates = string.Join(",", occupiedDates);

            ViewBag.DisabledDates = disabledDates;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddJob(AddJobVm addJobVm)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Error");
            }

            addJobVm.UserId = user.Id;
            addJobVm.RegistrationDate = DateTime.Now;
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                await _jobService.AddJobAsync(addJobVm);
                return RedirectToAction("Index");
            }
            else
                return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateJob(int id)
        {
            var fenceTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Ogrodzenie Betonowe", Value = "Ogrodzenie Betonowe" },
                new SelectListItem { Text = "Ogrodzenie Panelowe", Value = "Ogrodzenie Panelowe" },
                new SelectListItem { Text = "Wiata", Value = "Wiata" }
            };

            ViewBag.FenceTypes = new SelectList(fenceTypes, "Value", "Text");


            var jobs = await _jobService.GetJobsAsync();
            var occupiedDates = jobs.Select(job => job.DateOfExecution.ToString("yyyy-MM-dd")).ToList();
            var disabledDates = string.Join(",", occupiedDates);

            ViewBag.DisabledDates = disabledDates;


            var jobVm = await _jobService.GetJobAsync(x => x.Id == id);
            return View(Mapper.Map<UpdateJobVm>(jobVm));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateJob(UpdateJobVm updateJobVm)
        {
            if (ModelState.IsValid)
            {
                await _jobService.UpdateJobAsync(updateJobVm);
                return RedirectToAction("Index");
            } else
                return View("Error");
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

        public async Task<IActionResult> Delete(JobVm jobVm)
        {
            if (jobVm != null)
            {
                await _jobService.DeleteJobAsync(jobVm);
                return RedirectToAction("Index");
            }
            else
                return View("Error");
        }
    }
}
