using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementData.Contexts;
using TaskManagemetCore.Enums;
using TaskManagemetCore.Models;

namespace TaskManagementData
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            
            if (context.Users.Any(u => u.Role == UserRole.ADMIN))
                return;

            var admin = new User
            {
                Username = "admin",
                Email = "admin@local.com",
                PasswordHash = "admin123",
                Role = UserRole.ADMIN,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
