using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Enums;

namespace TaskManagemetCore.DTOs
{
    public class TaskDashboardDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public TaskPriority Priority { get; set; }
        public string CategoryName { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
