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
    public class TaskHistoryRepository : ITaskHistoryRepository
    {
        private readonly AppDbContext _context;

        public TaskHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(TaskHistory history)
        {
            _context.TaskHistories.Add(history);
            _context.SaveChanges();
        }
    }
}
