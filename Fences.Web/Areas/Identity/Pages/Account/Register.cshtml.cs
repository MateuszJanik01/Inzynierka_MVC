// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Fences.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Fences.DAL.EF;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using NuGet.DependencyResolver;
using Fences.ViewModels.VM;

namespace Fences.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public RegisterModel(UserManager<User> userManager, ILogger logger, IMapper mapper, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [BindProperty]
        public RegisterVm Input { get; set; } = default!;

        [TempData]
        public string StatusMessage { get; set; } = default!;
        // W przysz³oœci mo¿a usun¹æ jak nie bêddzie potrzebne
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var tupleUserRole = CreateUserBasedOnRole(Input);
                var result = await _userManager.CreateAsync(tupleUserRole.Item1, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    result = await _userManager.AddToRoleAsync(tupleUserRole.Item1, tupleUserRole.Item2.Name);
                    if (result.Succeeded)
                    {
                        OnGet();
                        StatusMessage = "User created";
                        return RedirectToPage();
                    }
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            OnGet();
            return RedirectToPage();
        }

        private Tuple<User, Role> CreateUserBasedOnRole(RegisterVm inputModel)
        {
            var role = _dbContext.Roles.FirstOrDefault(x => x.RoleValue == RoleValue.User);
            return new Tuple<User, Role>(_mapper.Map<User>(inputModel), role);
        }
    }
}
