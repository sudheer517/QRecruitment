using IdentityModel.Client;
using ODataModels.Quantium.Recruitment.ApiServices.Models;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Portal.Helpers
{
    public interface ICandidateHelper
    {
        bool CheckIfCandidateExistsAndActive(string email);
        string GetRoleForEmail(string email);
        bool IsAdminActive(string email);
    }

    public class CandidateHelper : ICandidateHelper
    {
        public bool CheckIfCandidateExistsAndActive(string email)
        {
            var client = getOdataClient();

            var activeCandidates = client
                .For<CandidateDto>()
                .Filter(b => b.Email == email && b.IsActive == true)
                .Select(y => y.IsActive)
                .FindEntriesAsync();

            var candidate = activeCandidates.Result.FirstOrDefault();

            return candidate == null ? false : true;
        }

        public string GetRoleForEmail(string email)
        {
            var client = getOdataClient();

            var activeCandidates = 
                client.For<CandidateDto>().Filter(b => b.Email == email && b.IsActive == true).Select(y => y.IsActive).FindEntriesAsync();

            var candidate = activeCandidates.Result;

            var activeAdmins =
                client.For<AdminDto>().Filter(a => a.Email == email && a.IsActive == true).Select(b => b.IsActive).FindEntriesAsync();

            var admin2 = activeAdmins;

            var admin = admin2.Result;

            if (admin.FirstOrDefault() != null)
                return "SuperAdmin";
            else if (candidate.FirstOrDefault() != null)
                return "Candidate";
            else
                return string.Empty;
        }

        private ODataClient getOdataClient()
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

        public bool IsAdminActive(string email)
        {
            var client = getOdataClient();

            var activeADmins = client
                .For<AdminDto>()
                .Filter(b => b.Email == email && b.IsActive == true)
                .Select(y => y.IsActive)
                .FindEntriesAsync();

            var admin = activeADmins.Result.FirstOrDefault();

            return admin == null ? false : true;
        }
    }
}
