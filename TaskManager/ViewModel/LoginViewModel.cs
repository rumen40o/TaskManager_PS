using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManagemetCore.DTOs;
using TaskManagemetCore.Services;
using TaskManagemetCore.Enums;
using TaskManagementData.Factories;
using TaskManagementData.Repositories;
using TaskManager.Commands;
using TaskManager.View;

namespace TaskManager.ViewModel
{
    public class LoginViewModel : AuthViewModel
    {
        private string _username = string.Empty;
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

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenRegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            OpenRegisterCommand = new RelayCommand(OpenRegister);
        }

        private void Login()
        {
            var result = LoginUser(Username, Password);

            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Message);
                return;
            }

            MessageBox.Show("Успешен вход!");

            if (result.User!.Role == UserRole.ADMIN)
            {
                var adminWindow = new AdminDashboard();
                adminWindow.Show();
            }
            else
            {
                var userWindow = new UserDashboard(result.User.Id);
                userWindow.Show();
            }

            foreach (Window window in Application.Current.Windows)
            {
                if (window is LoginWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

        private void OpenRegister()
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is LoginWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
