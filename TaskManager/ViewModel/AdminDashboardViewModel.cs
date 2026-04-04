using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
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
using TaskManagemetCore.DTOs;
using TaskManagemetCore.Enums;
using TaskManagemetCore.Models;
using TaskManager.Commands;
using TaskManager.View;

namespace TaskManager.ViewModel
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        private TaskDashboardDto? _selectedTask;

        public ObservableCollection<TaskDashboardDto> Tasks { get; set; } = new();

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

        public ICommand OpenCreateTaskCommand { get; }
        public ICommand OpenEditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand RefreshTasksCommand { get; }

        public AdminDashboardViewModel()
        {
            OpenCreateTaskCommand = new RelayCommand(OpenCreateTask);
            OpenEditTaskCommand = new RelayCommand(OpenEditTask, CanEditOrDeleteTask);
            DeleteTaskCommand = new RelayCommand(DeleteTask, CanEditOrDeleteTask);
            RefreshTasksCommand = new RelayCommand(LoadTasks);

            LoadTasks();
        }

        private bool CanEditOrDeleteTask()
        {
            return SelectedTask != null;
        }

        private void LoadTasks()
        {
            using var context = DbContextFactory.Create(DatabaseProvider.PostgreSql);
            var taskRepository = new TaskRepository(context);

            var tasks = taskRepository.GetAllForDashboard();

            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
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

                using var context = DbContextFactory.Create(DatabaseProvider.PostgreSql);
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
