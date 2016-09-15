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

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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


                // odata client code to be moved out
                //var odataClient = new ODataClient("http://localhost:60606/odata/");

                //var activeCandidates = await odataClient
                //    .For<CandidateDto>()
                //    .Filter(b => b.Email == email)
                //    .Select(y => y.IsActive)
                //    .FindEntriesAsync();

                //var candidate = activeCandidates.FirstOrDefault();

                ////Candidate is not in our database
                //if (candidate == null || !candidate.IsActive)
                //{
                //    return RedirectToAction("Login", "Account");
                //}

                //if (candidate.IsActive)
                //{
                //    return RedirectToAction("Test", "CandidateHome");
                //}

                return RedirectToAction("Test", "CandidateHome");
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
                if (result2.Succeeded)
                {
                    result2 = await _userManager.AddLoginAsync(user, info);
                    if (result2.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }

                return RedirectToAction("Account", "Login");
            }
        }
    }
}
