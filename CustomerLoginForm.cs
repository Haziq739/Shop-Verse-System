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

namespace DB_Project
{
    public partial class CustomerLoginForm : Form
    {
        public CustomerLoginForm()
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
        { // Fetch the entered customer username (name) and password
            string enteredCustomerName = maskedTextBox1.Text.Trim(); // Customer username field
            string enteredPassword = textBox1.Text.Trim(); // Customer password field

            // Validate that the username and password are not empty
            if (string.IsNullOrEmpty(enteredCustomerName) || string.IsNullOrEmpty(enteredPassword))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the username and password match
                    string query = "SELECT CustomerID FROM CustomersTab WHERE CustomerName = @CustomerName AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@CustomerName", enteredCustomerName);
                        cmd.Parameters.AddWithValue("@Password", enteredPassword);

                        // Execute query
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            // Login successful, fetch CustomerID
                            int customerID = Convert.ToInt32(result);

                            // Store in CustomerSession
                            CustomerSession.CustomerID = customerID;
                            Console.WriteLine(CustomerSession.CustomerID);
                            CustomerSession.CustomerName = enteredCustomerName;

                            MessageBox.Show("Login successful!");

                            // Navigate to the homepage
                            homepage customerDashboard = new homepage();
                            customerDashboard.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
                            customerDashboard.Show();

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
            CustomerSignUpForm customerSignForm = new CustomerSignUpForm();
            customerSignForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            customerSignForm.Show();

            // Hide the current form (homepage)
            this.Hide();

        }
        private void AdminPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
    }
}