using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementData.Factories;
using TaskManagementData.Repositories;
using TaskManagemetCore.DTOs;
using TaskManagemetCore.Services;

namespace TaskManager.ViewModel
{
    public class AuthViewModel : BaseViewModel
    {
        public LoginResult LoginUser(string username, string password)
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);
            var userRepo = new UserRepository(context);
            var authService = new AuthService(userRepo);

            return authService.Login(new LoginRequest
            {
                Username = username,
                Password = password
            });
        }
    }
}
