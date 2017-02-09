using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Portal.Helpers;
using Quantium.Recruitment.Models;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize]
    public class SurveryController : Controller
    {
        private readonly IHttpHelper _helper;

        public SurveryController(IHttpHelper helper)
        {
            _helper = helper;
        }        

        [HttpGet]

    }
}
