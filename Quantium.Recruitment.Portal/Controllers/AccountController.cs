using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Portal.Models;
using Quantium.Recruitment.Portal.Models.AccountViewModels;
using System;
using Simple.OData.Client;
using ODataModels.Quantium.Recruitment.ApiServices.Models;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using IdentityModel.Client;
using System.Net.Http.Headers;

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<MyIdentityRole> _roleManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<MyIdentityRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpGet]
        [AllowAnonymous]
        public string IsUserAuthenticated()
        {
            return User.Identity.IsAuthenticated.ToString();
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
           return View();
        }

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
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal("/Candidate/Test#/test");
                }
                AddErrors(result);
            }
            ViewData["activeAnchor"] = "registerView";
            // If we got this far, something failed, redisplay form
            return View("Login", model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
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
            if (result.Succeeded)
            {
                //request identity service token

                var tokenClient = new TokenClient(
                    "https://localhost:44317/identity/connect/token",
                    "qrecruitmentclientid",
                    "myrandomclientsecret");

                var tokenResponse = tokenClient.RequestClientCredentialsAsync("qrecruitment").Result;

                var accessToken = tokenResponse.AccessToken;

                var odataSettings = new ODataClientSettings("http://localhost:60606/odata/");
                odataSettings.BeforeRequest += delegate (HttpRequestMessage request)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                };
                
                // odata client code to be moved out
                var odataClient = new ODataClient(odataSettings);

                
                var activeCandidates = await odataClient
                    .For<CandidateDto>()
                    .Filter(b => b.Email == email)
                    .Select(y => y.IsActive)
                    .FindEntriesAsync();

                var candidate = activeCandidates.FirstOrDefault();

                ////Candidate is not in our database
                //if (candidate == null || !candidate.IsActive)
                //{
                //    return RedirectToAction("Login", "Account");
                //}

                //if (candidate.IsActive)
                //{
                //    return RedirectToAction("Test", "Candidate");
                //}

                return RedirectToAction("Test", "Candidate");
                //return Redirect($"{Url.RouteUrl(new { controller = "Candidate", action = "Test" })}#/test");
                // Do a canddate email check with the email
                //if (true)
                //{
                //    return RedirectToAction("Index", "Home");
                //}

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
                var user = new ApplicationUser { UserName = email, Email = email };
                var result2 = await _userManager.CreateAsync(user);
                string role = string.Empty;

                if (email == "rkshrohan@gmail.com" && !_roleManager.RoleExistsAsync("SuperAdmin").Result)
                {
                    MyIdentityRole newRole = new MyIdentityRole();
                    newRole.Name = "SuperAdmin";
                    var result3 = await _roleManager.CreateAsync(newRole);
                    role = newRole.Name;
                }

                if (email == "0firefist0@gmail.com" && !_roleManager.RoleExistsAsync("Candidate").Result)
                {
                    MyIdentityRole newRole = new MyIdentityRole();
                    newRole.Name = "Candidate";
                    var result3 = await _roleManager.CreateAsync(newRole);
                    role = newRole.Name;
                }

                if (result2.Succeeded)
                {
                    result2 = _userManager.AddLoginAsync(user, info).Result;
                    await _userManager.AddToRoleAsync(user, role);
                    if (result2.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }

                return RedirectToAction("Account", "Login");
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
