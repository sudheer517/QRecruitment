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
            return Ok("You are no longer Active dude");
        }

        public IActionResult NotRegistered()
        {
            return Ok("You are not registered in our system. Please contact admin@quantium.com for assistance UserCreationError");
        }

        public IActionResult UserCreationError()
        {
            return Ok("Error occurred during user creation. Please contact admin@quantium.com for assistance");
        }
    }
}
