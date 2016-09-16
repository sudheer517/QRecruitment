using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODataModels.Quantium.Recruitment.ApiServices.Models;
using Simple.OData.Client;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
