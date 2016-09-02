using System;

namespace Quantium.Recruitment.Infrastructure
{
    public interface IConnectionString
    {
        string GetConnectionString();
    }

    public class ConnectionString : IConnectionString
    {
        public string GetConnectionString()
        {
            return @"Server=.\MSSQL2012;Database=QRecruitment;Integrated security = SSPI";
        }
    }
}