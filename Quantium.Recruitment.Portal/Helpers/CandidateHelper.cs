using IdentityModel.Client;
using Newtonsoft.Json;
using Quantium.Recruitment.Models;
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
        string GeneratePassword();
    }

    public class CandidateHelper : ICandidateHelper
    {
        private IHttpHelper _helper;

        public CandidateHelper(IHttpHelper helper)
        {
            _helper = helper;
        }

        public bool CheckIfCandidateExistsAndActive(string email)
        {
            var candidate = _helper.GetData($"api/Candidate/GetCandidateByEmail?Email={email}");

            var result = JsonConvert.DeserializeObject<CandidateDto>(candidate.Content.ReadAsStringAsync().Result);

            if (result != null)
                return true;
            else
                return false;
        }

        public string GetRoleForEmail(string email)
        {
            var isAdmin = IsAdminActive(email);

            if (isAdmin)
                return "SuperAdmin";

            var candidate = _helper.GetData($"api/Candidate/GetCandidateByEmail?Email={email}");

            var result = JsonConvert.DeserializeObject<CandidateDto>(candidate.Content.ReadAsStringAsync().Result);

            if (result != null)
                return "Candidate";

            return string.Empty;
        }

        public bool IsAdminActive(string email)
        {
            var candidate = _helper.GetData($"api/Admin/IsAdmin?Email={email}");

            var result = JsonConvert.DeserializeObject<AdminDto>(candidate.Content.ReadAsStringAsync().Result);

            if (result != null)
                return true;
            else
                return false;
        }
        public string GeneratePassword()
        {
            Random randomizer = new Random();
            List<char> pwd = new List<char>();
            for (int i = 0; i < 3; i++)
            {
                pwd.Add((char)randomizer.Next(97, 122));
            }
            pwd.Add((char)randomizer.Next(65, 90));
            pwd.Add((char)randomizer.Next(35, 46));
            for (int i = 0; i < 3; i++)
            {
                pwd.Add((char)randomizer.Next(48, 57));
            }
            return new string(pwd.ToArray());
        }
    }
}
