using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Simple.OData.Client;

namespace Quantium.Recruitment.Portal.Helpers
{
    public class OdataHelper
    {
        public static ODataClient GetOdataClient()
        {
            var tokenClient = new TokenClient(
                    "https://localhost:44317/identity/connect/token",
                    "qrecruitmentclientid",
                    "myrandomclientsecret");

            var tokenResponse = tokenClient.RequestClientCredentialsAsync("qrecruitment").Result;

            var accessToken = tokenResponse.AccessToken;

            var odataSettings = new ODataClientSettings("http://localhost:60606/odata/");
            odataSettings.BeforeRequest += delegate (HttpRequestMessage request)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            };

            // odata client code to be moved out
            var odataClient = new ODataClient(odataSettings);

            return odataClient;
        }
    }
}
