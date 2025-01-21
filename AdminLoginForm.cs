using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DB_Project
{
    public partial class AdminLoginForm : Form
    {
        public AdminLoginForm()
        {
            InitializeComponent();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Fetch the entered admin username and password
            string enteredAdminName = maskedTextBox1.Text.Trim(); // Username field
            string enteredPassword = textBox1.Text.Trim(); // Password field

            // Validate that the username and password are not empty
            if (string.IsNullOrEmpty(enteredAdminName) || string.IsNullOrEmpty(enteredPassword))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

           

            try
            {

                string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
                // Create the SQL query to check if the entered admin name and password match any record in the Admin table
                string query = "SELECT COUNT(*) FROM AdminTab WHERE AdminName = @AdminName AND AdminPassword = @AdminPassword";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Execute the query to check the count of matching records
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@AdminName", enteredAdminName);
                        cmd.Parameters.AddWithValue("@AdminPassword", enteredPassword);

                        // Get the result of the query
                        int adminCount = (int)cmd.ExecuteScalar();

                        if (adminCount > 0)
                        {
                            // Admin found and password matches, proceed with login
                            MessageBox.Show("Login successful!");
                            // You can now proceed to the admin's main page, for example:
                            // var adminDashboard = new AdminDashboardForm();
                            // adminDashboard.Show();
                            // Open the MyCart form
                            AdminPage adminDashboard = new AdminPage();
                            adminDashboard.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
                            adminDashboard.Show();

                            this.Hide(); // Hide login form
                        }
                        else
                        {
                            // Invalid login (either username or password is incorrect)
                            MessageBox.Show("Invalid username or password. Please try again.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL errors
                MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle general errors
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            AdminSignUpForm adminSignForm = new AdminSignUpForm();
            adminSignForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            adminSignForm.Show();

            // Hide the current form (homepage)
            this.Hide();

        }
        private void AdminPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
    }
    }

