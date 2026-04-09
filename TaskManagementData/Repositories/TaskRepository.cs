using Microsoft.EntityFrameworkCore;
using TaskManagementData.Contexts;
using TaskManagemetCore.DTOs;
using TaskManagemetCore.Interfaces;
using TaskManagemetCore.Models;

namespace TaskManagementData.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(TaskItem task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public TaskItem? GetById(int id)
        {
            return _context.Tasks
                .Include(t => t.Category)
                .Include(t => t.CreatedByUser)
                .FirstOrDefault(t => t.Id == id);
        }

        public List<TaskItem> GetAll()
        {
            return _context.Tasks
                .Include(t => t.Category)
                .Include(t => t.CreatedByUser)
                .ToList();
        }

        public void Delete(TaskItem task)
        {
            _context.Tasks.Remove(task);
        }

        public void Update(TaskItem task)
        {
            _context.Tasks.Update(task);
        }

        public List<TaskDashboardDto> GetAllForDashboard()
        {
            return _context.Tasks
                .Include(t => t.Category)
                .Include(t => t.CreatedByUser)
                .Include(t => t.TaskAssignments)
                .Select(t => new TaskDashboardDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description ?? "",
                    Priority = t.Priority,
                    CategoryName = t.Category != null ? t.Category.Name : "No Category",
                    Status = t.TaskAssignments.Any()
                        ? t.TaskAssignments.Select(a => a.Status.ToString()).FirstOrDefault() ?? "No Status"
                        : "No Status",
                    DueDate = t.DueDate,
                    CreatedAt = t.CreatedAt,
                    CreatedByUsername = t.CreatedByUser != null ? t.CreatedByUser.Username : ""
                })
                .ToList();
        }

        public List<TaskDashboardDto> SearchForDashboard(TaskSearchFilterDto filter)
        {
            var query = _context.Tasks
                .Include(t => t.Category)
                .Include(t => t.CreatedByUser)
                .Include(t => t.TaskAssignments)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(t => t.Title.Contains(filter.Title));
            }

            if (filter.DueDate.HasValue)
            {
                var start = DateTime.SpecifyKind(filter.DueDate.Value.Date, DateTimeKind.Utc);
                var end = start.AddDays(1);

                query = query.Where(t =>
                    t.DueDate.HasValue &&
                    t.DueDate.Value >= start &&
                    t.DueDate.Value < end);
            }

            if (filter.Priority.HasValue)
            {
                query = query.Where(t => t.Priority == filter.Priority.Value);
            }

            if (filter.CreatedAt.HasValue)
            {
                var start = DateTime.SpecifyKind(filter.CreatedAt.Value.Date, DateTimeKind.Utc);
                var end = start.AddDays(1);

                query = query.Where(t => t.CreatedAt >= start && t.CreatedAt < end);
            }

            if (filter.CreatedByUserId.HasValue)
            {
                query = query.Where(t => t.CreatedByUserId == filter.CreatedByUserId.Value);
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == filter.CategoryId.Value);
            }

            return query
                .Select(t => new TaskDashboardDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description ?? "",
                    Priority = t.Priority,
                    CategoryName = t.Category != null ? t.Category.Name : "No Category",
                    DueDate = t.DueDate,
                    CreatedAt = t.CreatedAt,
                    CreatedByUsername = t.CreatedByUser != null ? t.CreatedByUser.Username : "",
                    Status = t.TaskAssignments.Any()
                        ? t.TaskAssignments
                            .OrderByDescending(a => a.UpdatedAt)
                            .Select(a => a.Status.ToString())
                            .FirstOrDefault() ?? "No Status"
                        : "No Status"
                })
                .ToList();
        }
    }
}