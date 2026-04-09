using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskManager.ViewModel;

namespace TaskManager.View
{
    /// <summary>
    /// Interaction logic for AdminUsersView.xaml
    /// </summary>
    public partial class AdminUsersView : Window
    {
        public AdminUsersView()
        {
            InitializeComponent();
            DataContext = new AdminUsersViewModel();
        }
    }
}
