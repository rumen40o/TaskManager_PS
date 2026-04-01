using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using TaskManagemetCore.Enums;

namespace TaskManagemetCore.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();

        public ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
    }
}
