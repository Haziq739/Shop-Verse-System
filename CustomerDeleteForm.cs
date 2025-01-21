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
    public partial class CustomerDeleteForm : Form
    {
        public CustomerDeleteForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(maskedTextBox1.Text))
            {
                MessageBox.Show("Please enter a valid Customer ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(maskedTextBox1.Text, out int customerId))
            {
                MessageBox.Show("Customer ID must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            

                            // Delete from IncludesTab (if necessary)
                            string deleteIncludesQuery = "DELETE FROM IncludesTab WHERE WishListID IN (SELECT WishListID FROM WishListTab WHERE CustomerID = @CustomerID)";
                            using (SqlCommand cmd = new SqlCommand(deleteIncludesQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete from WishListTab
                            string deleteWishlistQuery = "DELETE FROM WishListTab WHERE CustomerID = @CustomerID";
                            using (SqlCommand cmd = new SqlCommand(deleteWishlistQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete from DemoGraphicTab
                            string demoGraphicQuery = "DELETE FROM DemographicsTab WHERE CustomerID = @CustomerID";
                            using (SqlCommand cmd = new SqlCommand(demoGraphicQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmd.ExecuteNonQuery();
                            }


                            // Delete from UserEnagagementTab
                            string userEngagementQuery = "DELETE FROM UserEngagementTab WHERE CustomerID = @CustomerID";
                            using (SqlCommand cmd = new SqlCommand(userEngagementQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete from CartTab
                            string deleteCartTabQuery = "DELETE FROM CartTab WHERE CustomerID = @CustomerID";
                            using (SqlCommand cmd = new SqlCommand(deleteCartTabQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmd.ExecuteNonQuery();
                            }
                            // Delete from ProductReviewsTab
                            string deleteReviewsQuery = "DELETE FROM ProductReviewsTab WHERE CustomerID = @CustomerID";
                            using (SqlCommand cmd = new SqlCommand(deleteReviewsQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete from OrderTab
                            string deleteOrdersQuery = "DELETE FROM OrderTab WHERE CustomerID = @CustomerID";
                            using (SqlCommand cmd = new SqlCommand(deleteOrdersQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmd.ExecuteNonQuery();
                            }

                            // Finally, delete from CustomersTab
                            string deleteCustomerQuery = "DELETE FROM CustomersTab WHERE CustomerID = @CustomerID";
                            using (SqlCommand cmd = new SqlCommand(deleteCustomerQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmd.ExecuteNonQuery();
                            }

                            // Commit the transaction
                            transaction.Commit();
                            MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction on error
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred during deletion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
    }

