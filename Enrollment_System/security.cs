using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Enrollment_System;
using MySql.Data.MySqlClient;


namespace Enrollment_System
{
    public partial class security: Form
    {
        string server = "localhost";
        string uid = "root";
        string password = "jaaaahz023";
        string database = "enrollment";
        MySqlConnection conn;

        bool sidebarExpanded = true;
        int sidebarMaxWidth = 200;
        int sidebarMinWidth = 40;
        int sidebarSpeed = 20;
        public security()
        {
            InitializeComponent();
            theme.ApplyTheme(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            studfront form = new studfront();
            form.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dashb form = new dashb();
            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prof form = new prof();
            form.Show();
            this.Hide();
        }

        private void security_Load(object sender, EventArgs e)
        {
            string studentId = sessionmanager.LoggedInStudentId;
            flowLayoutPanel1.Width = sidebarMinWidth;
        }

        private void menuclick_Click(object sender, EventArgs e)
        {
            sidebar.Start();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            theme.ToggleTheme();
        }

        private void sidebar_Tick(object sender, EventArgs e)
        {
            if (sidebarExpanded)
            {
                // Collapse the sidebar
                flowLayoutPanel1.Width -= sidebarSpeed;
                if (flowLayoutPanel1.Width <= sidebarMinWidth)
                {
                    flowLayoutPanel1.Width = sidebarMinWidth; // Set to min width to ensure no negative width
                    sidebar.Stop();
                    sidebarExpanded = false;
                }
            }
            else
            {
                // Expand the sidebar
                flowLayoutPanel1.Width += sidebarSpeed;
                if (flowLayoutPanel1.Width >= sidebarMaxWidth)
                {
                    flowLayoutPanel1.Width = sidebarMaxWidth; // Set to max width to avoid overshooting
                    sidebar.Stop();
                    sidebarExpanded = true;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form1 form = new Form1();
                form.Show();
                this.Hide();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            studfront form = new studfront();
            form.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string recentEmail = textBox1.Text.Trim();
            string newEmail = textBox3.Text.Trim();
            string recentPassword = textBox2.Text.Trim();
            string newPassword = textBox4.Text.Trim();
            string studentId = sessionmanager.LoggedInStudentId;

            if (string.IsNullOrEmpty(studentId))
            {
                MessageBox.Show("Student ID not found. Please login again.");
                return;
            }

            string connStr = $"server={server};user id={uid};password={password};database={database};";
            conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();

                // Check if recent email and password match
                string checkQuery = @"
            SELECT * FROM security 
            WHERE student_id = @id AND email = @recentEmail AND password = @recentPassword";

                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id", studentId);
                    checkCmd.Parameters.AddWithValue("@recentEmail", recentEmail);
                    checkCmd.Parameters.AddWithValue("@recentPassword", recentPassword);

                    using (MySqlDataReader reader = checkCmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Incorrect recent email or password.");
                            return;
                        }
                    }
                }

                // Ask for confirmation before updating
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to update your email and password?",
                    "Confirm Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return; // User canceled the update
                }

                // Update with new email and password
                string updateQuery = @"
            UPDATE security 
            SET email = @newEmail, password = @newPassword 
            WHERE student_id = @id";

                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@newEmail", newEmail);
                    updateCmd.Parameters.AddWithValue("@newPassword", newPassword);
                    updateCmd.Parameters.AddWithValue("@id", studentId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Security details updated successfully.");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Update failed. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}

