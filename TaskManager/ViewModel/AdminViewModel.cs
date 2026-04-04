using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TaskManagemetCore.DTOs;
using TaskManagemetCore.Enums;
using TaskManagemetCore.Models;
using TaskManagemetCore.Services;
using TaskManagementData.Factories;
using TaskManagementData.Repositories;
using TaskManager.Commands;

namespace TaskManager.ViewModel
{
    public class AdminViewModel : BaseViewModel
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _newCategoryName = string.Empty;
        private Category? _selectedCategory;
        private TaskPriority _selectedPriority;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public string NewCategoryName
        {
            get => _newCategoryName;
            set { _newCategoryName = value; OnPropertyChanged(); }
        }

        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); }
        }

        public TaskPriority SelectedPriority
        {
            get => _selectedPriority;
            set { _selectedPriority = value; OnPropertyChanged(); }
        }

        public Array Priorities => Enum.GetValues(typeof(TaskPriority));

        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<SelectableUserItem> Users { get; set; } = new();

        public ICommand CreateTaskCommand { get; }
        public ICommand AddCategoryCommand { get; }

        public AdminViewModel()
        {
            SelectedPriority = TaskPriority.MEDIUM;

            CreateTaskCommand = new RelayCommand(CreateTask);
            AddCategoryCommand = new RelayCommand(AddCategory);

            LoadData();
        }

        private void LoadData()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

            var userRepository = new UserRepository(context);
            var categoryRepository = new CategoryRepository(context);

            var users = userRepository.GetAll()
                .Where(u => u.Role == UserRole.USER)
                .ToList();

            var categories = categoryRepository.GetAll();

            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(new SelectableUserItem
                {
                    Id = user.Id,
                    Username = user.Username,
                    IsSelected = false
                });
            }

            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private void AddCategory()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewCategoryName))
                {
                    MessageBox.Show("Category name is required.");
                    return;
                }

                using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

                var category = new Category
                {
                    Name = NewCategoryName,
                    Description = string.Empty
                };

                context.Categories.Add(category);
                context.SaveChanges();

                Categories.Add(category);
                NewCategoryName = string.Empty;

                MessageBox.Show("Category added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateTask()
        {
            try
            {
                using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

                var taskRepository = new TaskRepository(context);
                var taskAssignmentRepository = new TaskAssignmentRepository(context);
                var taskHistoryRepository = new TaskHistoryRepository(context);
                var taskService = new TaskService(taskRepository, taskAssignmentRepository, taskHistoryRepository);

                var selectedUserIds = Users
                    .Where(u => u.IsSelected)
                    .Select(u => u.Id)
                    .ToList();

                taskService.CreateTask(new CreateTaskRequest
                {
                    Title = Title,
                    Description = Description,
                    CategoryId = SelectedCategory?.Id,
                    AssignedUserIds = selectedUserIds,
                    CreatedByUserId = 1,
                    Priority = SelectedPriority
                });

                MessageBox.Show("Task created successfully.");

                Title = string.Empty;
                Description = string.Empty;
                SelectedCategory = null;
                SelectedPriority = TaskPriority.MEDIUM;

                foreach (var user in Users)
                {
                    user.IsSelected = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}