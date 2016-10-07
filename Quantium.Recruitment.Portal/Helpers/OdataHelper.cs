using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Simple.OData.Client;
using Microsoft.Extensions.Options;

namespace Quantium.Recruitment.Portal.Helpers
{
    public interface IOdataHelper
    {
        ODataClient GetOdataClient();
    }

    public class OdataHelper : IOdataHelper
    {
        private readonly IOptions<ConfigurationOptions> _configOptions;

        public OdataHelper(IOptions<ConfigurationOptions> configOptions)
        {
            _configOptions = configOptions;
        }

        public ODataClient GetOdataClient()
        {
            var tokenClient = new TokenClient(
                    _configOptions.Value.IdentityServer + "/identity/connect/token",
                    "qrecruitmentclientid",
                    "myrandomclientsecret");

            
            var tokenResponse = tokenClient.RequestClientCredentialsAsync("qrecruitment").Result;

            var accessToken = tokenResponse.AccessToken;

            var odataSettings = new ODataClientSettings(_configOptions.Value.ODataServer + "/odata/");
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
