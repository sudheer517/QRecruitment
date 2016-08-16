using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Infrastructure;
using Quantium.Recruitment.Infrastructure.Repositories;
using Quantium.Recruitment.Services.Models;
using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Services.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private ITestRepository _testRepository;

        public TestController(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        // GET: api/values
        [HttpGet]
        public IList<string> GetTemp()
        {
            return new List<string> { "hola" };
        }

    }
}
