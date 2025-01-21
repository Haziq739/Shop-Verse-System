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
    public partial class SellerLoginForm : Form
    {
        public SellerLoginForm()
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
            // Fetch the entered seller username (name) and password
            string enteredSellerName = maskedTextBox1.Text.Trim(); // Seller username field
            string enteredPassword = textBox1.Text.Trim(); // Seller password field

            // Validate that the username and password are not empty
            if (string.IsNullOrEmpty(enteredSellerName) || string.IsNullOrEmpty(enteredPassword))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                // Define the connection string (adjust as needed)
                string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                // Create the SQL query to check if the entered seller name and password match any record in the Sellers table
                string query = "SELECT COUNT(*) FROM SellersTab WHERE SellerName = @SellerName AND SellerPassword = @SellerPassword";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Execute the query to check the count of matching records
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@SellerName", enteredSellerName);
                        cmd.Parameters.AddWithValue("@SellerPassword", enteredPassword); // Password parameter

                        // Get the result of the query
                        int sellerCount = (int)cmd.ExecuteScalar();

                        if (sellerCount > 0)
                        {
                            // Seller found and password matches, proceed with login
                            MessageBox.Show("Login successful!");

                            Seller sellerboard = new Seller();
                            sellerboard.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
                            sellerboard.Show();

                            // Hide the current form (homepage)
                            this.Hide();
                            // You can now proceed to the seller's main page, for example:
                            // var sellerDashboard = new SellerDashboardForm();
                            // sellerDashboard.Show();
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
            SellerSignUpForm SellSignForm = new SellerSignUpForm();
            SellSignForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            SellSignForm.Show();

            // Hide the current form (homepage)
            this.Hide();

        }
        private void AdminPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
    }
    }

