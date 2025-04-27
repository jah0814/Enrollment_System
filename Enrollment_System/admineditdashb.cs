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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Enrollment_System
{
    public partial class admineditdashb : Form
    {
        string server = "localhost";
        string uid = "root";
        string password = "jaaaahz023";
        string database = "enrollment";
        MySqlConnection conn;

        public int SelectedStudentId { get; set; }


        public admineditdashb()
        {
            InitializeComponent();
            theme.ApplyTheme(this);
            this.Load += admineditdashb_Load;
        }
    

        private void button1_Click(object sender, EventArgs e)
        {
            admindashb form = new admindashb();
            form.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = @"UPDATE student 
                         SET last_name = @last_name, first_name = @first_name, middle_initial = @middle_initial,
                             age = @age, religion = @religion, gender = @gender, citizenship = @citizenship,
                             birth_date = @birth_date, school_year = @school_year, 
                             year_level = @year_level, home_address = @home_address,
                             department_id = @department_id, program_id = @program_id, type_id = @type_id
                         WHERE student_id = @student_id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@last_name", textBox1.Text);
                cmd.Parameters.AddWithValue("@first_name", textBox2.Text);
                cmd.Parameters.AddWithValue("@middle_initial", textBox3.Text);
                cmd.Parameters.AddWithValue("@age", textBox4.Text);
                cmd.Parameters.AddWithValue("@religion", textBox5.Text);
                cmd.Parameters.AddWithValue("@gender", textBox6.Text);
                cmd.Parameters.AddWithValue("@citizenship", textBox7.Text);
                cmd.Parameters.AddWithValue("@birth_date", textBox8.Text);
                cmd.Parameters.AddWithValue("@school_year", textBox9.Text);
                cmd.Parameters.AddWithValue("@year_level", textBox10.Text);
                cmd.Parameters.AddWithValue("@home_address", textBox11.Text);
                cmd.Parameters.AddWithValue("@department_id", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("@program_id", comboBox2.SelectedValue);
                cmd.Parameters.AddWithValue("@type_id", comboBox3.SelectedValue);
                cmd.Parameters.AddWithValue("@student_id", SelectedStudentId);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Student information updated successfully.");
                admindashb form = new admindashb();
                form.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating student: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    

        private void admineditdashb_Load(object sender, EventArgs e)
        {
            string connectionString = $"Server={server}; database={database}; UID={uid}; password={password};";
            conn = new MySqlConnection(connectionString);

            PopulateDropdowns();
            LoadStudentDetails();
        }

        private void PopulateDropdowns()
        {
            try
            {
                conn.Open();

                // Populate Department ComboBox
                string deptQuery = "SELECT department_id, department_name FROM department";
                MySqlDataAdapter deptAdapter = new MySqlDataAdapter(deptQuery, conn);
                DataTable deptTable = new DataTable();
                deptAdapter.Fill(deptTable);
                comboBox1.DataSource = deptTable;
                comboBox1.DisplayMember = "department_name";
                comboBox1.ValueMember = "department_id";

                // Populate Program ComboBox
                string progQuery = "SELECT program_id, program_name FROM program";
                MySqlDataAdapter progAdapter = new MySqlDataAdapter(progQuery, conn);
                DataTable progTable = new DataTable();
                progAdapter.Fill(progTable);
                comboBox2.DataSource = progTable;
                comboBox2.DisplayMember = "program_name";
                comboBox2.ValueMember = "program_id";

                // Populate Type ComboBox
                string typeQuery = "SELECT type_id, type_name FROM student_type";
                MySqlDataAdapter typeAdapter = new MySqlDataAdapter(typeQuery, conn);
                DataTable typeTable = new DataTable();
                typeAdapter.Fill(typeTable);
                comboBox3.DataSource = typeTable;
                comboBox3.DisplayMember = "type_name";
                comboBox3.ValueMember = "type_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dropdowns: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void LoadStudentDetails()
        {
            try
            {
                conn.Open();
                string query = @"SELECT last_name, first_name, middle_initial, age, religion, gender, citizenship, 
                         birth_date, school_year, year_level, home_address,
                         department_id, program_id, type_id
                         FROM student
                         WHERE student_id = @student_id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@student_id", SelectedStudentId);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    textBox1.Text = reader["last_name"].ToString();
                    textBox2.Text = reader["first_name"].ToString();
                    textBox3.Text = reader["middle_initial"].ToString();
                    textBox4.Text = reader["age"].ToString();
                    textBox5.Text = reader["religion"].ToString();
                    textBox6.Text = reader["gender"].ToString();
                    textBox7.Text = reader["citizenship"].ToString();
                    textBox8.Text = Convert.ToDateTime(reader["birth_date"]).ToString("yyyy-MM-dd");
                    textBox9.Text = reader["school_year"].ToString();
                    textBox10.Text = reader["year_level"].ToString();
                    textBox11.Text = reader["home_address"].ToString();

                    comboBox1.SelectedValue = reader["department_id"];
                    comboBox2.SelectedValue = reader["program_id"];
                    comboBox3.SelectedValue = reader["type_id"];
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student details: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
 }

