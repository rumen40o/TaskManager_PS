using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementData.Contexts;
using TaskManagemetCore.Interfaces;
using TaskManagemetCore.Models;

namespace TaskManagementData.Repositories
{
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly AppDbContext _context;

        public TaskAssignmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(TaskAssignment assignment)
        {
            _context.TaskAssignments.Add(assignment);
            _context.SaveChanges();
        }

        public List<TaskAssignment> GetByUserId(int userId)
        {
            return _context.TaskAssignments
                .Include(a => a.Task)
                    .ThenInclude(t => t.Category)
                .Include(a => a.Task)
                    .ThenInclude(t => t.CreatedByUser)
                .Where(a => a.UserId == userId)
                .ToList();
        }

        public TaskAssignment? GetById(int id)
        {
            return _context.TaskAssignments
                .Include(a => a.Task)
                .FirstOrDefault(a => a.Id == id);
        }

        public void Update(TaskAssignment assignment)
        {
            _context.TaskAssignments.Update(assignment);
            _context.SaveChanges();
        }
    }
}
