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
            this.dgvTasks = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();

            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            this.SuspendLayout();


            this.txtSearch.Location = new System.Drawing.Point(12, 12);
            this.txtSearch.Size = new System.Drawing.Size(300, 23);
            this.txtSearch.Name = "txtSearch";


            this.dgvTasks.Location = new System.Drawing.Point(12, 50);
            this.dgvTasks.Size = new System.Drawing.Size(760, 320);
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;


            this.btnRefresh.Location = new System.Drawing.Point(660, 380);
            this.btnRefresh.Size = new System.Drawing.Size(112, 30);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;

            this.ClientSize = new System.Drawing.Size(800, 420);
            this.Controls.Add(this.dgvTasks);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.txtSearch);
            this.Text = "User Tasks";

            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}