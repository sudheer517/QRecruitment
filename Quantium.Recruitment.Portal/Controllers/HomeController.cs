using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODataModels.Quantium.Recruitment.ApiServices.Models;
using Simple.OData.Client;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {

            var yz = getData();

            return View();
        }

        public async Task<IActionResult> getData()
        {
            string odataUri = "http://localhost:60606/odata/";
            var client = new ODataClient(odataUri);

            var packages2 = await client
                .For<CandidateDto>()
                .Filter(b => b.Email == "Pooja.Sharma41@gmail.com").Select(item => item.IsActive)
                .FindEntriesAsync();

            var x = packages2.Count();

            return RedirectToAction("Test", "CandidateHome");

        }
    }
}
