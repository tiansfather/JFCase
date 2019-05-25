using Master.Configuration;
using Master.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Master.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class MasterDbContextFactory : DbContextFactory<MasterDbContext>
    {
        public override MasterDbContext CreateDbContext(DbContextOptions<MasterDbContext> options)
        {
            return new MasterDbContext(options);
        }
    }
}