using System;

namespace ORMTests.Data.TelerikOpenAccess.Model
{
    public class OpenAccessProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
    }
}