using LibGit2Sharp;
using System;
using System.Collections.Generic;

namespace GitStart.Database
{
    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsSynchronizedWithGitHub { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Commit> Commits { get; set; } = new();
        public List<Branch> Branches { get; set; } = new();

        public RemoteRepository GitHubRemoteRepository { get; set; } = null!;
    }
}

