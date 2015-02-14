using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using SpatialData.Configuration;
using SpatialData.Sql;

namespace SpatialData
{
    [TestClass]
    public class SpatialDataTest
    {
        [TestMethod]
        public void SpatialInMemory_PerformanceTest()
        {
            const int pointsCount = 50000;
            const int ammountOfNearestPointsToFind = 5;
            var randomGenerator = new Random();
            var memoryUsage = GC.GetTotalMemory(true);
            var coordinates = new List<GeoCoordinate>();

            for (int i = 0; i < pointsCount; i++)
            {
                coordinates.Add(new GeoCoordinate(randomGenerator.Next(0, 9000) / 1000.0, randomGenerator.Next(0, 9000) / 1000.0));
            }
            memoryUsage = GC.GetTotalMemory(true) - memoryUsage;
            Trace.WriteLine(string.Format("GeoCoordinates memory usage {0} Mb", memoryUsage / 1024.0 / 1024.0));

            var searchPoint = new GeoCoordinate(randomGenerator.Next(0, 9000) / 1000.0,
                randomGenerator.Next(0, 9000) / 1000.0);
            GC.Collect();
            GC.WaitForFullGCComplete();
            var watch = new Stopwatch();

            watch.Start();
            for (int i = 0; i < 100; i++)
            {
                var nearestFiveCoordinates = coordinates.OrderBy(coordinate => coordinate.GetDistanceTo(searchPoint)).Take(ammountOfNearestPointsToFind).ToList();
            }
            watch.Stop();
            Trace.WriteLine(string.Format("For {0} spatial points it took {1} ms to found {2} nearest points", pointsCount, watch.ElapsedMilliseconds / 100, ammountOfNearestPointsToFind));
            watch.Restart();
            for (int i = 0; i < 100; i++)
            {
                var nearestFiveCoordinates = coordinates.AsParallel().OrderBy(coordinate => coordinate.GetDistanceTo(searchPoint)).Take(5).ToList();
            }
            watch.Stop();
            Trace.WriteLine(string.Format("Parallel - For {0} spatial points it took {1} ms to found {2} nearest points", pointsCount, watch.ElapsedMilliseconds / 100, ammountOfNearestPointsToFind));
        }

        [TestMethod]
        public void SpatialDatabase_PerformanceTest()
        {
            using (var connection = new SqlConnection(ConfigurationService.GetTestDbConnectionString()))
            {
                connection.Open();
                using (var command = new SqlCommand(ScriptsService.GetScript("GetNearestLocations"), connection))
                {
                    GC.Collect();
                    GC.WaitForFullGCComplete();
                    var watch = new Stopwatch();
                    watch.Start();
                    for (int i = 0; i < 100; i++)
                    {
                        command.ExecuteReader().Close();
                    }
                    watch.Stop();
                    Trace.WriteLine(string.Format("SQL Server - For {0} spatial points it took {1} ms to found {2} nearest points", GetLocationsAmmountFromDb(), watch.ElapsedMilliseconds / 100, 5));
                }
            }
        }

        private int GetLocationsAmmountFromDb()
        {
            using (var connection = new SqlConnection(ConfigurationService.GetTestDbConnectionString()))
            {
                connection.Open();
                using (var command = new SqlCommand(ScriptsService.GetScript("GetLocationsAmmount"), connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}
