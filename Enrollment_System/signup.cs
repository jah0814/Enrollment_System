using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Enrollment_System
{
    public partial class signup: Form
    {
        string server = "localhost";
        string uid = "root";
        string password = "jaaaahz023";
        string database = "enrollment";
        MySqlConnection conn;
        public signup()
        {
            InitializeComponent();
            conn = new MySqlConnection($"server={server};user id={uid};password={password};database={database};");
        }

        private void signup_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Use default names for textboxes
            string lastName = textBox1.Text.Trim();  // Last name input
            string firstName = textBox2.Text.Trim(); // First name input
            string middleInitial = textBox3.Text.Trim(); // Middle initial input
            string ageText = textBox4.Text.Trim(); // Age input
            string religion = comboBox1.SelectedItem.ToString(); // Religion from ComboBox
            string gender = comboBox2.SelectedItem.ToString(); // Gender from ComboBox
            string citizenship = textBox5.Text.Trim(); // Citizenship input
            DateTime birthDate = dateTimePicker1.Value; // Birth date from DateTimePicker
            string schoolYear = comboBox3.SelectedItem.ToString(); // School year from ComboBox
            string yearLevel = comboBox4.SelectedItem.ToString(); // Year level from ComboBox
            string homeAddress = textBox6.Text.Trim(); // Home address input
            string email = textBox7.Text.Trim(); // Email input
            string password = textBox8.Text.Trim(); // Password input

            // Validate input fields
            if (string.IsNullOrEmpty(lastName) || !Regex.IsMatch(lastName, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("Last name can only contain letters.");
                return;
            }

            if (string.IsNullOrEmpty(firstName) || !Regex.IsMatch(firstName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("First name can only contain letters and spaces.");
                return;
            }


            if (string.IsNullOrEmpty(middleInitial) || !Regex.IsMatch(middleInitial, @"^[a-zA-Z]$"))
            {
                MessageBox.Show("Middle initial must be a single letter.");
                return;
            }

            if (string.IsNullOrEmpty(ageText) || !int.TryParse(ageText, out int age) || age <= 0)
            {
                MessageBox.Show("Age must be a valid number.");
                return;
            }

            if (string.IsNullOrEmpty(citizenship) || !Regex.IsMatch(citizenship, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Citizenship can only contain letters.");
                return;
            }

            if (string.IsNullOrEmpty(schoolYear) || !Regex.IsMatch(schoolYear, @"^\d{4}-\d{4}$"))
            {
                MessageBox.Show("School year must be in the format 'YYYY-YYYY'.");
                return;
            }

            if (string.IsNullOrEmpty(yearLevel) || !Regex.IsMatch(yearLevel, @"^[a-zA-Z0-9\s]+$"))
            {
                MessageBox.Show("Year level can only contain letters and numbers.");
                return;
            }

            if (string.IsNullOrEmpty(homeAddress))
            {
                MessageBox.Show("Home address cannot be empty.");
                return;
            }

            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Please enter a valid email address.");
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

                // Step 1: Insert student data (auto-increment student_id)
                string studentQuery = @"
                    INSERT INTO student (last_name, first_name, middle_initial, age, religion, gender, citizenship, birth_date, school_year, year_level, home_address)
                    VALUES (@last_name, @first_name, @middle_initial, @age, @religion, @gender, @citizenship, @birth_date, @school_year, @year_level, @home_address);";

                MySqlCommand studentCmd = new MySqlCommand(studentQuery, conn, transaction);
                studentCmd.Parameters.AddWithValue("@last_name", lastName);
                studentCmd.Parameters.AddWithValue("@first_name", firstName);
                studentCmd.Parameters.AddWithValue("@middle_initial", middleInitial);
                studentCmd.Parameters.AddWithValue("@age", age);
                studentCmd.Parameters.AddWithValue("@religion", religion);
                studentCmd.Parameters.AddWithValue("@gender", gender);
                studentCmd.Parameters.AddWithValue("@citizenship", citizenship);
                studentCmd.Parameters.AddWithValue("@birth_date", birthDate);
                studentCmd.Parameters.AddWithValue("@school_year", schoolYear);
                studentCmd.Parameters.AddWithValue("@year_level", yearLevel);
                studentCmd.Parameters.AddWithValue("@home_address", homeAddress);

                studentCmd.ExecuteNonQuery();

                // Step 2: Get the last inserted student_id using LAST_INSERT_ID()
                long studentId = studentCmd.LastInsertedId;

                // Step 3: Insert security data with student_id as FK
                string securityQuery = @"
                    INSERT INTO security (student_id, email, password)
                    VALUES (@student_id, @email, @password);";

                MySqlCommand securityCmd = new MySqlCommand(securityQuery, conn, transaction);
                securityCmd.Parameters.AddWithValue("@student_id", studentId);  // Using the auto-generated student_id
                securityCmd.Parameters.AddWithValue("@email", email);
                securityCmd.Parameters.AddWithValue("@password", password);

                securityCmd.ExecuteNonQuery();

                // Commit the transaction
                transaction.Commit();

                MessageBox.Show("Student registered successfully!");

                // Open Form1 (assuming this is your login form)
                Form1 loginForm = new Form1();
                loginForm.Show();

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


        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
