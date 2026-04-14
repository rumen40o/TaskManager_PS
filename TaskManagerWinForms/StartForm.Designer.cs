namespace TaskManagerWinForms
{
    partial class StartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.ComboBox cmbDatabaseProvider;
        private System.Windows.Forms.Button btnContinue;

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
        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblDatabase = new Label();
            cmbDatabaseProvider = new ComboBox();
            btnContinue = new Button();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(35, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(202, 25);
            lblTitle.Text = "Choose Database";

            // lblDatabase
            lblDatabase.AutoSize = true;
            lblDatabase.Location = new System.Drawing.Point(35, 80);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Size = new System.Drawing.Size(103, 15);
            lblDatabase.Text = "Database provider";

            // cmbDatabaseProvider
            cmbDatabaseProvider.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDatabaseProvider.FormattingEnabled = true;
            cmbDatabaseProvider.Items.AddRange(new object[] { "PostgreSQL", "SQLite" });
            cmbDatabaseProvider.Location = new System.Drawing.Point(35, 100);
            cmbDatabaseProvider.Name = "cmbDatabaseProvider";
            cmbDatabaseProvider.Size = new System.Drawing.Size(240, 23);
            cmbDatabaseProvider.SelectedIndex = 0;

            // btnContinue
            btnContinue.Location = new System.Drawing.Point(35, 145);
            btnContinue.Name = "btnContinue";
            btnContinue.Size = new System.Drawing.Size(240, 35);
            btnContinue.Text = "Continue";
            btnContinue.UseVisualStyleBackColor = true;

            // StartForm
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(320, 220);
            Controls.Add(btnContinue);
            Controls.Add(cmbDatabaseProvider);
            Controls.Add(lblDatabase);
            Controls.Add(lblTitle);
            Name = "StartForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Start - WinForms";
            ResumeLayout(false);
            PerformLayout();
        }
    

        #endregion
    }
}