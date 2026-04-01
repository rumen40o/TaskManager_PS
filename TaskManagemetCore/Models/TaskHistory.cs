using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Enums;

namespace TaskManagemetCore.Models
{
    public class TaskHistory
    {
        public int Id { get; set; }

        public int TaskAssignmentId { get; set; }
        public TaskAssignment TaskAssignment { get; set; } = null!;

        public int ChangedByUserId { get; set; }
        public User ChangedByUser { get; set; } = null!;

        public AssignmentStatus OldStatus { get; set; }

        public AssignmentStatus NewStatus { get; set; }

        public DateTime ChangedAt { get; set; }

        public string? Note { get; set; }
    }
}
