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
    public partial class admin: Form
    {
        string server = "localhost";
        string uid = "root";
        string password = "jaaaahz023";
        string database = "enrollment";

        MySqlConnection conn;

        public admin()
        {
            InitializeComponent();
            conn = new MySqlConnection($"server={server};user id={uid};password={password};database={database};");
            textBox2.PasswordChar = '●';

            // Add event handler for checkbox checked change
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text.Trim(); // Admin ID or Admin Name
            string pass = textBox2.Text.Trim();  // Admin password

            try
            {
                conn.Open();

                string query = @"
        SELECT a.admin_id 
        FROM security s
        JOIN admin a ON s.admin_id = a.admin_id
        WHERE (a.admin_id = @input OR a.admin_name = @input)
        AND s.password = @pass";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@input", input);
                    cmd.Parameters.AddWithValue("@pass", pass);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MessageBox.Show("Admin login successful!");

                            // Store the logged-in admin's ID globally
                            sessionmanager.LoggedInAdminId = reader["admin_id"].ToString();

                            // Now you can navigate to the admin dashboard or home screen
                            adminfront dash = new adminfront();
                            dash.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid admin ID/name or password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login failed: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Show the password as plain text
                textBox2.PasswordChar = '\0'; // No masking (plain text)
            }
            else
            {
                // Hide the password with '*' or any other masking character
                textBox2.PasswordChar = '●'; // Masked password
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            addadmin form = new addadmin();
            form.Show();
            this.Hide();
        }
    }
}

