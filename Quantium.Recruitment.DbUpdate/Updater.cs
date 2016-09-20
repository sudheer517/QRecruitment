using Quantium.Recruitment.DbInitialize;
using Microsoft.Practices.Unity;
using Quantium.Recruitment.Infrastructure;
using System;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;
using System.Configuration;
using System.Web.Hosting;
using System.Web;

namespace Quantium.Recruitment.DbUpdate
{
    class Updater
    {
        static void Main(string[] args)
        {
            var manifestPath = Path.Combine(Directory.GetCurrentDirectory(), "UpdateScriptManifest.txt");
            var updateScriptsPath = Path.Combine(Directory.GetCurrentDirectory(), "UpdateSQLScripts");

            var container = IocContainer.GetContainer();
            var dataUpdater = container.Resolve<IRecruitmentContext>();

            Console.WriteLine("Updating database");

            foreach (string sqlFileName in File.ReadLines(manifestPath))
            {
                string sqlQuery = File.ReadAllText(Path.Combine(updateScriptsPath, string.Format("{0}.sql", sqlFileName)));

                dataUpdater.GetDatabase().ExecuteSqlCommand(sqlQuery);

                Console.WriteLine("Updated " + sqlFileName);
            }
        }
    }
}
