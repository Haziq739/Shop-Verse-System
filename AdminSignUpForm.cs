using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DB_Project
{
    public partial class AdminSignUpForm : Form
    {
        public AdminSignUpForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox3_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Fetch input values from the form
            string adminName = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string contactNumber = maskedTextBox2.Text.Trim();
            string password = textBox3.Text.Trim();
            string confirmPassword = maskedTextBox3.Text.Trim();

            // Validate inputs
            if (string.IsNullOrEmpty(adminName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(contactNumber) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            // Email validation using regular expression
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Please enter a valid email address.");
                return;
            }

            // Contact number validation (assuming the format 123-456-7890)
            string contactPattern = @"^\d{3}-\d{3}-\d{4}$"; // Updated to match 123-456-7890 format
            if (!Regex.IsMatch(contactNumber, contactPattern))
            {
                MessageBox.Show("Please enter a valid contact number (e.g., 123-456-7890).");
                return;
            }

            // Password validation (check if password and confirm password match)
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                // Connection string (adjust as necessary)
                string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                // Check if the admin already exists in the database
                string checkQuery = "SELECT COUNT(*) FROM AdminTab WHERE AdminName = @AdminName";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the admin name already exists
                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AdminName", adminName);

                        int adminCount = (int)cmd.ExecuteScalar();

                        if (adminCount > 0)
                        {
                            MessageBox.Show("Admin with this name already exists.");
                            return;
                        }
                    }

                    // Insert new admin into the database
                    string insertQuery = "INSERT INTO AdminTab (AdminName, EmailAddress, ContactNo, AdminPassword) " +
                                         "VALUES (@AdminName, @EmailAddress, @ContactNo, @AdminPassword)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@AdminName", adminName);
                        cmd.Parameters.AddWithValue("@EmailAddress", email);
                        cmd.Parameters.AddWithValue("@ContactNo", contactNumber);
                        cmd.Parameters.AddWithValue("@AdminPassword", password);

                        // Execute the insert query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Admin account created successfully!");
                            // Optionally, close the sign-up form and open the login page
                            this.Hide();
                            // var loginForm = new AdminLoginForm();
                            // loginForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("An error occurred while creating the account.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
    }
    

