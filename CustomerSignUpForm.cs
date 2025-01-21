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
//using Microsoft.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace DB_Project
{
    public partial class CustomerSignUpForm : Form
    {
        public CustomerSignUpForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False");




        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Set up form title
            this.Text = "Customer Sign Up Form";

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string fullName = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string userName = maskedTextBox2.Text.Trim();
            string password = textBox3.Text.Trim();
            string confirmPassword = maskedTextBox3.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                con.Open();

                // Step 1: Insert data into UsersTab and retrieve UserID
                string insertUserQuery = "INSERT INTO UsersTab (UserName, UserPassword, UserRole) OUTPUT INSERTED.UserID VALUES (@UserName, @UserPassword, @UserRole)";
                SqlCommand userCmd = new SqlCommand(insertUserQuery, con);
                userCmd.Parameters.AddWithValue("@UserName", userName);
                userCmd.Parameters.AddWithValue("@UserPassword", password);
                userCmd.Parameters.AddWithValue("@UserRole", "Customer");

                int userId = (int)userCmd.ExecuteScalar(); // Retrieve the generated UserID

                // Step 2: Insert data into CustomersTab
                string insertCustomerQuery = "INSERT INTO CustomersTab (CustomerName, Email, UserID, AdminID, RegistrationDate, Password) " +
                                             "VALUES (@CustomerName, @Email, @UserID, @AdminID, @RegistrationDate, @Password)";
                SqlCommand customerCmd = new SqlCommand(insertCustomerQuery, con);
                customerCmd.Parameters.AddWithValue("@CustomerName", fullName);
                customerCmd.Parameters.AddWithValue("@Email", email);
                customerCmd.Parameters.AddWithValue("@UserID", userId);
                customerCmd.Parameters.AddWithValue("@AdminID", 1);
                customerCmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                customerCmd.Parameters.AddWithValue("@Password", password); // Adding Password here

                customerCmd.ExecuteNonQuery();

                MessageBox.Show("Sign up successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
