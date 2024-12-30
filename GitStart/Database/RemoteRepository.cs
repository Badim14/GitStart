using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitStart.Database
{
    public class RemoteRepository
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public Repository Repository { get; set; } = null!;
        public string Url { get; set; } = string.Empty;
        public string Provider { get; set; } = "GitHub";
        public DateTime? LastSync { get; set; }
    }
}

