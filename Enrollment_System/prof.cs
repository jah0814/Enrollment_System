using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Enrollment_System;
using MySql.Data.MySqlClient;


namespace Enrollment_System
{
    public partial class prof : Form
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
        public prof()
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

        private void button2_Click(object sender, EventArgs e)
        {
            security form = new security();
            form.Show();
            this.Hide();
        }

        private void prof_Load(object sender, EventArgs e)
        {
            string studentId = sessionmanager.LoggedInStudentId;
            flowLayoutPanel1.Width = sidebarMinWidth;
            if (string.IsNullOrEmpty(studentId))
            {
                MessageBox.Show("Student ID not found. Please login again.");
                this.Close();
                return;
            }

            string connStr = $"server={server};user id={uid};password={password};database={database};";
            conn = new MySqlConnection(connStr);
            {

                try
                {
                    conn.Open();

                    // Load profile labels
                    string query = @"
            SELECT 
                s.student_id,
                s.last_name,
                s.first_name,
                s.middle_initial,
                s.age,
                s.religion,
                s.gender,
                s.citizenship,
                s.birth_date,
                s.school_year,
                s.year_level,
                sec.email
            FROM student s
            LEFT JOIN security sec ON s.student_id = sec.student_id
            WHERE s.student_id = @studentId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@studentId", studentId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                label25.Text = reader["student_id"].ToString();
                                label12.Text = reader["last_name"].ToString();
                                label13.Text = reader["first_name"].ToString();
                                label14.Text = reader["middle_initial"].ToString();
                                label15.Text = reader["age"].ToString();
                                label16.Text = reader["religion"].ToString();
                                label17.Text = reader["gender"].ToString();
                                label18.Text = reader["citizenship"].ToString();
                                label19.Text = Convert.ToDateTime(reader["birth_date"]).ToString("yyyy-MM-dd");
                                label20.Text = reader["school_year"].ToString();
                                label21.Text = reader["year_level"].ToString();
                                label22.Text = reader["email"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Student data not found.");
                            }
                        }
                    }
                    string picQuery = "SELECT profile_picture FROM student WHERE student_id = @studentId";
                    using (MySqlCommand picCmd = new MySqlCommand(picQuery, conn))
                    {
                        picCmd.Parameters.AddWithValue("@studentId", studentId);
                        object result = picCmd.ExecuteScalar();

                        if (result != DBNull.Value && result != null)
                        {
                            byte[] imageBytes = (byte[])result;
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                panel7.BackgroundImage = Image.FromStream(ms);
                                panel7.BackgroundImageLayout = ImageLayout.Stretch;
                            }
                        }
                        else
                        {
                            panel7.BackgroundImage = null; 
                        }
                    }

                    // Load additional info into DataGridView
                    string gridQuery = @"
            SELECT d.department_name, p.program_name, p.major, t.type_name
            FROM student s
            JOIN department d ON s.department_id = d.department_id
            JOIN program p ON s.program_id = p.program_id
            JOIN student_type t ON s.type_id = t.type_id
            WHERE s.student_id = @studentId";

                    using (MySqlCommand gridCmd = new MySqlCommand(gridQuery, conn))
                    {
                        gridCmd.Parameters.AddWithValue("@studentId", studentId);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(gridCmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading profile: " + ex.Message);
                }
                finally
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

        private void button7_Click(object sender, EventArgs e)
        {
            editprof editForm = new editprof(
        label12.Text, // last name
        label13.Text, // first name
        label14.Text, // middle
        label15.Text, // age
        label16.Text, // religion
        label17.Text, // gender
        label18.Text, // citizenship
        label19.Text // birth date
    );

            editForm.FormClosed += (s, args) => this.prof_Load(null, null); // Refresh labels after edit
            editForm.Show();
            this.Hide();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select a Profile Picture";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Preview in panel7
                        panel7.BackgroundImage = Image.FromFile(ofd.FileName);
                        panel7.BackgroundImageLayout = ImageLayout.Stretch;

                        // Save to database
                        byte[] imageBytes;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image img = Image.FromFile(ofd.FileName);
                            img.Save(ms, img.RawFormat);
                            imageBytes = ms.ToArray();
                        }

                        string studentId = sessionmanager.LoggedInStudentId;
                        string connStr = $"server={server};user id={uid};password={password};database={database};";
                        using (MySqlConnection conn = new MySqlConnection(connStr))
                        {
                            conn.Open();
                            string query = "UPDATE student SET profile_picture = @profilePic WHERE student_id = @studentId";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@profilePic", imageBytes);
                                cmd.Parameters.AddWithValue("@studentId", studentId);
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();
                        }

                        MessageBox.Show("Profile picture updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error uploading profile picture: " + ex.Message);
                    }
                }
            }
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
