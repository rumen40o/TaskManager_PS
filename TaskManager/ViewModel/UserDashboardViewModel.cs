using System;
using System.Collections.Generic;
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
    public class UserDashboardViewModel : BaseViewModel
    {
        private UserTaskItemViewModel? _selectedTask;
        private readonly int _currentUserId;
        private string _newComment = string.Empty;
        private string _searchText = string.Empty;

        private List<UserTaskItemViewModel> _allTasks = new();

        public ObservableCollection<UserTaskItemViewModel> Tasks { get; set; } = new();
        public ObservableCollection<TaskCommentDto> Comments { get; set; } = new();

        public UserTaskItemViewModel? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
                LoadComments();
                CommandManager.InvalidateRequerySuggested();
            }
        }

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

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public Array Statuses => Enum.GetValues(typeof(AssignmentStatus));

        public ICommand RefreshTasksCommand { get; }
        public ICommand UpdateStatusCommand { get; }
        public ICommand AddCommentCommand { get; }

        public UserDashboardViewModel(int currentUserId)
        {
            _currentUserId = currentUserId;

            RefreshTasksCommand = new RelayCommand(LoadTasks);
            UpdateStatusCommand = new RelayCommand(UpdateStatus, CanUpdateStatus);
            AddCommentCommand = new RelayCommand(AddComment, CanAddComment);

            LoadTasks();
        }

        private bool CanUpdateStatus()
        {
            return SelectedTask != null;
        }

        private bool CanAddComment()
        {
            return SelectedTask != null && !string.IsNullOrWhiteSpace(NewComment);
        }

        private void LoadTasks()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

            var assignmentRepository = new TaskAssignmentRepository(context);
            var assignments = assignmentRepository.GetByUserId(_currentUserId);

            var selectedTaskAssignmentId = SelectedTask?.TaskAssignmentId;

            _allTasks = assignments.Select(assignment => new UserTaskItemViewModel
            {
                TaskAssignmentId = assignment.Id,
                Title = assignment.Task.Title,
                Description = assignment.Task.Description ?? "",
                CategoryName = assignment.Task.Category != null ? assignment.Task.Category.Name : "No Category",
                Priority = assignment.Task.Priority.ToString(),
                SelectedStatus = assignment.Status
            }).ToList();

            ApplyFilter(selectedTaskAssignmentId);
        }

        private void ApplyFilter(int? selectedTaskAssignmentId = null)
        {
            if (selectedTaskAssignmentId == null)
            {
                selectedTaskAssignmentId = SelectedTask?.TaskAssignmentId;
            }

            var filteredTasks = string.IsNullOrWhiteSpace(SearchText)
                ? _allTasks
                : _allTasks.Where(t =>
                    (!string.IsNullOrWhiteSpace(t.Title) && t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(t.Description) && t.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(t.CategoryName) && t.CategoryName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(t.Priority) && t.Priority.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    t.SelectedStatus.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            Tasks.Clear();
            foreach (var task in filteredTasks)
            {
                Tasks.Add(task);
            }

            if (selectedTaskAssignmentId.HasValue)
            {
                SelectedTask = Tasks.FirstOrDefault(t => t.TaskAssignmentId == selectedTaskAssignmentId.Value);
            }
            else if (Tasks.Count == 0)
            {
                SelectedTask = null;
            }
        }

        private void LoadComments()
        {
            Comments.Clear();

            if (SelectedTask == null)
                return;

            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);
            var commentRepository = new TaskCommentRepository(context);
            var assignmentRepository = new TaskAssignmentRepository(context);

            var assignment = assignmentRepository.GetById(SelectedTask.TaskAssignmentId);
            if (assignment == null)
                return;

            var comments = commentRepository.GetByTaskId(assignment.TaskId)
                .Select(c => new TaskCommentDto
                {
                    Username = c.User.Username,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                })
                .ToList();

            foreach (var comment in comments)
            {
                Comments.Add(comment);
            }
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

                var userExists = context.Users.Any(u => u.Id == _currentUserId);
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
                    UserId = _currentUserId,
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
                    ChangedByUserId = _currentUserId,
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