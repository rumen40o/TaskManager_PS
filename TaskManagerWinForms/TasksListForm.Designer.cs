namespace TaskManagerWinForms
{
    partial class TasksListForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.Button btnRefresh;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dgvTasks = new DataGridView();
            btnRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvTasks).BeginInit();
            SuspendLayout();
            
            dgvTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTasks.Location = new Point(10, 9);
            dgvTasks.Margin = new Padding(3, 2, 3, 2);
            dgvTasks.Name = "dgvTasks";
            dgvTasks.RowHeadersWidth = 51;
            dgvTasks.RowTemplate.Height = 29;
            dgvTasks.Size = new Size(665, 270);
            dgvTasks.TabIndex = 0;
            dgvTasks.CellContentClick += dgvTasks_CellContentClick;
            
            
            btnRefresh.Location = new Point(566, 292);
            btnRefresh.Margin = new Padding(3, 2, 3, 2);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(109, 26);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(686, 331);
            Controls.Add(btnRefresh);
            Controls.Add(dgvTasks);
            Margin = new Padding(3, 2, 3, 2);
            Name = "TasksListForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tasks List - WinForms";
            ((System.ComponentModel.ISupportInitialize)dgvTasks).EndInit();
            ResumeLayout(false);
        }
    }
}