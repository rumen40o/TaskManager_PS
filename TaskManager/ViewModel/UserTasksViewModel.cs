using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaskManagementData.Factories;
using TaskManagementData.Repositories;
using TaskManagemetCore.DTOs;

namespace TaskManager.ViewModel
{
    public class UserTasksViewModel : BaseViewModel
    {
        protected readonly int CurrentUserId;

        private UserTaskItemViewModel? _selectedTask;
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

        public UserTasksViewModel(int currentUserId)
        {
            CurrentUserId = currentUserId;
            LoadTasks();
        }

        public virtual void LoadTasks()
        {
            using var context = DbContextFactory.Create(AppSession.CurrentDatabaseProvider);

            var assignmentRepository = new TaskAssignmentRepository(context);
            var assignments = assignmentRepository.GetByUserId(CurrentUserId);

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

        public virtual void ApplyFilter(int? selectedTaskAssignmentId = null)
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

        public virtual void LoadComments()
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
    }
}