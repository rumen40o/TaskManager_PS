namespace TaskManagerWinForms
{
    partial class LoginForm
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

        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private void InitializeComponent()
        {
            lblUsername = new Label();
            lblPassword = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            SuspendLayout();


            lblUsername.AutoSize = true;
            lblUsername.Location = new System.Drawing.Point(40, 35);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new System.Drawing.Size(63, 15);
            lblUsername.Text = "Username";


            txtUsername.Location = new System.Drawing.Point(40, 55);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(240, 23);


            lblPassword.AutoSize = true;
            lblPassword.Location = new System.Drawing.Point(40, 95);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new System.Drawing.Size(57, 15);
            lblPassword.Text = "Password";


            txtPassword.Location = new System.Drawing.Point(40, 115);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new System.Drawing.Size(240, 23);
            txtPassword.UseSystemPasswordChar = true;


            btnLogin.Location = new System.Drawing.Point(40, 160);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(240, 35);
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;


            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(330, 240);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblUsername);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login - WinForms";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}