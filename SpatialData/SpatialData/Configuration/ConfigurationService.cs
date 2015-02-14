using System;

namespace SpatialData.Configuration
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

        public static string GetMasterDbConnectionString()
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings[Configuration.ConfigurationService.ConnectionStringName];
            if (connection == null)
            {
                throw new ApplicationException(string.Format("Connection string with name {0} is not defined", Configuration.ConfigurationService.ConnectionStringName));
            }

            return connection.ConnectionString;
        }

        public static string GetTestDbConnectionString()
        {
            return string.Format("{0};Initial Catalog={1}", GetMasterDbConnectionString(), Configuration.ConfigurationService.InstanceNameOfTestDatabase);
        }
    }
}
