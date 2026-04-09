using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TaskManagementData.Factories;
using TaskManagementData.Repositories;
using TaskManagemetCore.DTOs;
using TaskManagemetCore.Enums;
using TaskManager.Commands;
using TaskManager.View;

namespace TaskManager.ViewModel
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        private TaskDashboardDto? _selectedTask;

        private string _searchTitle = string.Empty;
        private DateTime? _searchDueDate;
        private TaskPriority? _searchPriority;
        private DateTime? _searchCreatedAt;
        private int? _searchCreatedByUserId;
        private int? _searchCategoryId;

        public ObservableCollection<TaskDashboardDto> Tasks { get; set; } = new();
        public ObservableCollection<ComboBoxItemDto> Users { get; set; } = new();
        public ObservableCollection<ComboBoxItemDto> Categories { get; set; } = new();

        public Array Priorities => Enum.GetValues(typeof(TaskPriority));

        public TaskDashboardDto? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string SearchTitle
        {
            get => _searchTitle;
            set
            {
                _searchTitle = value;
                OnPropertyChanged();
            }
        }

        public DateTime? SearchDueDate
        {
            get => _searchDueDate;
            set
            {
                _searchDueDate = value;
                OnPropertyChanged();
            }
        }

        public TaskPriority? SearchPriority
        {
            get => _searchPriority;
            set
            {
                _searchPriority = value;
                OnPropertyChanged();
            }
        }

        public DateTime? SearchCreatedAt
        {
            get => _searchCreatedAt;
            set
            {
                _searchCreatedAt = value;
                OnPropertyChanged();
            }
        }

        public int? SearchCreatedByUserId
        {
            get => _searchCreatedByUserId;
            set
            {
                _searchCreatedByUserId = value;
                OnPropertyChanged();
            }
        }

        public int? SearchCategoryId
        {
            get => _searchCategoryId;
            set
            {
                _searchCategoryId = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenCreateTaskCommand { get; }
        public ICommand OpenEditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand RefreshTasksCommand { get; }
        public ICommand SearchTasksCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        public ICommand OpenUserWindowCommand { get; }

        public AdminDashboardViewModel()
        {
            OpenCreateTaskCommand = new RelayCommand(OpenCreateTask);
            OpenEditTaskCommand = new RelayCommand(OpenEditTask, CanEditOrDeleteTask);
            OpenUserWindowCommand = new RelayCommand(OpenUserWindow);
            DeleteTaskCommand = new RelayCommand(DeleteTask, CanEditOrDeleteTask);
            RefreshTasksCommand = new RelayCommand(LoadTasks);
            SearchTasksCommand = new RelayCommand(SearchTasks);
            ClearFiltersCommand = new RelayCommand(ClearFilters);

            LoadUsers();
            LoadCategories();
            LoadTasks();
        }

        private bool CanEditOrDeleteTask()
        {
            return SelectedTask != null;
        }

        private void LoadTasks()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);
            var taskRepository = new TaskRepository(context);

            var tasks = taskRepository.GetAllForDashboard();

            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }

        private void LoadUsers()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

            Users.Clear();

            var users = context.Users
                .Select(u => new ComboBoxItemDto
                {
                    Id = u.Id,
                    Name = u.Username
                })
                .ToList();

            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private void LoadCategories()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

            Categories.Clear();

            var categories = context.Categories
                .Select(c => new ComboBoxItemDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private void SearchTasks()
        {
            try
            {
                using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);
                var taskRepository = new TaskRepository(context);

                var dueDateUtc = SearchDueDate.HasValue
                    ? DateTime.SpecifyKind(SearchDueDate.Value.Date, DateTimeKind.Utc)
                    : (DateTime?)null;

                var createdAtUtc = SearchCreatedAt.HasValue
                    ? DateTime.SpecifyKind(SearchCreatedAt.Value.Date, DateTimeKind.Utc)
                    : (DateTime?)null;

                var filter = new TaskSearchFilterDto
                {
                    Title = string.IsNullOrWhiteSpace(SearchTitle) ? null : SearchTitle,
                    DueDate = dueDateUtc,
                    Priority = SearchPriority,
                    CreatedAt = createdAtUtc,
                    CreatedByUserId = SearchCreatedByUserId,
                    CategoryId = SearchCategoryId
                };

                var tasks = taskRepository.SearchForDashboard(filter);

                Tasks.Clear();
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearFilters()
        {
            SearchTitle = string.Empty;
            SearchDueDate = null;
            SearchPriority = null;
            SearchCreatedAt = null;
            SearchCreatedByUserId = null;
            SearchCategoryId = null;

            LoadTasks();
        }

        private void OpenCreateTask()
        {
            try
            {
                var window = new AddTaskWindow();
                var result = window.ShowDialog();

                if (result == true)
                {
                    LoadTasks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenUserWindow()
        {
            try
            {
                var window = new AdminUsersView();
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenEditTask()
        {
            try
            {
                if (SelectedTask == null)
                    return;

                var window = new EditTaskWindow(SelectedTask.Id);
                var result = window.ShowDialog();

                if (result == true)
                {
                    LoadTasks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTask()
        {
            try
            {
                if (SelectedTask == null)
                    return;

                var result = MessageBox.Show(
                    $"Are you sure you want to delete task \"{SelectedTask.Title}\"?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                    return;

                using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);
                var taskRepository = new TaskRepository(context);

                var task = taskRepository.GetById(SelectedTask.Id);
                if (task == null)
                {
                    MessageBox.Show("Task not found.");
                    return;
                }

                taskRepository.Delete(task);
                context.SaveChanges();

                MessageBox.Show("Task deleted successfully.");
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}