using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Data.Database
{
    public class DatabaseConfiguration : DbConfiguration
    {
        public DatabaseConfiguration() : base()
        {
            var path = Path.GetDirectoryName(GetType().Assembly.Location);

            SetModelStore(new DefaultDbModelStore(path));
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy(3, TimeSpan.FromSeconds(10))); 
        }
    }
}