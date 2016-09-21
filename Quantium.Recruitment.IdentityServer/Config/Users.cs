using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quantium.Recruitment.IdentityServer.Config
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>()
            {
                new InMemoryUser
                {
                    Username = "Kevin",
                    Password = "secret",
                    Subject = "e0a07388-16f9-4140-8e93-90e97fd1ae47"
                },
                new InMemoryUser
                {
                    Username = "Sven",
                    Password = "secret",
                    Subject = "6b2d9ed6-31c3-4161-a609-28379f093b8f"
                }
            };
        }
    }
}