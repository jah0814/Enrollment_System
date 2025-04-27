using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Enrollment_System;


namespace Enrollment_System
{
    public partial class studfront: Form
    {
        bool sidebarExpanded = true;
        int sidebarMaxWidth = 200;
        int sidebarMinWidth = 40;
        int sidebarSpeed = 20;
        public string LoggedInStudentId { get; set; }




        public studfront()
        {
            InitializeComponent();
            theme.ApplyTheme(this);
        }
        
        private void studfront_Load(object sender, EventArgs e)
        {
            string studentId = sessionmanager.LoggedInStudentId;
            flowLayoutPanel1.Width = sidebarMinWidth;    
        }

        private void button1_Click(object sender, EventArgs e)
        {

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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            dashb dashboard = new dashb();
            dashboard.LoggedInStudentId = this.LoggedInStudentId; // Pass the LoggedInStudentId here
            dashboard.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            security form = new security();
            form.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            theme.ToggleTheme();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prof form = new prof();
            form.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

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
    }
}
