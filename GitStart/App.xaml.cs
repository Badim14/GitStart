using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Windows;
using GitStart.Data;

namespace GitStart
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var context = new GitDbContext())
            {
                context.Database.EnsureCreated();  
            }
        }
    }
}
