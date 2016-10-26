using IdentityServer3.AccessTokenValidation;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Quantium.Recruitment.ApiServices
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(
                new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = "https://localhost:44316/identity",
                    RequiredScopes = new [] { "qrecruitment" }
                });

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            
            app.UseWebApi(config);
            config.DependencyResolver = UnityConfig.RegisterComponents();
            
            AutoMapperConfig.RegisterMappings();
        }
    }
}