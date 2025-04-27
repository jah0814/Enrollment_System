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
    public partial class dashb : Form
    {
        string server = "localhost";
        string uid = "root";
        string password = "jaaaahz023";
        string database = "enrollment";
        MySqlConnection conn;
        public string LoggedInStudentId { get; set; }


        bool isEnrolled = false;

        bool sidebarExpanded = true;
        int sidebarMaxWidth = 200;
        int sidebarMinWidth = 40;
        int sidebarSpeed = 20;
        Dictionary<string, int> semesterMap = new Dictionary<string, int>();
        public dashb()
        {
            InitializeComponent();
            theme.ApplyTheme(this);
            conn = new MySqlConnection($"server={server};user id={uid};password={password};database={database};");
        }

        private void dashb_Load(object sender, EventArgs e)
        {
            string studentId = sessionmanager.LoggedInStudentId;
            flowLayoutPanel1.Width = sidebarMinWidth;
            

            LoadDepartments();
            LoadStudentTypes();
            LoadSemesters();
            CheckIfAlreadyEnrolled(studentId);
        }

        private void LoadDepartments()
        {
            string deptQuery = "SELECT department_name FROM department";
            try
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(deptQuery, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string departmentName = reader["department_name"].ToString();
                        if (!comboBox2.Items.Contains(departmentName))
                        {
                            comboBox2.Items.Add(departmentName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading departments: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Load student types
        private void LoadStudentTypes()
        {
            try
            {
                conn.Open();
                string query = "SELECT type_name FROM student_type";
                comboBox1.Items.Clear();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string typeName = reader["type_name"].ToString();
                        if (!comboBox1.Items.Contains(typeName))
                        {
                            comboBox1.Items.Add(typeName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student types: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Load semesters into comboBox6 and map semester names to IDs
        private void LoadSemesters()
        {
            try
            {
                conn.Open();
                string semesterQuery = "SELECT semester_id, semester_name FROM semester";

                using (MySqlCommand cmd = new MySqlCommand(semesterQuery, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    comboBox6.Items.Clear();
                    semesterMap.Clear();

                    while (reader.Read())
                    {
                        string semesterName = reader["semester_name"].ToString();
                        int semesterId = Convert.ToInt32(reader["semester_id"]);
                        comboBox6.Items.Add(semesterName);
                        semesterMap[semesterName] = semesterId;  // Store mapping of name to ID
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading semesters: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Check if the student is already enrolled
        private void CheckIfAlreadyEnrolled(string studentId)
        {
            try
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM student WHERE student_id = @student_id AND department_id IS NOT NULL";
                using (MySqlCommand cmd = new MySqlCommand(checkQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@student_id", studentId);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        isEnrolled = true;
                        MessageBox.Show("You are already enrolled. You cannot enroll again.");
                        DisableEnrollmentFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking enrollment: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Disable fields if already enrolled
        private void DisableEnrollmentFields()
        {
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            comboBox6.Enabled = false;
            button6.Enabled = false;
        }

        private void menuclick_Click(object sender, EventArgs e)
        {
            sidebar.Start();
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            theme.ToggleTheme();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            studfront form = new studfront();
            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prof form = new prof();
            form.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            security form = new security();
            form.Show();
            this.Hide();
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
            string studentId = sessionmanager.LoggedInStudentId;
            if (isEnrolled)
            {
                MessageBox.Show("You cannot modify your enrollment after submitting.");
                return;
            }
            if (string.IsNullOrEmpty(studentId))
            {
                MessageBox.Show("Student ID is not available. Please log in again.");
                return;
            }

            try
            {
                conn.Open();

                // Check if the student is already enrolled
                string checkQuery = "SELECT COUNT(*) FROM student WHERE student_id = @student_id AND department_id IS NOT NULL";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@student_id", studentId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Enrollment data already exists for this student. You cannot enroll again.");
                        return;
                    }
                }

                // Get the semester_id based on the selected semester_name from comboBox6
                string semesterQuery = @"
            SELECT semester_id 
            FROM semester 
            WHERE semester_name = @semesterName 
            LIMIT 1";

                int semesterId = 0;
                using (MySqlCommand semesterCmd = new MySqlCommand(semesterQuery, conn))
                {
                    semesterCmd.Parameters.AddWithValue("@semesterName", comboBox6.Text); // Using semester_name from comboBox6
                    object result = semesterCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        semesterId = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("No semester found for the selected semester name.");
                        return;
                    }
                }

                // Update student enrollment
                string updateQuery = @"
            UPDATE student 
            SET 
                department_id = (SELECT department_id FROM department WHERE department_name = @departmentName LIMIT 1),
                program_id = (SELECT program_id FROM program WHERE program_name = @programName LIMIT 1),
                type_id = (SELECT type_id FROM student_type WHERE type_name = @studentType LIMIT 1),
                program_id = (SELECT program_id FROM program WHERE major = @major AND department_id = (SELECT department_id FROM department WHERE department_name = @departmentName) LIMIT 1),
                year_level = @yearLevel
            WHERE student_id = @studentId";
                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@departmentName", comboBox2.Text);
                    updateCmd.Parameters.AddWithValue("@programName", comboBox3.Text);
                    updateCmd.Parameters.AddWithValue("@studentType", comboBox1.Text);
                    updateCmd.Parameters.AddWithValue("@semesterName", semesterId);
                    updateCmd.Parameters.AddWithValue("@major", comboBox4.Text);
                    updateCmd.Parameters.AddWithValue("@yearLevel", comboBox5.Text);
                    updateCmd.Parameters.AddWithValue("@studentId", studentId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Enrolled successfully!");
                        DisableEnrollmentFields();
                        isEnrolled = true;
                    }
                    else
                    {
                        MessageBox.Show("Failed to enroll.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            string selectedDepartment = comboBox2.SelectedItem.ToString();
            string query = "SELECT program_name FROM program WHERE department_id = (SELECT department_id FROM department WHERE department_name = @department_name LIMIT 1)";

            try
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@department_name", selectedDepartment);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string programName = reader["program_name"].ToString();
                            if (!comboBox3.Items.Contains(programName))
                            {
                                comboBox3.Items.Add(programName);
                            }
                        }

                        if (comboBox3.Items.Count == 0)
                        {
                            MessageBox.Show("No programs found for the selected department.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading programs: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            string selectedProgram = comboBox3.SelectedItem.ToString();
            string query = "SELECT DISTINCT major FROM program WHERE program_name = @program_name";

            try
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@program_name", selectedProgram);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox4.Items.Add(reader["major"].ToString());
                        }

                        if (comboBox4.Items.Count == 0)
                        {
                            MessageBox.Show("No majors found for the selected program.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading majors: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void LoadCoursesForStudent(string studentId)
        {
            try
            {
                conn.Open();

                // Get the program ID for the student
                string programQuery = "SELECT program_id FROM student WHERE student_id = @student_id";
                int? programId = null;

                using (MySqlCommand programCmd = new MySqlCommand(programQuery, conn))
                {
                    programCmd.Parameters.AddWithValue("@student_id", studentId);
                    object result = programCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        programId = Convert.ToInt32(result);
                    }
                }

                if (programId.HasValue)
                {
                    // Fetch courses based on the program ID
                    string courseQuery = "SELECT subject_code, subject_name, units FROM course WHERE program_id = @program_id";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(courseQuery, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@program_id", programId.Value);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dt;

                        // Ensure custom columns are shown
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            // Display only necessary columns based on your requirements
                            column.Visible = (column.Name == "subject_code" || column.Name == "subject_name" || column.Name == "units");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No program assigned to this student.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading courses: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }





        private void button7_Click(object sender, EventArgs e)
        {
            LoadCoursesForStudent(sessionmanager.LoggedInStudentId);
        }
    }
}

