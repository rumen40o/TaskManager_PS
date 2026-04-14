namespace TaskManagerWinForms
{
    partial class UserTasksForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 
        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtSearch;
        private void InitializeComponent()
        {
            dgvTasks = new DataGridView();
            btnRefresh = new Button();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvTasks).BeginInit();
            SuspendLayout();
            // 
            // dgvTasks
            // 
            dgvTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTasks.Location = new Point(12, 50);
            dgvTasks.Name = "dgvTasks";
            dgvTasks.Size = new Size(760, 320);
            dgvTasks.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(660, 380);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(112, 30);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(300, 23);
            txtSearch.TabIndex = 2;
            // 
            // UserTasksForm
            // 
            ClientSize = new Size(800, 420);
            Controls.Add(dgvTasks);
            Controls.Add(btnRefresh);
            Controls.Add(txtSearch);
            Name = "UserTasksForm";
            Text = "User Tasks";
            ((System.ComponentModel.ISupportInitialize)dgvTasks).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}