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
using TaskManager.ViewModel;

namespace TaskManagerWinForms
{
    public partial class LoginForm : Form
    {
        private readonly AuthViewModel _loginViewModel = new AuthViewModel();

        public LoginForm()
        {
            InitializeComponent();

            btnLogin.Click += btnLogin_Click;
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please enter username and password.",
                        "Login",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var result = _loginViewModel.LoginUser(username, password);

                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Message,
                        "Login failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Login successful.",
                    "Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (result.User!.Role == UserRole.ADMIN)
                {
                    MessageBox.Show("Admin WinForms screen is not implemented yet.",
                        "Info",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                var userTasksForm = new UserTasksForm(result.User.Id);
                userTasksForm.FormClosed += (s, args) => this.Close();
                userTasksForm.Show();

                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
