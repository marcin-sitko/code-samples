using ORMTests.Data.TelerikOpenAccess.Model;
using Telerik.OpenAccess.Metadata.Fluent;

namespace ORMTests.Data.TelerikOpenAccess.Mappings
{
    public class DataMetadataSource : FluentMetadataSource
    {
        protected override System.Collections.Generic.IList<MappingConfiguration> PrepareMapping()
        {
            MappingConfiguration<OpenAccessProduct> productConfiguration = new MappingConfiguration<OpenAccessProduct>();
            productConfiguration.MapType().ToTable("OpenAccess_Products");
            productConfiguration.HasProperty(p => p.Id).IsIdentity();

            return new MappingConfiguration[]
            {
                productConfiguration
            };
        }
    }
}