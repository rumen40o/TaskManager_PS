using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Enums;

namespace TaskManagemetCore.DTOs
{
    public class TaskSearchResultDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUsername { get; set; } = null!;
        public string? CategoryName { get; set; }
    }
}
