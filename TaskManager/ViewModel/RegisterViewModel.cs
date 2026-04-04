using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManagementData.Factories;
using TaskManagementData.Repositories;
using TaskManagemetCore.DTOs;
using TaskManagemetCore.Enums;
using TaskManagemetCore.Services;
using TaskManager.Commands;
using TaskManager.View;

namespace TaskManager.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _username = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand { get; }
        public ICommand BackToLoginCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
            BackToLoginCommand = new RelayCommand(BackToLogin);
        }

        private void Register()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);
            var userRepository = new UserRepository(context);
            var authService = new AuthService(userRepository);

            var result = authService.Register(new RegisterUserRequest
            {
                Username = Username,
                Email = Email,
                Password = Password
            });

            MessageBox.Show(result.Message, "Register", MessageBoxButton.OK, MessageBoxImage.Information);

            if (result.IsSuccess)
            {
                BackToLogin();
            }
        }

        private void BackToLogin()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is RegisterWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
