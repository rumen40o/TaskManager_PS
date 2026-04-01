using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TaskManagemetCore.DTOs;
using TaskManagemetCore.Enums;
using TaskManagemetCore.Interfaces;
using TaskManagemetCore.Models;

namespace TaskManagemetCore.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;
        private readonly ITaskHistoryRepository _taskHistoryRepository;

        public TaskService(
            ITaskRepository taskRepository,
            ITaskAssignmentRepository taskAssignmentRepository,
            ITaskHistoryRepository taskHistoryRepository)
        {
            _taskRepository = taskRepository;
            _taskAssignmentRepository = taskAssignmentRepository;
            _taskHistoryRepository = taskHistoryRepository;
        }

        public void CreateTask(CreateTaskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new Exception("Title is required.");
            }

            if (request.AssignedUserIds == null || request.AssignedUserIds.Count == 0)
            {
                throw new Exception("At least one user must be selected.");
            }

            var task = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                CategoryId = request.CategoryId,
                CreatedByUserId = request.CreatedByUserId,
                CreatedAt = DateTime.UtcNow,
                Priority = TaskPriority.MEDIUM
            };

            _taskRepository.Add(task);

            foreach (var userId in request.AssignedUserIds)
            {
                var assignment = new TaskAssignment
                {
                    TaskId = task.Id,
                    UserId = userId,
                    Status = AssignmentStatus.NEW,
                    AssignedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _taskAssignmentRepository.Add(assignment);
            }
        }

        public void UpdateAssignmentStatus(UpdateAssignmentStatusRequest request)
        {
            var assignment = _taskAssignmentRepository.GetById(request.TaskAssignmentId);

            if (assignment == null)
            {
                throw new Exception("Assignment not found.");
            }

            var oldStatus = assignment.Status;

            if (!IsValidStatusTransition(oldStatus, request.NewStatus))
            {
                throw new Exception($"Invalid status transition: {oldStatus} -> {request.NewStatus}");
            }

            assignment.Status = request.NewStatus;
            assignment.UpdatedAt = DateTime.UtcNow;

            _taskAssignmentRepository.Update(assignment);

            var history = new TaskHistory
            {
                TaskAssignmentId = assignment.Id,
                ChangedByUserId = request.ChangedByUserId,
                OldStatus = oldStatus,
                NewStatus = request.NewStatus,
                ChangedAt = DateTime.UtcNow,
                Note = request.Note
            };

            _taskHistoryRepository.Add(history);
        }

        private bool IsValidStatusTransition(AssignmentStatus oldStatus, AssignmentStatus newStatus)
        {
            if (oldStatus == newStatus)
                return true;

            return oldStatus switch
            {
                AssignmentStatus.NEW =>
                    newStatus == AssignmentStatus.InProgres ||
                    newStatus == AssignmentStatus.Cancelled,

                AssignmentStatus.InProgres =>
                    newStatus == AssignmentStatus.DONE ||
                    newStatus == AssignmentStatus.Cancelled,

                AssignmentStatus.DONE => false,

                AssignmentStatus.Cancelled => false,

                _ => false
            };
        }
    }
}
