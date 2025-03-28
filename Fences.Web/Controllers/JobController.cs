﻿using AutoMapper;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Fences.ViewModels.VM;
using Ganss.Xss;
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
        private readonly IEmailService _emailService;

        public JobController(IJobService jobService, UserManager<User> userManager, IEmailService emailService, IStringLocalizer localizer, ILogger logger, IMapper mapper) : base(logger, mapper, localizer)
        {
            _jobService = jobService;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(string? searchInput = null)
        {
            var sanitizer = new HtmlSanitizer();
            if (!string.IsNullOrWhiteSpace(searchInput))
            {
                searchInput = sanitizer.Sanitize(searchInput);
            }
            ViewBag.searchInput = searchInput;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("Error");
            }
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                if (!string.IsNullOrWhiteSpace(searchInput))
                {
                    return View(await _jobService.GetJobsAsync(x =>
                        x.Description!.Contains(searchInput) ||
                        x.Town.Contains(searchInput) ||
                        x.JobType.Contains(searchInput) ||
                        x.User.FirstName.Contains(searchInput) ||
                        x.User.LastName.Contains(searchInput) ||
                        x.User.Email!.Contains(searchInput) ||
                        x.User.PhoneNumber!.Contains(searchInput)
                    ));
                } else
                    return View(await _jobService.GetJobsAsync());
            }
            else if (await _userManager.IsInRoleAsync(user, "User"))
            {
                if (!string.IsNullOrWhiteSpace(searchInput))
                {
                    return View(await _jobService.GetJobsAsync(x => x.UserId == user.Id && 
                    (
                        x.Description!.Contains(searchInput) ||
                        x.Town.Contains(searchInput) ||
                        x.JobType.Contains(searchInput)
                    )
                    ));
                } else
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
                if (updateJobVm.TotalPrice == 0.00)
                    updateJobVm.TotalPrice = Math.Round((updateJobVm.TotalLength / 2.15) * updateJobVm.Height * 150);
                await _jobService.UpdateJobAsync(updateJobVm);

                // Sending E-mail on some Changes
                /*
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    Logger.LogError("Error with getting User");
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(user, "User"))
                    {
                        ContactForm contactForm = new ContactForm();
                        contactForm.Name = user.FirstName + " " + user.LastName + " Id : " + user.Id;
                        contactForm.Email = user.Email ?? "Brak adresu E-mail";
                        contactForm.Content = $"This user updated job. JobId = {updateJobVm.Id} Please check your schedule";

                        try
                        {
                            await _emailService.SendEmailAsync(contactForm);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, ex.Message);
                        }
                    }
                    else
                    {
                        Logger.LogError("Error with getting UserRole, JobController, UpdateJob");
                    }
                }
                */
                //

                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var job = await _jobService.GetJobAsync(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            string fullAddress = $"{job.Street} {job.Number}, {job.Town}, {job.ZipCode}";
            string apiKey = "AIzaSyBtuw_WLOy7zHaXt192nkx9PtokcQIRWMk";
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