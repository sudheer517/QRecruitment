using AspNetCoreSpa.Server.Repositories.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Portal.Server.Helpers
{
    public interface IAccountHelper
    {
        bool CheckIfCandidateExistsAndActive(string email);
        bool IsAdminActive(string email);
        bool IsAccountActive(string email);
        string GetRoleForEmail(string email);
    }
    public class AccountHelper : IAccountHelper
    {
        private IEntityBaseRepository<Candidate> _candidateRepository;
        private IEntityBaseRepository<Admin> _adminRepository;

        public AccountHelper(IEntityBaseRepository<Candidate> candidateRepository, IEntityBaseRepository<Admin> adminRepository)
        {
            _candidateRepository = candidateRepository;
            _adminRepository = adminRepository;
        }

        public string GetRoleForEmail(string email)
        {
            var isAdmin = IsAdminActive(email);

            if (isAdmin)
                return "Admin";

            var isCandidate = this.CheckIfCandidateExistsAndActive(email);

            if (isCandidate)
                return "Candidate";

            return string.Empty;
        }

        public bool CheckIfCandidateExistsAndActive(string email)
        {
            var candidate = _candidateRepository.GetAll().SingleOrDefault(item => item.Email == email && item.IsActive == true);

            if (candidate != null)
                return true;
            else
                return false;
        }

        public bool IsAdminActive(string email)
        {
            var admin = _adminRepository.GetAll().SingleOrDefault(a => a.Email == email && a.IsActive == true);

            if (admin != null)
                return true;
            else
                return false;
        }

        public bool IsAccountActive(string email)
        {
            if (this.CheckIfCandidateExistsAndActive(email) || this.IsAdminActive(email))
                return true;
            else
                return false;
        }

        public static string GenerateRandomString()
        {
            var length = new Random().Next(5, 10);
            string allowedLetterChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string allowedNumberChars = "23456789";
            char[] chars = new char[length];
            Random rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < length; i++)
            {
                if (useLetter)
                {
                    chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)];
                    useLetter = false;
                }
                else
                {
                    chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)];
                    useLetter = true;
                }

            }

            return new string(chars);
        }
    }
}
