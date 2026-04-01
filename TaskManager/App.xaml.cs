using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Configuration;
using System.Data;
using System.Windows;
using TaskManagementData;
using TaskManagementData.Factories;
using TaskManagemetCore.Enums;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using var context = DbContextFactory.Create(DatabaseProvider.PostgreSql);

            DbSeeder.Seed(context);
        }

    }

}
