using IdentityServer3.Core.Configuration;
using Owin;
using Quantium.Recruitment.IdentityServer.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Quantium.Recruitment.IdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/identity", idsrvApp =>
            {
                var idServerServiceFactory =
                    new IdentityServerServiceFactory()
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get())
                    .UseInMemoryUsers(Users.Get());

                var options = new IdentityServerOptions
                {
                    Factory = idServerServiceFactory,
                    SiteName = "Quantium Recruitment Security Token Service",
                    IssuerUri = "https://QRecruitment/identity",
                    PublicOrigin = "https://localhost:44316",
                    SigningCertificate = LoadCertificate()
                };

                idsrvApp.UseIdentityServer(options);
            });
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2($@"{AppDomain.CurrentDomain.BaseDirectory}\certificates\idsrv3test.pfx", "idsrv3test");
        }
    }
}