using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ORMTests.Configuration;
using ORMTests.Data.EntityFramework.Model;
using Telerik.OpenAccess;

namespace ORMTests.Data.EntityFramework
{
    public class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext() : base(ConfigurationService.GetTestDbConnectionString())
        {
        }

        public DbSet<EntityFrameworkProduct> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntityFrameworkProduct>().ToTable(EntityFrameworkProduct.TableName);
        } 
    }
}
