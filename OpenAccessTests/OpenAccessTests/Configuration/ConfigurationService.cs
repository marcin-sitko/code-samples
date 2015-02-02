using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAccessTests.Configuration
{
    public class ConfigurationService
    {
        public static readonly string ConnectionStringName = "testDatabase";
        public static readonly string InstanceNameOfTestDatabase = System.Configuration.ConfigurationManager.AppSettings["nameOfTheTestedDatabase"];

        public static string GetConnectionString()
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionStringName];
            if (connection == null)
            {
                throw new ApplicationException(string.Format("Connection string with name {0} is not defined", connection.ConnectionString));
            }

            return connection.ConnectionString;
        }
    }
}
