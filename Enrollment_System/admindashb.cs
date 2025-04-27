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
    public partial class admindashb: Form
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
        public string LoggedInAdminId { get; set; }
        public admindashb()
        {
            InitializeComponent();
            theme.ApplyTheme(this);

            string connectionString = $"Server={server}; database={database}; UID={uid}; password={password};";
            conn = new MySqlConnection(connectionString);
        }

        private void admindashb_Load(object sender, EventArgs e)
        {
            string studentId = sessionmanager.LoggedInAdminId;
            flowLayoutPanel1.Width = sidebarMinWidth;
            LoadStudentData();

        }
        private void LoadStudentData()
        {
            try
            {
                // Check if connection is already open
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();  // Close the connection if it's already open
                }

                conn.Open();

                // SQL Query to get student data along with department_name, program_name, and type_name
                string query = @"
            SELECT s.student_id, s.last_name, s.first_name, s.middle_initial, s.age, s.religion, s.gender, s.citizenship, 
                   s.birth_date, s.school_year, s.year_level, s.home_address, d.department_name, p.program_name, t.type_name
            FROM student s
            JOIN department d ON s.department_id = d.department_id
            JOIN program p ON s.program_id = p.program_id
            JOIN student_type t ON s.type_id = t.type_id
        ";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Bind the data to the DataGridView
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                // Ensure connection is closed
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
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
                admin form = new admin();
                form.Show();
                this.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminfront form = new adminfront();
            form.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            adminfront form = new adminfront();
            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.Trim();  // Text entered in the search textbox
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            try
            {
                conn.Open();

                // SQL Query with a WHERE clause to filter based on student information
                string query = @"
            SELECT s.student_id, s.last_name, s.first_name, s.middle_initial, s.age, s.religion, s.gender, s.citizenship, 
                   s.birth_date, s.school_year, s.year_level, s.home_address, d.department_name, p.program_name, t.type_name
            FROM student s
            JOIN department d ON s.department_id = d.department_id
            JOIN program p ON s.program_id = p.program_id
            JOIN student_type t ON s.type_id = t.type_id
            WHERE s.student_id LIKE @searchTerm
               OR s.last_name LIKE @searchTerm
               OR s.first_name LIKE @searchTerm
               OR s.middle_initial LIKE @searchTerm
               OR d.department_name LIKE @searchTerm
               OR p.program_name LIKE @searchTerm
               OR t.type_name LIKE @searchTerm
        ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Bind the search results to the DataGridView
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the student_id from the selected row
                int studentId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["student_id"].Value);

                try
                {
                    // Close the connection if it's already open
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    // Open connection
                    conn.Open();

                    // SQL query to delete the student record
                    string deleteQuery = "DELETE FROM student WHERE student_id = @student_id";

                    MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                    cmd.Parameters.AddWithValue("@student_id", studentId);

                    // Execute the DELETE query
                    cmd.ExecuteNonQuery();

                    // After deletion, reload the data to refresh the DataGridView
                    LoadStudentData();

                    MessageBox.Show("Student record deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting student: " + ex.Message);
                }
                finally
                {
                    // Ensure connection is closed after operations
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a student to delete.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            adminsecurity form = new adminsecurity();
            form.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the student_id of the selected row
                int selectedStudentId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["student_id"].Value);

                // Create instance of the edit form
                admineditdashb editForm = new admineditdashb();
                editForm.SelectedStudentId = selectedStudentId; // Pass the ID
                editForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please select a student to edit.");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LoadStudentData();
        }
    }
 }
 
 

