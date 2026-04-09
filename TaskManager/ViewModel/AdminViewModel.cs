using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TaskManagementData.Factories;
using TaskManagementData.Repositories;
using TaskManagemetCore.DTOs;
using TaskManagemetCore.Enums;
using TaskManagemetCore.Models;
using TaskManagemetCore.Services;
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
        private DateTime? _dueDate;

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

        public DateTime? DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                OnPropertyChanged();
            }
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

        private void LoadCategoriesOnly()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

            var categoryRepository = new CategoryRepository(context);
            var categories = categoryRepository.GetAll();

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

                var exists = context.Categories.Any(c => c.Name == NewCategoryName.Trim());
                if (exists)
                {
                    MessageBox.Show("Category already exists.");
                    return;
                }

                var category = new Category
                {
                    Name = NewCategoryName.Trim(),
                    Description = string.Empty
                };

                context.Categories.Add(category);
                context.SaveChanges();

                LoadCategoriesOnly();

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
                if (string.IsNullOrWhiteSpace(Title))
                {
                    MessageBox.Show("Title is required.");
                    return;
                }

                using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

                var taskRepository = new TaskRepository(context);
                var taskAssignmentRepository = new TaskAssignmentRepository(context);
                var taskHistoryRepository = new TaskHistoryRepository(context);
                var taskService = new TaskService(taskRepository, taskAssignmentRepository, taskHistoryRepository);

                var selectedUserIds = Users
                    .Where(u => u.IsSelected)
                    .Select(u => u.Id)
                    .ToList();

                var dueDateUtc = DueDate.HasValue
                    ? DateTime.SpecifyKind(DueDate.Value, DateTimeKind.Utc)
                        : (DateTime?)null;

                taskService.CreateTask(new CreateTaskRequest
                {
                    Title = Title,
                    Description = Description,
                    CategoryId = SelectedCategory?.Id,
                    AssignedUserIds = selectedUserIds,
                    CreatedByUserId = 1,
                    Priority = SelectedPriority,
                    DueDate = dueDateUtc
                });

                MessageBox.Show("Task created successfully.");

                Title = string.Empty;
                Description = string.Empty;
                SelectedCategory = null;
                SelectedPriority = TaskPriority.MEDIUM;
                DueDate = null;

                foreach (var user in Users)
                {
                    user.IsSelected = false;
                }
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                var current = ex;

                while (current != null)
                {
                    sb.AppendLine(current.GetType().FullName);
                    sb.AppendLine(current.Message);
                    sb.AppendLine("-----------------------------------");
                    current = current.InnerException;
                }

                MessageBox.Show(sb.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}