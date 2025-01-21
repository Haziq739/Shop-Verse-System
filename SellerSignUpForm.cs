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
    public partial class SellerSignUpForm : Form
    {
        public SellerSignUpForm()
        {
            InitializeComponent();

        }

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False");

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void SellerForm_Load(object sender, EventArgs e)
        {

        }

        private void maskedTextBox3_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Retrieve inputs
            string sellerName = textBox1.Text.Trim();       // SellerName
            string email = textBox2.Text.Trim();           // EmailAddress
            string accountDetails = maskedTextBox2.Text.Trim(); // AccountDetails
            string password = textBox3.Text.Trim();        // Password
            string confirmPassword = maskedTextBox3.Text.Trim(); // ConfirmPassword
            string storeName = maskedTextBox1.Text.Trim(); // StoreName

            // Validation
            if (string.IsNullOrEmpty(sellerName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(accountDetails) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(storeName))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (email.Equals(password)) // Check if email and password are the same
            {
                MessageBox.Show("Password and email cannot be the same.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Open the connection
                con.Open();

                // Check if the account details already exist
                string checkAccountQuery = "SELECT COUNT(*) FROM SellersTab WHERE CAST(AccountDetails AS NVARCHAR(MAX)) = @AccountDetails";
                SqlCommand checkAccountCmd = new SqlCommand(checkAccountQuery, con);
                checkAccountCmd.Parameters.AddWithValue("@AccountDetails", accountDetails);

                int accountExists = (int)checkAccountCmd.ExecuteScalar();
                if (accountExists > 0)
                {
                    MessageBox.Show("Account details already in use. Please choose a different account.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if the email already exists
                string checkEmailQuery = "SELECT COUNT(*) FROM SellersTab WHERE CAST(Email AS NVARCHAR(MAX)) = @Email";
                SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, con);
                checkEmailCmd.Parameters.AddWithValue("@Email", email);

                int emailExists = (int)checkEmailCmd.ExecuteScalar();
                if (emailExists > 0)
                {
                    MessageBox.Show("Email address already in use. Please choose a different email.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Step 1: Insert data into UsersTab and retrieve UserID
                string insertUserQuery = "INSERT INTO UsersTab (UserName, UserPassword, UserRole) OUTPUT INSERTED.UserID VALUES (@UserName, @UserPassword, @UserRole)";
                SqlCommand userCmd = new SqlCommand(insertUserQuery, con);
                userCmd.Parameters.AddWithValue("@UserName", sellerName); // Using SellerName as UserName
                userCmd.Parameters.AddWithValue("@UserPassword", password);
                userCmd.Parameters.AddWithValue("@UserRole", "Seller");

                int userId = (int)userCmd.ExecuteScalar(); // Retrieve the generated UserID

                // Step 2: Insert data into SellersTab
                string insertSellerQuery = "INSERT INTO SellersTab (SellerName, Email, AccountDetails, SellerPassword, StoreName, UserID, AdminID, VerificationStatus, JoinDate) " +
                                           "VALUES (@SellerName, @Email, @AccountDetails, @SellerPassword, @StoreName, @UserID, @AdminID, @VerificationStatus, @JoinDate)";
                SqlCommand sellerCmd = new SqlCommand(insertSellerQuery, con);
                sellerCmd.Parameters.AddWithValue("@SellerName", sellerName);
                sellerCmd.Parameters.AddWithValue("@Email", email);
                sellerCmd.Parameters.AddWithValue("@AccountDetails", accountDetails);  // Corrected mapping here
                sellerCmd.Parameters.AddWithValue("@SellerPassword", password);
                sellerCmd.Parameters.AddWithValue("@StoreName", storeName);  // Corrected mapping here
                sellerCmd.Parameters.AddWithValue("@UserID", userId);
                sellerCmd.Parameters.AddWithValue("@AdminID", 1); // Assuming AdminID = 1 for all sellers initially
                sellerCmd.Parameters.AddWithValue("@VerificationStatus", "Pending"); // Default status
                sellerCmd.Parameters.AddWithValue("@JoinDate", DateTime.Now);

                sellerCmd.ExecuteNonQuery();

                MessageBox.Show("Seller signup successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    }

}

