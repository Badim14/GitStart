using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitStart.Database
{
    public class GitStartContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Repository> Repositories { get; set; } = null!;
        public DbSet<Commit> Commits { get; set; } = null!;
        public DbSet<Branch> Branches { get; set; } = null!;
        public DbSet<RemoteRepository> RemoteRepositories { get; set; } = null!;
        public DbSet<Settings> Settings { get; set; } = null!;

        public GitStartContext(DbContextOptions<GitStartContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=GitStart.db");
        }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Индексы для уникальных полей
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Repository>()
                .HasIndex(r => r.Path)
                .IsUnique();

            modelBuilder.Entity<Commit>()
                .HasIndex(c => c.Hash)
                .IsUnique();
        }
    }
}
