using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GitStart.Database
{
    public class GitStartContextFactory : IDesignTimeDbContextFactory<GitStartContext>
    {
        public GitStartContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GitStartContext>();
            optionsBuilder.UseSqlite("Data Source=GitStart.db");

            return new GitStartContext(optionsBuilder.Options);
        }
    }
}
