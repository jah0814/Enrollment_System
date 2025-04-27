using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Enrollment_System
{
    public partial class Form1: Form
    {
        string server = "localhost";
        string uid = "root";
        string password = "jaaaahz023";
        string database = "enrollment";

        MySqlConnection conn;
        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection($"server={server};user id={uid};password={password};database={database};");
            textBox2.PasswordChar = '●';

            // Add event handler for checkbox checked change
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {
            admin form = new admin();
            form.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            signup form = new signup();
            form.Show();
        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text.Trim();       // Email or Student ID
            string password = textBox2.Text.Trim();    // Password

            try
            {
                conn.Open(); // Open the connection

                string query = @"
    SELECT sec.student_id 
    FROM security sec
    JOIN student s ON sec.student_id = s.student_id 
    WHERE (sec.email = @input OR s.student_id = @input) 
    AND sec.password = @pass";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@input", input);
                    cmd.Parameters.AddWithValue("@pass", password);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MessageBox.Show("Student login successful!");

                            // ✅ Store student ID globally
                            sessionmanager.LoggedInStudentId = reader["student_id"].ToString();

                            // ✅ Go to studfront
                            studfront frontForm = new studfront();
                            frontForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid email/student ID or password.");
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
                conn.Close(); // Always close the connection
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
    }
}
