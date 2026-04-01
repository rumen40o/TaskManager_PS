using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.ViewModel;


namespace TaskManager
{
    public class SelectableUserItem : BaseViewModel
    {
        private bool _isSelected;

        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}
