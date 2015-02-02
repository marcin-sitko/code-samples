using System.Linq;
using OpenAccessTests.Data.Model;
using OpenAccessTests.Data.TelerikOpenAccess.Mappings;
using OpenAccessTests.PreparationLogic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;

namespace OpenAccessTests.Data.TelerikOpenAccess
{
    public partial class DataContext : OpenAccessContext
    {
        static readonly MetadataContainer MetadataContainer = new DataMetadataSource().GetModel();
        static readonly BackendConfiguration BackendConfiguration = new BackendConfiguration()
        {
            Backend = "mssql"
        };

        public DataContext()
            : base(MsTestsLifecycle.GetConnectionString() + ";Initial Catalog=" + Configuration.ConfigurationService.InstanceNameOfTestDatabase, BackendConfiguration, MetadataContainer)
        {
        }

        public IQueryable<Product> Products
        {
            get
            {
                return this.GetAll<Product>();
            }
        }
    }
}
