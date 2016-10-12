using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Extensions;
using AutoMapper;
using Microsoft.OData.Edm;
using Quantium.Recruitment.ApiServices.Models;
using Quantium.Recruitment.Entities;
using System.Web.OData.Builder;
using System.Web.OData.Batch;
namespace Quantium.Recruitment.ApiServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.EnsureInitialized();
        }
    }
}
