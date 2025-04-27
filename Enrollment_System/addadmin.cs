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
    public partial class addadmin: Form
    {
        string server = "localhost";
        string uid = "root";
        string password = "jaaaahz023";
        string database = "enrollment";
        MySqlConnection conn;
        public addadmin()
        {
            InitializeComponent();
            conn = new MySqlConnection($"server={server};user id={uid};password={password};database={database};");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            admin form = new admin();
            form.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string adminName = textBox1.Text.Trim();  // Admin name input
            string password = textBox2.Text.Trim();  // Admin password input

            // Validate input fields
            if (string.IsNullOrEmpty(adminName))
            {
                MessageBox.Show("Admin name cannot be empty.");
                return;
            }

            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.");
                return;
            }

            try
            {
                conn.Open();

                // Start a transaction to ensure both insertions succeed
                MySqlTransaction transaction = conn.BeginTransaction();

                // Step 1: Insert admin data (auto-increment admin_id)
                string adminQuery = @"
                    INSERT INTO admin (admin_name) 
                    VALUES (@admin_name);";

                MySqlCommand adminCmd = new MySqlCommand(adminQuery, conn, transaction);
                adminCmd.Parameters.AddWithValue("@admin_name", adminName);

                adminCmd.ExecuteNonQuery();

                // Step 2: Get the last inserted admin_id using LAST_INSERT_ID()
                long adminId = adminCmd.LastInsertedId;

                // Step 3: Insert security data with admin_id as FK
                string securityQuery = @"
                    INSERT INTO security (admin_id, password)
                    VALUES (@admin_id, @password);";

                MySqlCommand securityCmd = new MySqlCommand(securityQuery, conn, transaction);
                securityCmd.Parameters.AddWithValue("@admin_id", adminId);  // Using the auto-generated admin_id
                securityCmd.Parameters.AddWithValue("@password", password);

                securityCmd.ExecuteNonQuery();

                // Commit the transaction
                transaction.Commit();

                MessageBox.Show("Admin registered successfully!");

                // Optionally, navigate to the admin login page or dashboard
                admin form = new admin(); // Replace with your actual admin login form
                form.Show();

                // Optionally, hide the current sign-up form
                this.Hide();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration failed: " + ex.Message);
            }
            finally
            {
                conn.Close(); // Ensure connection is closed after use
            }
        }

    }
}

