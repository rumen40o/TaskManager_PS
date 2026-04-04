using Microsoft.EntityFrameworkCore.Storage;
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
using TaskManagemetCore.Enums;

namespace TaskManager.View
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (DatabaseProviderComboBox.SelectedIndex == 0)
            {
                AppSession.CurrentDatabaseProvider = DatabaseProvider.PostgreSql;
            }
            else
            {
                AppSession.CurrentDatabaseProvider = DatabaseProvider.Sqlite;
            }

            var loginWindow = new LoginWindow();
            loginWindow.Show();

            Close();
        }
    }
}
