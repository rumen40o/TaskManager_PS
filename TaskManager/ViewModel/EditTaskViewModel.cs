using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TaskManagementData.Factories;
using TaskManagementData.Repositories;
using TaskManagemetCore.Enums;
using TaskManagemetCore.Models;
using TaskManager.Commands;

namespace TaskManager.ViewModel
{
    public class EditTaskViewModel : BaseViewModel
    {
        private readonly int _taskId;

        private string _title = string.Empty;
        private string _description = string.Empty;
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
            set { _dueDate = value; OnPropertyChanged(); }
        }

        public Array Priorities => Enum.GetValues(typeof(TaskPriority));

        public ObservableCollection<Category> Categories { get; set; } = new();

        public ICommand SaveCommand { get; }

        public EditTaskViewModel(int taskId)
        {
            _taskId = taskId;
            SaveCommand = new RelayCommand(SaveTask);

            LoadData();
        }

        private void LoadData()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

            var taskRepository = new TaskRepository(context);
            var categoryRepository = new CategoryRepository(context);

            var task = taskRepository.GetById(_taskId);
            if (task == null)
            {
                MessageBox.Show("Task not found.");
                return;
            }

            Title = task.Title;
            Description = task.Description ?? string.Empty;
            SelectedPriority = task.Priority;
            DueDate = task.DueDate.HasValue
                ? DateTime.SpecifyKind(task.DueDate.Value, DateTimeKind.Utc)
                : null;

            var categories = categoryRepository.GetAll();

            Categories.Clear();
            foreach (var c in categories)
            {
                Categories.Add(c);
            }

            SelectedCategory = Categories.FirstOrDefault(c => c.Id == task.CategoryId);
        }

        private void SaveTask()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Title))
                {
                    MessageBox.Show("Title is required.");
                    return;
                }

                if (SelectedCategory == null)
                {
                    MessageBox.Show("Category is required.");
                    return;
                }

                using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);
                var taskRepository = new TaskRepository(context);

                var task = taskRepository.GetById(_taskId);
                if (task == null)
                {
                    MessageBox.Show("Task not found.");
                    return;
                }

                var dueDateUtc = DueDate.HasValue
                    ? DateTime.SpecifyKind(DueDate.Value.Date, DateTimeKind.Utc)
                    : (DateTime?)null;

                task.Title = Title;
                task.Description = Description;
                task.CategoryId = SelectedCategory.Id;
                task.Priority = SelectedPriority;
                task.DueDate = dueDateUtc;

                taskRepository.Update(task);
                context.SaveChanges();

                MessageBox.Show("Task updated successfully.");

                var activeWindow = Application.Current.Windows
                    .OfType<Window>()
                    .SingleOrDefault(w => w.IsActive);

                if (activeWindow != null)
                {
                    activeWindow.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}