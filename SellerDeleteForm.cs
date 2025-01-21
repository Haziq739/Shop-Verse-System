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
    public partial class SellerDeleteForm : Form
    {
        public SellerDeleteForm()
        {
            InitializeComponent();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Fetch the Seller ID from the MaskedTextBox
            string sellerID = maskedTextBox1.Text.Trim();

            if (string.IsNullOrEmpty(sellerID))
            {
                MessageBox.Show("Please enter a valid Seller ID.");
                return;
            }

            // Connection string to your SQL Server database
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // SQL query to delete the seller from the database
                    string query = "DELETE FROM SellersTab WHERE SellerID = @SellerID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Adding the SellerID parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@SellerID", sellerID);

                        // Execute the deletion query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Seller and all related data deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Seller not found. Please check the Seller ID.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    
}
    }

