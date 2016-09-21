using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quantium.Recruitment.IdentityServer.Config
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "qrecruitmentclientid",
                    ClientName = "Quantium Recruitment client credentials",
                    Flow = Flows.ClientCredentials,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("myrandomclientsecret".Sha256())
                    },
                    AllowAccessToAllScopes = true
                }
            };
        }
    }
}