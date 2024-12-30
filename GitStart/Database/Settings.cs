using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitStart.Database
{
    public class Settings
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Configuration { get; set; } = string.Empty;  // JSON строка с настройками
    }
}
