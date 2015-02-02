using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using OpenAccessTests.Data.TelerikOpenAccess;

namespace OpenAccessTests.PreparationLogic
{
    [TestClass]
    public class MsTestsLifecycle
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            DropDatabaseIfExists();
            CreateDatabase();

            using (var dataContext = new DataContext())
            {
                var schemaHandler = dataContext.GetSchemaHandler();
                schemaHandler.CreateDatabase();
                schemaHandler.ForceExecuteDDLScript(schemaHandler.CreateDDLScript());
            }
        }

        public static string GetConnectionString()
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings[Configuration.ConfigurationService.ConnectionStringName];
            if (connection == null)
            {
                throw new ApplicationException(string.Format("Connection string with name {0} is not defined", Configuration.ConfigurationService.ConnectionStringName));
            }

            return connection.ConnectionString;
        }

        private static void  CreateDatabase(){
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();

                var sqlCommand = new SqlCommand("CREATE DATABASE " + Configuration.ConfigurationService.InstanceNameOfTestDatabase, sqlConnection);

                sqlCommand.ExecuteNonQuery();
            }
        }

        private static void DropDatabaseIfExists()
        {
            var dropScript = @"
                IF db_id('" + Configuration.ConfigurationService.InstanceNameOfTestDatabase + @"') IS NOT NULL
                BEGIN
                    DECLARE @kill varchar(8000) = '';
                    SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), spid) + ';'
                    FROM master..sysprocesses
                    WHERE dbid = db_id('" + Configuration.ConfigurationService.InstanceNameOfTestDatabase + @"')
                    EXEC(@kill); 

                    DROP DATABASE " + Configuration.ConfigurationService.InstanceNameOfTestDatabase + @"
                END";

            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();

                var sqlCommand = new SqlCommand(dropScript, sqlConnection);

                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
