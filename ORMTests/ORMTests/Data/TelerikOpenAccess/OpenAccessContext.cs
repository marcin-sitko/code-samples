using System.Linq;
using ORMTests.PreparationLogic;
using ORMTests.Configuration;
using ORMTests.Data.TelerikOpenAccess.Mappings;
using ORMTests.Data.TelerikOpenAccess.Model;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;

namespace ORMTests.Data.TelerikOpenAccess
{
    public partial class OpenAccessContext : Telerik.OpenAccess.OpenAccessContext
    {
        static readonly MetadataContainer MetadataContainer = new DataMetadataSource().GetModel();
        static readonly BackendConfiguration BackendConfiguration = new BackendConfiguration()
        {
            Backend = "mssql"
        };

        public OpenAccessContext()
            : base(ConfigurationService.GetTestDbConnectionString(), BackendConfiguration, MetadataContainer)
        {
        }

        public IQueryable<OpenAccessProduct> Products
        {
            get
            {
                return this.GetAll<OpenAccessProduct>();
            }
        }
    }
}
