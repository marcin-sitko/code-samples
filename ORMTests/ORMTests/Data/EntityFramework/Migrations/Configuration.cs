using System.Data.Entity.Migrations;

namespace ORMTests.Data.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EntityFrameworkContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
