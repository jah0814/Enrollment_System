using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_System
{
    public partial class adminsecurity: Form
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
        public adminsecurity()
        {
            InitializeComponent();
            theme.ApplyTheme(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            adminfront form = new adminfront();
            form.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                admin form = new admin();
                form.Show();
                this.Hide();
            }
        }

        private void menuclick_Click(object sender, EventArgs e)
        {
            sidebar.Start();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            theme.ToggleTheme();
        }

        private void adminsecurity_Load(object sender, EventArgs e)
        {
            string studentId = sessionmanager.LoggedInAdminId;
            flowLayoutPanel1.Width = sidebarMinWidth;
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

        private void button4_Click(object sender, EventArgs e)
        {
            admindashb form = new admindashb();
            form.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminfront form = new adminfront();
            form.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string recentUsername = textBox1.Text.Trim();
            string newUsername = textBox3.Text.Trim();
            string recentPassword = textBox2.Text.Trim();
            string newPassword = textBox4.Text.Trim();
            string adminId = sessionmanager.LoggedInAdminId;

            if (string.IsNullOrEmpty(adminId))
            {
                MessageBox.Show("Admin ID not found. Please login again.");
                return;
            }

            string connStr = $"server={server};user id={uid};password={password};database={database};";
            conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();

                // Check if the current admin name and password are correct
                string checkQuery = @"
            SELECT * FROM admin 
            JOIN security ON admin.admin_id = security.admin_id
            WHERE admin.admin_id = @id AND admin.admin_name = @recentName AND security.password = @recentPassword";

                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id", adminId);
                    checkCmd.Parameters.AddWithValue("@recentName", recentUsername);
                    checkCmd.Parameters.AddWithValue("@recentPassword", recentPassword);

                    using (MySqlDataReader reader = checkCmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Incorrect current admin name or password.");
                            return;
                        }
                    }
                }

                // Ask for confirmation to update the admin credentials
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to update your username and password?",
                    "Confirm Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Update the admin name in the admin table and password in the security table
                string updateQuery = @"
            UPDATE admin 
            SET admin_name = @newName 
            WHERE admin_id = @id;
            
            UPDATE security 
            SET password = @newPassword 
            WHERE admin_id = @id";

                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@newName", newUsername);
                    updateCmd.Parameters.AddWithValue("@newPassword", newPassword);
                    updateCmd.Parameters.AddWithValue("@id", adminId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Admin credentials updated successfully.");
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

