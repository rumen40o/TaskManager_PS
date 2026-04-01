using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Models;

namespace TaskManagemetCore.Interfaces
{
    public interface ITaskRepository
    {
        void Add(TaskItem task);

        TaskItem? GetById(int id);

        List<TaskItem> GetAll();
    }
}
