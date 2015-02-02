using System;
using System.ComponentModel.DataAnnotations;

namespace ORMTests.Data.EntityFramework.Model
{
    public class EntityFrameworkProduct
    {
        public const string TableName = "EntityFramework_Product";

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
