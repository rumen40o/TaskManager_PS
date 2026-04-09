using System;
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
    public class UserDashboardViewModel : UserTasksViewModel
    {
        private string _newComment = string.Empty;

        public string NewComment
        {
            get => _newComment;
            set
            {
                _newComment = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public Array Statuses => Enum.GetValues(typeof(AssignmentStatus));

        public ICommand RefreshTasksCommand { get; }
        public ICommand UpdateStatusCommand { get; }
        public ICommand AddCommentCommand { get; }

        public UserDashboardViewModel(int currentUserId) : base(currentUserId)
        {
            RefreshTasksCommand = new RelayCommand(LoadTasks);
            UpdateStatusCommand = new RelayCommand(UpdateStatus, CanUpdateStatus);
            AddCommentCommand = new RelayCommand(AddComment, CanAddComment);
        }

        private bool CanUpdateStatus()
        {
            return SelectedTask != null;
        }

        private bool CanAddComment()
        {
            return SelectedTask != null && !string.IsNullOrWhiteSpace(NewComment);
        }

        private void AddComment()
        {
            try
            {
                var selectedTask = SelectedTask;
                var newComment = NewComment?.Trim();

                if (selectedTask == null)
                {
                    MessageBox.Show("Select a task first.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(newComment))
                {
                    MessageBox.Show("Comment cannot be empty.");
                    return;
                }

                using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

                var assignmentRepository = new TaskAssignmentRepository(context);
                var commentRepository = new TaskCommentRepository(context);

                var assignment = assignmentRepository.GetById(selectedTask.TaskAssignmentId);
                if (assignment == null)
                {
                    MessageBox.Show("Task assignment not found.");
                    return;
                }

                var userExists = context.Users.Any(u => u.Id == CurrentUserId);
                var taskExists = context.Tasks.Any(t => t.Id == assignment.TaskId);

                if (!userExists)
                {
                    MessageBox.Show("Current user does not exist in database.");
                    return;
                }

                if (!taskExists)
                {
                    MessageBox.Show("Selected task does not exist in database.");
                    return;
                }

                var comment = new TaskComment
                {
                    TaskId = assignment.TaskId,
                    UserId = CurrentUserId,
                    Content = newComment,
                    CreatedAt = DateTime.UtcNow
                };

                commentRepository.Add(comment);
                context.SaveChanges();

                NewComment = string.Empty;
                LoadComments();

                MessageBox.Show("Comment added successfully.");
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                var current = ex;

                while (current != null)
                {
                    sb.AppendLine(current.GetType().FullName);
                    sb.AppendLine(current.Message);
                    sb.AppendLine("--------------------------------------------------");
                    current = current.InnerException;
                }

                MessageBox.Show(sb.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateStatus()
        {
            try
            {
                var selectedTask = SelectedTask;

                if (selectedTask == null)
                {
                    MessageBox.Show("Select a task first.");
                    return;
                }

                using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

                var taskRepository = new TaskRepository(context);
                var taskAssignmentRepository = new TaskAssignmentRepository(context);
                var taskHistoryRepository = new TaskHistoryRepository(context);

                var taskService = new TaskService(taskRepository, taskAssignmentRepository, taskHistoryRepository);

                taskService.UpdateAssignmentStatus(new UpdateAssignmentStatusRequest
                {
                    TaskAssignmentId = selectedTask.TaskAssignmentId,
                    NewStatus = selectedTask.SelectedStatus,
                    ChangedByUserId = CurrentUserId,
                    Note = "Status updated by user."
                });

                MessageBox.Show("Status updated successfully.");
                LoadTasks();
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                var current = ex;

                while (current != null)
                {
                    sb.AppendLine(current.GetType().FullName);
                    sb.AppendLine(current.Message);
                    sb.AppendLine("--------------------------------------------------");
                    current = current.InnerException;
                }

                MessageBox.Show(sb.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}