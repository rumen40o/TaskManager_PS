using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Enums;

namespace TaskManagemetCore.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public UserRole? Role { get; set; }
    }
}
