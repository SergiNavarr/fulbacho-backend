using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Fulbacho.Shared
{

    public class FulbachoDbContextFactory : IDesignTimeDbContextFactory<FulbachoDbContext>
    {
        public FulbachoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FulbachoDbContext>();

            string connectionString = "Host=localhost;Port=5432;Database=FulbachoDB;Username=postgres;Password=152634";

            optionsBuilder.UseNpgsql(connectionString);

            return new FulbachoDbContext(optionsBuilder.Options);
        }
    }
}