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
using TaskManagemetCore.DTOs;
using TaskManagemetCore.Enums;
using TaskManagemetCore.Services;
using TaskManager.Commands;

namespace TaskManager.ViewModel
{
    public class UserDashboardViewModel : BaseViewModel
    {
        private UserTaskItemViewModel? _selectedTask;
        private readonly int _currentUserId;

        public ObservableCollection<UserTaskItemViewModel> Tasks { get; set; } = new();

        public UserTaskItemViewModel? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
            }
        }

        public Array Statuses => Enum.GetValues(typeof(AssignmentStatus));

        public ICommand RefreshTasksCommand { get; }
        public ICommand UpdateStatusCommand { get; }



        public UserDashboardViewModel(int currentUserId)
        {
            _currentUserId = currentUserId;

            RefreshTasksCommand = new RelayCommand(LoadTasks);
            UpdateStatusCommand = new RelayCommand(UpdateStatus);

            LoadTasks();
        }

        private void LoadTasks()
        {
            using var context = DbContextFactory.Create(DatabaseProvider.PostgreSql);

            var assignmentRepository = new TaskAssignmentRepository(context);
            var assignments = assignmentRepository.GetByUserId(_currentUserId);

            Tasks.Clear();

            foreach (var assignment in assignments)
            {
                Tasks.Add(new UserTaskItemViewModel
                {
                    TaskAssignmentId = assignment.Id,
                    Title = assignment.Task.Title,
                    Description = assignment.Task.Description ?? "",
                    CategoryName = assignment.Task.Category != null ? assignment.Task.Category.Name : "No Category",
                    Priority = assignment.Task.Priority.ToString(),
                    SelectedStatus = assignment.Status
                });
            }
        }

        private void UpdateStatus()
        {
            try
            {
                if (SelectedTask == null)
                {
                    MessageBox.Show("Select a task first.");
                    return;
                }

                using var context = DbContextFactory.Create(DatabaseProvider.PostgreSql);

                var taskRepository = new TaskRepository(context);
                var taskAssignmentRepository = new TaskAssignmentRepository(context);
                var taskHistoryRepository = new TaskHistoryRepository(context);

                var taskService = new TaskService(taskRepository, taskAssignmentRepository, taskHistoryRepository);

                taskService.UpdateAssignmentStatus(new UpdateAssignmentStatusRequest
                {
                    TaskAssignmentId = SelectedTask.TaskAssignmentId,
                    NewStatus = SelectedTask.SelectedStatus,
                    ChangedByUserId = _currentUserId,
                    Note = "Status updated by user."
                });

                MessageBox.Show("Status updated successfully.");
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
