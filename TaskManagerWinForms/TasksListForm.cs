using Microsoft.EntityFrameworkCore.Internal;
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
using TaskManagementData.Factories;
using TaskManagementData.Repositories;
using TaskManagemetCore.Enums;

namespace TaskManagerWinForms
{
    public partial class TasksListForm : Form
    {

        public TasksListForm()
        {
            InitializeComponent();
            LoadTasks();
        }

        private void LoadTasks()
        {
            try
            {
                using var context = DbContextFactory.Create(DatabaseProvider.PostgreSql);
                var taskRepository = new TaskRepository(context);

                var tasks = taskRepository.GetAll()
                    .Select(t => new
                    {
                        t.Id,
                        t.Title,
                        t.Description,
                        Priority = t.Priority.ToString(),
                        Category = t.Category != null ? t.Category.Name : "No Category"
                    })
                    .ToList();

                dgvTasks.DataSource = null;
                dgvTasks.DataSource = tasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTasks();
        }

        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
