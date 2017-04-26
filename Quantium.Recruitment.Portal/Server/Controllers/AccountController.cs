using System.Linq;
using System.Threading.Tasks;
using AspNetCoreSpa.Server.Entities;
using AspNetCoreSpa.Server.Extensions;
using AspNetCoreSpa.Server.Services.Abstract;
using AspNetCoreSpa.Server.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Quantium.Recruitment.Portal.Server.Entities;
using System.Security.Claims;
using System.Collections.Generic;
using Quantium.Recruitment.Portal.Server.Helpers;
using System;
using Newtonsoft.Json;
using Portal.Server.ViewModels.AccountViewModels;
using System.Threading;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AspNetCoreSpa.Server.Controllers.api
{
    [Authorize]
    //[Route("[controller]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<QRecruitmentUser> _userManager;
        private readonly SignInManager<QRecruitmentUser> _signInManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;

        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly IAccountHelper _accountHelper;
        private readonly IHostingEnvironment _env;

        public AccountController(
            UserManager<QRecruitmentUser> userManager,
            SignInManager<QRecruitmentUser> signInManager,
            RoleManager<QRecruitmentRole> roleManager,
            IAccountHelper accountHelper,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,
            IHostingEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountHelper = accountHelper;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _env = env;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRole()
        {
            var result = await _userManager.GetRolesAsync(this.GetCurrentUserAsync().Result);
            return Ok(JsonConvert.SerializeObject(result.First()));
            //var roleResult = _roleManager.GetRoleNameAsync(userResult);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Unauthorized()
        {
            ViewBag.UserMessage = "You are not authorized to access this page";
            return View("Restricted");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GoAway()
        {
            return Ok("Go away mate");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            //return RedirectToAction("Index", "Home");
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var a = "xyz";
            }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (_accountHelper.IsAccountActive(model.Email))
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToAction("Index", "Home");
                }
                else
                    return RedirectToAction("Unauthorized");

                //return AppUtils.SignIn(user, roles);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { RememberMe = model.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(2, "User account locked out.");
                //return BadRequest("Lockout");
                ModelState.AddModelError(string.Empty, "User is locked out");
                return View(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Username or password is incorrect");
                //return BadRequest(ModelState.GetModelErrors());
                return View(model);
            }

        }


        [HttpPost("logout")]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            //_logger.LogInformation(4, "User logged out.");
            //return Ok();
            _logger.LogInformation(4, $"User with name:{User.Identity.Name} logged out");
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            //_logger.LogInformation(4, "User logged out.");
            //return Ok();
            _logger.LogInformation(4, $"User with name:{User.Identity.Name} logged out");
            return RedirectToLocal("/");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            //var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            //var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            //return Challenge(properties, provider);

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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return Render(ExternalLoginStatus.Error);
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return Render(ExternalLoginStatus.Invalid);
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (result.Succeeded)
            {
                var user = _userManager.FindByEmailAsync(email).Result;
                IList<string> roles = _userManager.GetRolesAsync(user).Result;

                if (_accountHelper.IsAccountActive(email))
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Unauthorized");
                
            }
            if (result.RequiresTwoFactor)
            {
                return Render(ExternalLoginStatus.TwoFactor);
            }
            if (result.IsLockedOut)
            {
                return Render(ExternalLoginStatus.Lockout);
            }
            else
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                info.Principal.FindFirstValue(ClaimTypes.Email);

                string roleName = _accountHelper.GetRoleForEmail(email);

                if (string.IsNullOrEmpty(roleName))
                {
                    return RedirectToAction("NotRegistered");
                }

                var user = new QRecruitmentUser { UserName = email, Email = email, CreatedDate = DateTime.Now };
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
                        return RedirectToAction("Index", "Home");
                    }
                }

                if (createUserTaskResult.Errors.First().Code == "DuplicateUserName")
                {
                    return RedirectToAction("DuplicateUserError");
                }


                return RedirectToAction("UserCreationError");
            }
            //else
            //{
            //    // If the user does not have an account, then ask the user to create an account.
            //    // ViewData["ReturnUrl"] = returnUrl;
            //    // ViewData["LoginProvider"] = info.LoginProvider;
            //    // var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            //    // return RedirectToAction("Index", "Home", new ExternalLoginCreateAccountViewModel { Email = email });
            //    return Render(ExternalLoginStatus.CreateAccount);
            //}

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult NotRegistered()
        {
            ViewBag.UserMessage = "You are not registered in our system";
            return View("Restricted");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserCreationError()
        {
            //ViewBag.UserMessage = "Error occurred during user creation";
            return Ok("Error occurred during user creation");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult DuplicateUserError()
        {
            //ViewBag.UserMessage = "A user with the given email from another social login provider already exits in our system";
            return Ok("A user with the given email from another social login provider already exits in our system. Please use that social login to continue");

        }
        [HttpPost("ExternalLoginCreateAccount")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginCreateAccount([FromBody]ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest("External login information cannot be accessed, try again.");
            }
            var user = new QRecruitmentUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                    return Ok(); // Everything ok
                }
            }
            return BadRequest(new[] { "Email already exists" });
        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var currentUser = await _userManager.FindByNameAsync(model.Email);

            if (currentUser != null && currentUser.Email != "user@user.com")
            {
                var Content = System.Net.WebUtility.HtmlDecode(System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates\\ResetPasswordEmailTemplate.html")));
                var password = AccountHelper.GenerateRandomString();               
                Dictionary<string, string> parameters = new Dictionary<string, string>();                
                parameters.Add("Password", password);              
                foreach (var param in parameters)
                {
                    Content = Content.Replace("<" + param.Key + ">", param.Value);
                }
                var emailTask = _emailSender.SendEmailAsync(new EmailModel
                {
                    To = model.Email,
                    From = Startup.Configuration["RecruitmentAdminEmail"],
                    DisplayName = "Quantium Recruitment",
                    Subject = "Password reset",
                    HtmlBody = Content
                });

                await Task.Run(() => emailTask);

                ViewData["Message"] = "Please check your email to reset your password.";
            }
            else
            {
                ViewData["Message"] = "User does not exist or signed in using social login";
            }
            //if (currentUser == null || !(await _userManager.IsEmailConfirmedAsync(currentUser)))
            //{
            //    // Don't reveal that the user does not exist or is not confirmed
            //    return View("ForgotPasswordConfirmation");
            //}
            //// For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            //// Send an email with this link
            //var code = await _userManager.GeneratePasswordResetTokenAsync(currentUser);

            //var host = Request.Scheme + "://" + Request.Host;
            //var callbackUrl = host + "?userId=" + currentUser.Id + "&passwordResetCode=" + code;
            //var confirmationLink = "<a class='btn-primary' href=\"" + callbackUrl + "\">Reset your password</a>";
            ////var emailTask =_emailSender.SendEmailAsync(new EmailModel { To = model.Email });
            return View("ForgotPasswordConfirmation");
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Ok("Reset confirmed");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok("Reset confirmed"); ;
            }
            AddErrors(result);
            return BadRequest(ModelState.GetModelErrors());
        }

        [HttpGet("SendCode")]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return BadRequest("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost("SendCode")]
        [AllowAnonymous]
        public async Task<IActionResult> SendCode([FromBody]SendCodeViewModel model)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return BadRequest("Error");
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest("Error");
            }

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                await _emailSender.SendEmailAsync(MailType.SecurityCode, new EmailModel { }, null);
                //await _emailSender.SendEmailAsync(Email, await _userManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (model.SelectedProvider == "Phone")
            {
                await _smsSender.SendSmsTwillioAsync(await _userManager.GetPhoneNumberAsync(user), message);
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [HttpGet("VerifyCode")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return BadRequest("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost("VerifyCode")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
            }
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<QRecruitmentUser> GetCurrentUserAsync()
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
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private IActionResult Render(ExternalLoginStatus status)
        {
            return RedirectToAction("Index", "Home", new { externalLoginStatus = (int)status });
        }

        #endregion
    }
}
