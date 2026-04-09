using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManagementData.Repositories;
using TaskManagemetCore.Enums;
using TaskManagemetCore.DTOs;
using TaskManager.Commands;
using Microsoft.EntityFrameworkCore.Internal;
using TaskManagementData.Factories;

namespace TaskManager.ViewModel
{
    public class AdminUsersViewModel : BaseViewModel
    {
        private string _searchUsername = "";
        private string _searchEmail = "";
        private UserRole? _searchRole;

        public string SearchUsername
        {
            get => _searchUsername;
            set { _searchUsername = value; OnPropertyChanged(); }
        }

        public string SearchEmail
        {
            get => _searchEmail;
            set { _searchEmail = value; OnPropertyChanged(); }
        }

        public UserRole? SearchRole
        {
            get => _searchRole;
            set { _searchRole = value; OnPropertyChanged(); }
        }

        public Array Roles => Enum.GetValues(typeof(UserRole));

        public ObservableCollection<UserDTO> Users { get; set; } = new();

        public ICommand SearchUsersCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        public AdminUsersViewModel()
        {
            SearchUsersCommand = new RelayCommand(SearchUsers);
            ClearFiltersCommand = new RelayCommand(ClearFilters);
            LoadUsers();
        }

        private void LoadUsers()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);
            var repo = new UserRepository(context);

            var users = repo.GetAll();

            Users.Clear();
            foreach (var u in users)
            {
                Users.Add(new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role
                });
            }
        }
        private void ClearFilters()
        {
            SearchUsername = string.Empty;
            SearchEmail = string.Empty;
            SearchRole = null;

            LoadUsers();
        }

        private void SearchUsers()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);
            var repo = new UserRepository(context);

            var query = context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchUsername))
                query = query.Where(u => u.Username.Contains(SearchUsername));

            if (!string.IsNullOrWhiteSpace(SearchEmail))
                query = query.Where(u => u.Email.Contains(SearchEmail));

            if (SearchRole.HasValue)
                query = query.Where(u => u.Role == SearchRole.Value);

            var result = query.ToList();

            Users.Clear();
            foreach (var u in result)
            {
                Users.Add(new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role
                });
            }
        }
    }
}
