// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
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
using Fences.ViewModels.VM;

namespace Fences.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager, ILogger logger, IMapper mapper, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [BindProperty]
        public RegisterVm Input { get; set; } = default!;

        [TempData]
        public string StatusMessage { get; set; } = default!;
        
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync()
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
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
                        await _signInManager.SignInAsync(tupleUserRole.Item1, isPersistent: false);
                        StatusMessage = "User created and logged in.";
                        return RedirectToPage("/Index");
                    }
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        public IActionResult OnPostExternalLogin(string provider)
        {
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return Page();
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");
                return Page();
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }
            
            // If the user does not have an account, then create one
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
            var user = new User
            {
                UserName = email,
                Email = email,
                FirstName = firstName ?? "Unknown",
                LastName = lastName ?? "Unknown"
            };
            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                createResult = await _userManager.AddLoginAsync(user, info);
                if (createResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                    return LocalRedirect(returnUrl ?? Url.Content("~/"));
                }
            }
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        private Tuple<User, Role> CreateUserBasedOnRole(RegisterVm inputModel)
        {
            var role = _dbContext.Roles.FirstOrDefault(x => x.RoleValue == RoleValue.User);
            return new Tuple<User, Role>(_mapper.Map<User>(inputModel), role);
        }
    }
}
