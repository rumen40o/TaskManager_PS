using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Enums;

namespace TaskManagemetCore.DTOs
{
    public class TaskSearchFilterDto
    {
        public string? Title { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority? Priority { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? CategoryId { get; set; }
    }

}
