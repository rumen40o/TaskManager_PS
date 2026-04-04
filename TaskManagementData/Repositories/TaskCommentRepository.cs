using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementData.Contexts;
using TaskManagemetCore.Models;

namespace TaskManagementData.Repositories
{
    public class TaskCommentRepository
    {
        private readonly AppDbContext _context;

        public TaskCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<TaskComment> GetByTaskId(int taskId)
        {
            return _context.TaskComments
                .Include(c => c.User)
                .Where(c => c.TaskId == taskId)
                .OrderBy(c => c.CreatedAt)
                .ToList();
        }

        public void Add(TaskComment comment)
        {
            _context.TaskComments.Add(comment);
        }
    }
}
