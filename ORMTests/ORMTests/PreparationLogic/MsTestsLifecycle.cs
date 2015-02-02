using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using ORMTests.Configuration;
using ORMTests.Data.TelerikOpenAccess;

namespace ORMTests.PreparationLogic
{
    [TestClass]
    public class MsTestsLifecycle
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            DropDatabaseIfExists();
            CreateDatabase();

            using (var dataContext = new OpenAccessContext())
            {
                var schemaHandler = dataContext.GetSchemaHandler();
                schemaHandler.CreateDatabase();
                schemaHandler.ForceExecuteDDLScript(schemaHandler.CreateDDLScript());
            }
        }

        private static void CreateDatabase()
        {
            using (var sqlConnection = new SqlConnection(ConfigurationService.GetMasterDbConnectionString()))
            {
                sqlConnection.Open();

                var sqlCommand = new SqlCommand("CREATE DATABASE " + Configuration.ConfigurationService.InstanceNameOfTestDatabase, sqlConnection);

                sqlCommand.ExecuteNonQuery();
            }
        }

        private static void DropDatabaseIfExists()
        {
            var dbName = Configuration.ConfigurationService.InstanceNameOfTestDatabase;
            var dropScript = @"
                IF db_id('" + dbName + @"') IS NOT NULL
                BEGIN
                    DECLARE @kill varchar(8000) = '';
                    SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), spid) + ';'
                    FROM master..sysprocesses
                    WHERE dbid = db_id('" + dbName + @"')
                    EXEC(@kill); 

                    DROP DATABASE " + dbName + @"
                END";

            using (var sqlConnection = new SqlConnection(ConfigurationService.GetMasterDbConnectionString()))
            {
                sqlConnection.Open();

                var sqlCommand = new SqlCommand(dropScript, sqlConnection);

                sqlCommand.ExecuteNonQuery();
            }
        }  
    }
}
