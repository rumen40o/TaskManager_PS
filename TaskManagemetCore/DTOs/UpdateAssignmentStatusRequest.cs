using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Enums;

namespace TaskManagemetCore.DTOs
{
    public class UpdateAssignmentStatusRequest
    {
        public int TaskAssignmentId { get; set; }

        public AssignmentStatus NewStatus { get; set; }

        public int ChangedByUserId { get; set; }

        public string? Note { get; set; }
    }
}
