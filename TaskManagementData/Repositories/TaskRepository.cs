using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                .Select(t => new TaskDashboardDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    CategoryName = t.Category.Name,

                    Status = string.Join(", ",
                        _context.TaskAssignments
                            .Where(a => a.TaskId == t.Id)
                            .Select(a => a.Status.ToString())
                            .Distinct()
                    )
                })
                .ToList();
        }
    }
}
