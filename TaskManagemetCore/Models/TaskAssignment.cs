using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Enums;

namespace TaskManagemetCore.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }

        public int TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public AssignmentStatus Status { get; set; }

        public DateTime AssignedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();
    }
}
