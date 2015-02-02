using OpenAccessTests.Data.Model;
using Telerik.OpenAccess.Metadata.Fluent;

namespace OpenAccessTests.Data.TelerikOpenAccess.Mappings
{
    public class DataMetadataSource : FluentMetadataSource
    {
        protected override System.Collections.Generic.IList<MappingConfiguration> PrepareMapping()
        {
            MappingConfiguration<Product> productConfiguration = new MappingConfiguration<Product>();
            productConfiguration.MapType().ToTable("Products");
            productConfiguration.HasProperty(p => p.Id).IsIdentity();

            return new MappingConfiguration[]
            {
                productConfiguration
            };
        }
    }
}