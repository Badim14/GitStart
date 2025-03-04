using Microsoft.EntityFrameworkCore;
using System;

namespace GitStart.Data
{
    public class GitDbContext : DbContext
    {
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Commit> Commits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string dbPath = System.IO.Path.Combine(AppContext.BaseDirectory, "gitstart.db");
            options.UseSqlite($"Data Source={dbPath}");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Repository>().HasData(
                new Repository { ID = 1, Name = "MyRepo", Path = "C:/Git/MyRepo", CreationDate = DateTime.Now }
            );

            modelBuilder.Entity<Branch>().HasData(
                new Branch { ID = 1, Name = "main", RepositoryID = 1 }
            );

            modelBuilder.Entity<Commit>().HasData(
                new Commit { ID = 1, Hash = "abc123", Message = "Initial commit", Timestamp = DateTime.Now, BranchID = 1 }
            );
        }
    }
}
