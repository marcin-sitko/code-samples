using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpatialData.Sql
{

    /// <summary>
    /// Marked this internal so it can't be executed in another assembly
    /// </summary>
    internal static class ScriptsService
    {
        internal static string GetScript(string scriptName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = string.Format("SpatialData.Sql.{0}.sql", scriptName);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new ApplicationException(string.Format("Missing {0} .sql script for test data preparation", scriptName));
                }
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
