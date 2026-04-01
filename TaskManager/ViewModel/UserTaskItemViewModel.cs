using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Enums;

namespace TaskManager.ViewModel
{
    public class UserTaskItemViewModel : BaseViewModel
    {
        private AssignmentStatus _selectedStatus;

        public int TaskAssignmentId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public AssignmentStatus SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
            }
        }
    }
}
