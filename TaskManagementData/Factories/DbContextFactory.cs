using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementData.Contexts;
using TaskManagemetCore.Enums;

namespace TaskManagementData.Factories
{
    public static class DbContextFactory 
    {
        public static AppDbContext Create(DatabaseProvider provider)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();

            switch (provider)
            {
                case DatabaseProvider.Sqlite:
                    builder.UseSqlite("Data Source=taskmanager.db");
                    break;

                case DatabaseProvider.PostgreSql:
                    builder.UseNpgsql("Host=localhost;Port=5432;Database=taskmanager_pg;Username=postgres;Password=1111");
                    break;

                default:
                    throw new Exception("Unsupported database");
            }

            return new AppDbContext(builder.Options);
        }
    }
}
