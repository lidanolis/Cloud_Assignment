﻿// Licensed to the .NET Foundation under one or more agreements.
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
using Cloud_Assignment.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using Cloud_Assignment.Data;
using Cloud_Assignment.Models;
using Cloud_Assignment.Controllers;
namespace Cloud_Assignment.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly Cloud_AssignmentContext _Subscriptioncontext;
        private readonly SignInManager<Cloud_AssignmentUser> _signInManager;
        private readonly UserManager<Cloud_AssignmentUser> _userManager;
        private readonly IUserStore<Cloud_AssignmentUser> _userStore;
        private readonly IUserEmailStore<Cloud_AssignmentUser> _emailStore;
        private readonly ILogger<RegisterModel> _context;
        private readonly IEmailSender _emailSender;
        //private readonly 

        public RegisterModel(
            Cloud_AssignmentContext context,
            UserManager<Cloud_AssignmentUser> userManager,
            IUserStore<Cloud_AssignmentUser> userStore,
            SignInManager<Cloud_AssignmentUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _Subscriptioncontext = context;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _context = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "You must enter the name first before submitting your form!")]
            [StringLength(256, ErrorMessage = "You must enter the value between 6 - 256 chars", MinimumLength = 6)]
            [Display(Name = "User Full Name")] //label
            public string userfullname { get; set; }

            [Required]
            [Display(Name = "User DOB")]
            [DataType(DataType.Date)]
            public DateTime DoB { get; set; }

            [Display(Name = "User Role")] //label
            public string UserRole { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string newsLetter, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.UserFullName = Input.userfullname;
                user.UserDOB = Input.DoB;
                user.EmailConfirmed = true;
                user.UserRole = "user";
                user.EmailConfirmed = true;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);
                string subscriptionResult = "success";
                if (newsLetter == "true")
                {
                    try
                    {
                        var userId = await _userManager.FindByEmailAsync(Input.Email);
                        var SubscriptionEmail = new NewsletterEmail
                        {
                            Email = Input.Email,
                            UserId = userId.Id
                        };

                        _Subscriptioncontext.Add(SubscriptionEmail);
                        await _Subscriptioncontext.SaveChangesAsync();

                        SNSController snsControllerInstance = new SNSController(_Subscriptioncontext, _userManager);
                        subscriptionResult = await snsControllerInstance.doSubscribe(Input.Email);
                    }
                    catch(Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            return BadRequest("error " + ex.Message + "\nInner Exception " + ex.InnerException.Message);
                        }
                        else
                        {
                            return BadRequest("error " + ex.Message);
                        }

                    }
                }

                if (result.Succeeded && subscriptionResult == "success")
                {
                    //_context.LogInformation("User created a new account with password.");

                    //var userId = await _userManager.GetUserIdAsync(user);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        //return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        return RedirectToPage("Login");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private Cloud_AssignmentUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Cloud_AssignmentUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Cloud_AssignmentUser)}'. " +
                    $"Ensure that '{nameof(Cloud_AssignmentUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Cloud_AssignmentUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Cloud_AssignmentUser>)_userStore;
        }
    }
}
