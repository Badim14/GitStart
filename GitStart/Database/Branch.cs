using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitStart.Database
{
    public class Branch
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public Repository Repository { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public bool IsRemote { get; set; }
    }
}

