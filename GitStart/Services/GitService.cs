using LibGit2Sharp;
using System.IO;

namespace GitStart.Services
{
    public class GitService
    {
        public void CloneRepository(string url, string localPath)
        {
            Repository.Clone(url, localPath);
        }

        public void CommitChanges(string repoPath, string message)
        {
            using var repo = new Repository(repoPath);
            Commands.Stage(repo, "*");

            var author = new Signature("User", "user@example.com", DateTime.Now);
            repo.Commit(message, author, author);
        }
        public bool IsGitRepository(string path)
        {
            return Directory.Exists(Path.Combine(path, ".git"));
        }

        public void CreateBranch(string repoPath, string branchName)
        {
            using var repo = new Repository(repoPath);
            repo.CreateBranch(branchName);
        }

        public void CheckoutBranch(string repoPath, string branchName)
        {
            using var repo = new Repository(repoPath);
            var branch = repo.Branches[branchName];
            Commands.Checkout(repo, branch);
        }

        public void PushChanges(string repoPath)
        {
            using var repo = new Repository(repoPath);
            var remote = repo.Network.Remotes["origin"];
            var options = new PushOptions();
            repo.Network.Push(remote, @"refs/heads/main", options);
        }

        public void PullChanges(string repoPath)
        {
            using var repo = new Repository(repoPath);
            var signature = new Signature("User", "user@example.com", DateTime.Now);
            var pullOptions = new PullOptions(); // Используем правильный PullOptions
            Commands.Pull(repo, signature, pullOptions);
        }

    }
}
