using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Enums;

namespace TaskManagemetCore.DTOs
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        public List<int> AssignedUserIds { get; set; } = new();

        public int CreatedByUserId { get; set; }

        public TaskPriority Priority { get; set; }
    }
}
