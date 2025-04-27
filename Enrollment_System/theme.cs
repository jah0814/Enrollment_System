using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_System
{
    public static class theme
    {
        public static bool IsDarkMode { get; private set; } = false;

        public static void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            ApplyThemeToAllOpenForms();
        }

        public static void ApplyThemeToAllOpenForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                ApplyTheme(form);
            }
        }

        public static void ApplyTheme(Form form)
        {
            Color backColor = IsDarkMode ? Color.FromArgb(45, 45, 48) : Color.White;
            Color foreColor = IsDarkMode ? Color.White : Color.Black;

            form.BackColor = backColor;
            form.ForeColor = foreColor;

            foreach (Control control in form.Controls)
            {
                ApplyThemeToControl(control);
            }
        }

        private static void ApplyThemeToControl(Control control)
        {
            if (control.Tag != null && control.Tag.ToString() == "no-theme")
                return;

            if (IsDarkMode)
            {
                if (!IsDarkStyled(control.BackColor))
                    control.BackColor = Color.FromArgb(45, 45, 48); // Softer dark mode

                control.ForeColor = Color.White;

                if (control is Label)
                    control.BackColor = Color.Transparent;

                if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.BackColor = Color.FromArgb(70, 70, 74);
                    button.ForeColor = Color.White;
                    button.FlatAppearance.BorderColor = Color.Gray;
                }

                if (control is DataGridView dgv)
                {
                    dgv.BackgroundColor = Color.FromArgb(45, 45, 48);
                    dgv.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
                    dgv.DefaultCellStyle.ForeColor = Color.White;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
                    dgv.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgv.EnableHeadersVisualStyles = false;
                }
            }
            else
            {
                control.BackColor = Color.White;
                control.ForeColor = Color.Black;

                if (control is Label)
                    control.BackColor = Color.Transparent;

                if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Standard;
                    button.BackColor = SystemColors.Control;
                    button.ForeColor = Color.Black;
                }

                if (control is DataGridView dgv)
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.DefaultCellStyle.BackColor = Color.White;
                    dgv.DefaultCellStyle.ForeColor = Color.Black;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                    dgv.RowHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                    dgv.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
                    dgv.EnableHeadersVisualStyles = true;
                }
            }

            foreach (Control child in control.Controls)
            {
                ApplyThemeToControl(child);
            }
        }

        private static bool IsDarkStyled(Color color)
        {
            return color.R < 60 && color.G < 60 && color.B < 60;
        }
    }
}

