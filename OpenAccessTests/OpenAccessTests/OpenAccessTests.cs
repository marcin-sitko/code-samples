using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using OpenAccessTests.Data.Model;
using OpenAccessTests.Data.TelerikOpenAccess;
using Telerik.OpenAccess.Exceptions;

namespace OpenAccessTests
{
    [TestClass]
    public class OpenAccessTests
    {
        [TestMethod]
        public void ConcurrentlyUpdateTheSameRecord_OptimisticConcurrencyExceptionShouldBeThrown()
        {
            var newRecordId = CreateNewRecord();
           
            using (var context1 = new DataContext())
            {
                var product1 = context1.GetAll<Product>().First(p => p.Id == newRecordId);
                product1.Price = 1m;
                using (var context2 = new DataContext())
                {
                    var product2 = context2.GetAll<Product>().First(p => p.Id == newRecordId);
                    product2.Price = 2m;
                    context2.SaveChanges();
                }
                try
                {
                    context1.SaveChanges();
                }
                catch (OptimisticVerificationException exception)
                {
                    System.Diagnostics.Trace.WriteLine(exception.ToString()); 

                     
                    return;
                }
            }
        
            //We reached end of the test...it means that we assumed incorrect behaviour 
            Assert.Fail();
        }

        private Guid CreateNewRecord()
        {
            using (var context = new DataContext())
            {
                var product = new Product()
                {
                    Id = Guid.NewGuid(),
                };

                context.Add(product);
                context.SaveChanges();
                return product.Id;
            }
        }
    }
}
