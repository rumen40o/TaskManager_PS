using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagemetCore.Enums;
using TaskManager;

namespace TaskManagerWinForms
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();

            btnContinue.Click += btnContinue_Click;
        }

        private void btnContinue_Click(object? sender, EventArgs e)
        {
            if (cmbDatabaseProvider.SelectedIndex == 0)
            {
                AppSession.CurrentDatabaseProvider = DatabaseProvider.PostgreSql;
            }
            else
            {
                AppSession.CurrentDatabaseProvider = DatabaseProvider.Sqlite;
            }

            var loginForm = new LoginForm();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();

            this.Hide();
        }
    }
}
