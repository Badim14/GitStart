using Microsoft.EntityFrameworkCore;
using GitStart.Database;
using Xunit;
using System;
using System.Linq;
using System.IO;

namespace GitStart.Tests
{
    public class DatabaseTests
    {
        private readonly DbContextOptions<GitStartContext> _options;
        private const string DatabaseFileName = "GitStartTest.db";

        public DatabaseTests()
        {
            // Удаляем старую базу данных перед тестами
            if (File.Exists(DatabaseFileName))
            {
                File.Delete(DatabaseFileName);
            }

            // Настройка контекста для тестирования
            _options = new DbContextOptionsBuilder<GitStartContext>()
                .UseSqlite($"Data Source={DatabaseFileName}")
                .Options;

            // Создаем новую базу данных
            using (var context = new GitStartContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        [Fact]
        public void TestDatabaseConnection()
        {
            // Проверка успешного подключения к базе данных
            using (var context = new GitStartContext(_options))
            {
                Assert.True(context.Database.CanConnect(), "Не удалось подключиться к базе данных.");
            }
        }

        [Fact]
        public void TestAddUser()
        {
            // Тест добавления пользователя в базу данных
            using (var context = new GitStartContext(_options))
            {
                var user = new User
                {
                    Name = "Test User",
                    Email = "test@example.com",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                context.Users.Add(user);
                context.SaveChanges();

                var addedUser = context.Users.SingleOrDefault(u => u.Email == "test@example.com");
                Assert.NotNull(addedUser);
                Assert.Equal("Test User", addedUser.Name);
            }
        }

        [Fact]
        public void TestUniqueEmailConstraint()
        {
            // Тест уникальности email
            using (var context = new GitStartContext(_options))
            {
                var user1 = new User
                {
                    Name = "User One",
                    Email = "duplicate@example.com",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                var user2 = new User
                {
                    Name = "User Two",
                    Email = "duplicate@example.com", // Дублирующий email
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                context.Users.Add(user1);
                context.SaveChanges();

                // Попытка добавить второго пользователя с тем же email должна вызвать исключение
                Assert.Throws<DbUpdateException>(() =>
                {
                    context.Users.Add(user2);
                    context.SaveChanges();
                });
            }
        }

        [Fact]
        public void TestRepositoryUserRelationship()
        {
            // Проверка связи репозитория с пользователем
            using (var context = new GitStartContext(_options))
            {
                var user = new User
                {
                    Name = "Repo Owner",
                    Email = "owner@example.com",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Users.Add(user);
                context.SaveChanges();

                var repository = new Repository
                {
                    Name = "User Repo",
                    Path = "/repos/user_repo",
                    OwnerId = user.Id,
                    IsSynchronizedWithGitHub = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                context.Repositories.Add(repository);
                context.SaveChanges();

                var addedRepo = context.Repositories.SingleOrDefault(r => r.Name == "User Repo");
                Assert.NotNull(addedRepo);
                Assert.Equal(user.Id, addedRepo.OwnerId);
            }
        }

        [Fact]
        public void TestBranchRepositoryRelationship()
        {
            // Проверка связи ветки с репозиторием
            using (var context = new GitStartContext(_options))
            {
                var user = new User
                {
                    Name = "Branch Owner",
                    Email = "branch_owner@example.com",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Users.Add(user);
                context.SaveChanges();

                var repository = new Repository
                {
                    Name = "Branch Repo",
                    Path = "/repos/branch_repo",
                    OwnerId = user.Id,
                    IsSynchronizedWithGitHub = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Repositories.Add(repository);
                context.SaveChanges();

                var branch = new Branch
                {
                    Name = "main",
                    RepositoryId = repository.Id,
                    IsDefault = true,
                    IsRemote = false
                };

                context.Branches.Add(branch);
                context.SaveChanges();

                var addedBranch = context.Branches.SingleOrDefault(b => b.Name == "main");
                Assert.NotNull(addedBranch);
                Assert.Equal(repository.Id, addedBranch.RepositoryId);
            }
        }

        [Fact]
        public void TestCommitRepositoryRelationship()
        {
            // Проверка связи коммита с репозиторием
            using (var context = new GitStartContext(_options))
            {
                var user = new User
                {
                    Name = "Commit Owner",
                    Email = "commit_owner@example.com",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Users.Add(user);
                context.SaveChanges();

                var repository = new Repository
                {
                    Name = "Commit Repo",
                    Path = "/repos/commit_repo",
                    OwnerId = user.Id,
                    IsSynchronizedWithGitHub = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Repositories.Add(repository);
                context.SaveChanges();

                var commit = new Commit
                {
                    RepositoryId = repository.Id,
                    Hash = "commit123",
                    Author = "Commit Author",
                    Message = "Initial commit",
                    Timestamp = DateTime.Now
                };

                context.Commits.Add(commit);
                context.SaveChanges();

                var addedCommit = context.Commits.SingleOrDefault(c => c.Hash == "commit123");
                Assert.NotNull(addedCommit);
                Assert.Equal(repository.Id, addedCommit.RepositoryId);
            }
        }

        [Fact]
        public void TestRemoteRepositoryAssociation()
        {
            // Проверка связи удаленного репозитория с репозиторием
            using (var context = new GitStartContext(_options))
            {
                var user = new User
                {

                    Name = "Test User Owner",
                    Email = "test_user_owner@example.com",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now

                };
                context.Users.Add(user);
                context.SaveChanges();

                var repository = new Repository
                {
                    Name = "Remote Repo",
                    Path = "/repos/remote_repo",
                    IsSynchronizedWithGitHub = true,
                    OwnerId = user.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Repositories.Add(repository);
                context.SaveChanges();

                var remoteRepository = new RemoteRepository
                {
                    RepositoryId = repository.Id,
                    Url = "https://github.com/test/remote_repo",
                    Provider = "GitHub",
                    LastSync = DateTime.Now
                };

                context.RemoteRepositories.Add(remoteRepository);
                context.SaveChanges();

                var addedRemoteRepo = context.RemoteRepositories.SingleOrDefault(r => r.Url == "https://github.com/test/remote_repo");
                Assert.NotNull(addedRemoteRepo);
                Assert.Equal(repository.Id, addedRemoteRepo.RepositoryId);
            }
        }
    }
}
