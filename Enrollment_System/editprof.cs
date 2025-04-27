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
    public partial class editprof: Form
    {
        string studentId = sessionmanager.LoggedInStudentId;
        public editprof(string lastName, string firstName, string middle, string age,
                    string religion, string gender, string citizenship, string birthDate
                    )
        {
            InitializeComponent();
            theme.ApplyTheme(this);

            textBox1.Text = lastName;
            textBox2.Text = firstName;
            textBox3.Text = middle;
            textBox4.Text = age;
            textBox5.Text = religion;
            textBox6.Text = gender;
            textBox7.Text = citizenship;
            textBox8.Text = birthDate;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string connStr = $"server=localhost;user id=root;password=jaaaahz023;database=enrollment;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string updateStudent = @"
            UPDATE student
            SET last_name = @ln, first_name = @fn, middle_initial = @mi,
                age = @age, religion = @rel, gender = @gen,
                citizenship = @cit, birth_date = @bd
            WHERE student_id = @id";

                    

                    using (MySqlCommand cmd1 = new MySqlCommand(updateStudent, conn))
                    {
                        cmd1.Parameters.AddWithValue("@ln", textBox1.Text); // last_name
                        cmd1.Parameters.AddWithValue("@fn", textBox2.Text); // first_name
                        cmd1.Parameters.AddWithValue("@mi", textBox3.Text); // middle_initial
                        cmd1.Parameters.AddWithValue("@age", textBox4.Text); // age
                        cmd1.Parameters.AddWithValue("@rel", textBox5.Text); // religion
                        cmd1.Parameters.AddWithValue("@gen", textBox6.Text); // gender
                        cmd1.Parameters.AddWithValue("@cit", textBox7.Text); // citizenship
                        cmd1.Parameters.AddWithValue("@bd", textBox8.Text); // birth_date
                        cmd1.Parameters.AddWithValue("@id", sessionmanager.LoggedInStudentId);
                        cmd1.ExecuteNonQuery();
                    }

                    

                    MessageBox.Show("Profile updated successfully.");

                    // Go back to profile form
                    prof profileForm = new prof();
                    profileForm.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update failed: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            prof profileForm = new prof();
            profileForm.Show();
            this.Close();
        }
    }
}
