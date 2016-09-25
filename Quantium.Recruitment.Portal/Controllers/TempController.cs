using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Portal.Models;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    public class TempController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;

        public TempController(UserManager<ApplicationUser> userManager, RoleManager<QRecruitmentRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var user = this.GetCurrentUserAsync();
            var temp2 = this.User;
            return Json("tempYo");
        }

        public IActionResult Provoke()
        {
            var user = this.GetCurrentUserAsync();
            var temp2 = this.User;
            return Json("tempYo");
        }

        public IActionResult GetUserRole()
        {
            return Json(this.User.Claims.LastOrDefault().Value);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
