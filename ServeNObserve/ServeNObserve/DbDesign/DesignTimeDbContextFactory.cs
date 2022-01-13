using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ServeNObserve.Persistence.Contexts;

namespace ServeNObserve.DbDesign
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            var connectionString = configuration.GetConnectionString("LocalDb");
            builder.UseSqlServer(connectionString);
            return new DatabaseContext(builder.Options);
        }
    }
}
