using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using ORMTests.Configuration;
using ORMTests.Data.EntityFramework;
using ORMTests.Data.EntityFramework.Model;
using ORMTests.Data.TelerikOpenAccess;
using ORMTests.Data.TelerikOpenAccess.Model;
using Telerik.OpenAccess.Exceptions;

namespace ORMTests
{
    [TestClass]
    public class ORMTests
    {
        [TestMethod]
        public void OpenAccess_ConcurrentlyUpdateTheSameRecord_OptimisticConcurrencyExceptionShouldBeThrown()
        {
            //arrange
            var newRecordId = CreateNewOpenAccessRecord();

            using (var context1 = new OpenAccessContext())
            {
                var product1 = context1.GetAll<OpenAccessProduct>().First(p => p.Id == newRecordId);
                product1.Price = 1m;
                using (var context2 = new OpenAccessContext())
                {
                    var product2 = context2.GetAll<OpenAccessProduct>().First(p => p.Id == newRecordId);
                    product2.Price = 2m;

                    //act
                    context2.SaveChanges();
                }
                try
                {
                    context1.SaveChanges();
                }
                //assert
                catch (OptimisticVerificationException exception)
                {
                    System.Diagnostics.Trace.WriteLine(exception.ToString());
                    return;
                }
            }

            //We reached end of the test...it means that we assumed incorrect behaviour 
            Assert.Fail();
        }

        [TestMethod]
        public void EntityFramework_ConcurrentlyUpdateTheSameRecord_DbUpdateConcurrencyExceptionShouldBeThrown()
        {
            //arrange
            var newRecordId = CreateNewEntityFrameworkRecord();

            using (var context1 = new EntityFrameworkContext())
            {
                var product1 = context1.Products.First(p => p.Id == newRecordId);
                product1.Price = 3;
                using (var context2 = new EntityFrameworkContext())
                {
                    var product2 = context2.Products.First(p => p.Id == newRecordId);
                    product2.Price = 2;
                    context2.SaveChanges();
                }
                try
                {
                    context1.SaveChanges();
                }
                //assert
                catch (DbUpdateConcurrencyException exception)
                {
                    System.Diagnostics.Trace.WriteLine(exception.ToString());
                    return;
                }
            }

            //We reached end of the test...it means that we assumed incorrect behaviour 
            Assert.Fail();
        }

        private Guid CreateNewOpenAccessRecord()
        {
            using (var context = new OpenAccessContext())
            {
                var product = new OpenAccessProduct()
                {
                    Id = Guid.NewGuid()
                };

                context.Add(product);
                context.SaveChanges();
                return product.Id;
            }
        }

        private Guid CreateNewEntityFrameworkRecord()
        {
            using (var context = new EntityFrameworkContext())
            {
                var product = new EntityFrameworkProduct()
                {
                    Id = Guid.NewGuid()
                };
                context.Products.Add(product);
                context.SaveChanges();
                return product.Id;
            }
        }
    }
}
