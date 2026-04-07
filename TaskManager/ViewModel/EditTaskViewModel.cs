using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private TaskStatus _selectedStatus;

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

        public TaskStatus SelectedStatus
        {
            get => _selectedStatus;
            set { _selectedStatus = value; OnPropertyChanged(); }
        }

        public Array Priorities => Enum.GetValues(typeof(TaskPriority));
        public Array Statuses => Enum.GetValues(typeof(TaskStatus));

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

            var taskRepository = new TaskRepository(context);
            var categoryRepository = new CategoryRepository(context);

            var task = taskRepository.GetById(_taskId);
            if (task == null)
            {
                MessageBox.Show("Task not found.");
                return;
            }

            // populate fields
            Title = task.Title;
            Description = task.Description;
            SelectedPriority = task.Priority;

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

                var taskRepository = new TaskRepository(context);

                var task = taskRepository.GetById(_taskId);
                if (task == null)
                {
                    MessageBox.Show("Task not found.");
                    return;
                }

                task.Title = Title;
                task.Description = Description;
                task.CategoryId = SelectedCategory.Id;
                task.Priority = SelectedPriority;
                taskRepository.Update(task);
                context.SaveChanges();

                MessageBox.Show("Task updated successfully.");

                // Затваряме прозореца успешно
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
