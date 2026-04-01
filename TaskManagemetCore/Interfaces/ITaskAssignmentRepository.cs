using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Models;

namespace TaskManagemetCore.Interfaces
{
    public interface ITaskAssignmentRepository
    {
        void Add(TaskAssignment assignment);

        List<TaskAssignment> GetByUserId(int userId);

        TaskAssignment? GetById(int id);

        void Update(TaskAssignment assignment);
    }
}
