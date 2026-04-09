using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using TaskManager.ViewModel;

namespace TaskManagerWinForms
{
    public partial class UserTasksForm : Form
    {
        private readonly UserTasksViewModel _viewModel;

        public UserTasksForm(int userId)
        {
            InitializeComponent();

            _viewModel = new UserTasksViewModel(userId);

            LoadTasksToGrid();
        }

        private void LoadTasksToGrid()
        {
            dgvTasks.DataSource = null;

            dgvTasks.DataSource = _viewModel.Tasks
                .Select(t => new
                {
                    t.TaskAssignmentId,
                    t.Title,
                    t.Description,
                    t.CategoryName,
                    t.Priority,
                    Status = t.SelectedStatus.ToString()
                })
                .ToList();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _viewModel.LoadTasks();
            LoadTasksToGrid();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _viewModel.SearchText = txtSearch.Text;
            LoadTasksToGrid();
        }

        private void dgvTasks_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTasks.CurrentRow == null)
                return;

            var id = (int)dgvTasks.CurrentRow.Cells["TaskAssignmentId"].Value;

            _viewModel.SelectedTask =
                _viewModel.Tasks.FirstOrDefault(t => t.TaskAssignmentId == id);
        }
    }
}
