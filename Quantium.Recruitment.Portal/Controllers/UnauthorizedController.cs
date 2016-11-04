using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Portal.Controllers
{
    public class UnauthorizedController: Controller
    {
        public IActionResult NotActive()
        {
            ViewBag.UserMessage = "You are no longer active in our system";
            return View("Restricted");
        }

        public IActionResult NotRegistered()
        {
            ViewBag.UserMessage = "You are not registered in our system";
            return View("Restricted");
        }

        public IActionResult UserCreationError()
        {
            ViewBag.UserMessage = "Error occurred during user creation";
            return View("Restricted");
        }

        public IActionResult DuplicateUserError()
        {
            ViewBag.UserMessage = "A user with the given email from another social login provider already exits in our system";
            return View("Restricted");
        }
    }
}
