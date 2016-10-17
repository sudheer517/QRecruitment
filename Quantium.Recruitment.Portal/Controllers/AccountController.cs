using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Portal.Models;
using Quantium.Recruitment.Portal.Models.AccountViewModels;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using IdentityModel.Client;
using System.Net.Http.Headers;
using Quantium.Recruitment.Portal.Helpers;
using System.Collections.Generic;

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;
        private readonly ILogger _logger;
        private readonly ICandidateHelper _candidateHelper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<QRecruitmentRole> roleManager,
            ILoggerFactory loggerFactory,
            ICandidateHelper candidateHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _candidateHelper = candidateHelper;
        }

        // GET: /Account/IsUserAuthenticated
        [HttpGet]
        public bool IsUserAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
           return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                //}
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View("_RegisterPartial");
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, $"User created a new account with email:{model.Email}");
                    return RedirectToLocal("/Candidate/Test#/test");
                }
                AddErrors(result);
            }
            ViewData["activeAnchor"] = "registerView";
            return View("Login", model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, $"User with name:{User.Identity.Name} logged out");
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null)
        {
            var externaluser = await HttpContext.Authentication.AuthenticateAsync("Identity.External");
            if (externaluser != null)
            {
                await HttpContext.Authentication.SignOutAsync("Identity.External");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info != null)
            {
                return RedirectToAction("ExternalLoginCallback");
            }

            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (IsUserAuthenticated())
            {
                var b = "xyz";
            }
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (result.Succeeded) // social login registered user
            {
                //request identity service token
                // token client code
                var user = _userManager.FindByEmailAsync(email).Result;
                IList<string> roles = _userManager.GetRolesAsync(user).Result;

                if (roles.FirstOrDefault() == "Candidate")
                {
                    var isCandidateActive = _candidateHelper.CheckIfCandidateExistsAndActive(email);
                    if (!isCandidateActive)
                    {
                        return RedirectToAction("NotActive", "Unauthorized");
                    }
                    return RedirectToAction("Test", "Candidate");
                }
                else if (roles.FirstOrDefault() == "SuperAdmin")
                {
                    var isAdminActive = _candidateHelper.IsAdminActive(email);
                    if (!isAdminActive)
                    {
                        return RedirectToAction("NotActive", "Unauthorized");
                    }
                    return RedirectToAction("Index", "Home");
                }


                //return Redirect($"{Url.RouteUrl(new { controller = "Candidate", action = "Test" })}#/test");

            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                info.Principal.FindFirstValue(ClaimTypes.Email);
                
                string roleName = _candidateHelper.GetRoleForEmail(email);

                if (string.IsNullOrEmpty(roleName))
                {
                   return RedirectToAction("NotRegistered", "Unauthorized");
                }

                var user = new ApplicationUser { UserName = email, Email = email };
                var createUserTaskResult = _userManager.CreateAsync(user).Result;
                IdentityResult roleCreationResult = null;


                if (!_roleManager.RoleExistsAsync(roleName).Result)
                {
                    roleCreationResult = _roleManager.CreateAsync(new QRecruitmentRole(roleName)).Result;
                }

                if (createUserTaskResult.Succeeded)
                {
                    var createLoginTaskResult = _userManager.AddLoginAsync(user, info).Result;
                    var addUserToRoleTaskResult = _userManager.AddToRoleAsync(user, roleName).Result;
                    if (createLoginTaskResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        if (roleName == "SuperAdmin")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        if (roleName == "Candidate")
                        {
                            return RedirectToAction("Test", "Candidate");
                        }
                    }
                }

                return RedirectToAction("UserCreationError", "Unauthorized");
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
