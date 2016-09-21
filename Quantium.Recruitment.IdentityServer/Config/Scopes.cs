using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quantium.Recruitment.IdentityServer.Config
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>()
            {
                new Scope
                {
                    Name = "qrecruitment",
                    DisplayName = "Quantium Recruitment Api",
                    Description = "Allow the client to interact with api services",
                    Type = ScopeType.Resource
                }
            };
        }
    }
}