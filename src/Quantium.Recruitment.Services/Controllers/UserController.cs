using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Infrastructure.Repositories;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Services.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/values
        [HttpGet]
        public IList<string> GetTemp()
        {
            return new List<string> { "hola" };
        }

    }
}
