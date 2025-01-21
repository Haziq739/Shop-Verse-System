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
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox8_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Connection string
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

            // Query to insert a product
            string insertQuery = @"
        INSERT INTO ProductsTab (ProductID, ProductName, Description, Price, CreationDate, StockQuantity, Brand, SKU, ImageURL,CategoryID) 
        VALUES (@ProductID, @ProductName, @Description, @Price, @CreationDate, @Quantity, @Brand, @SKU, @ImageURL,@CategoryID)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create the SQL command
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // Add parameters with the values from the text boxes
                        command.Parameters.AddWithValue("@ProductID", maskedTextBox1.Text); // Assuming textBox1 is for Product ID
                        command.Parameters.AddWithValue("@ProductName", maskedTextBox2.Text); // Assuming textBox2 is for Product Name
                        command.Parameters.AddWithValue("@Description", maskedTextBox3.Text);
                        command.Parameters.AddWithValue("@Price", decimal.Parse(maskedTextBox4.Text));
                        command.Parameters.AddWithValue("@CreationDate", DateTime.Parse(maskedTextBox5.Text));
                        command.Parameters.AddWithValue("@Quantity", int.Parse(maskedTextBox6.Text));
                        command.Parameters.AddWithValue("@Brand", maskedTextBox7.Text);
                        command.Parameters.AddWithValue("@SKU", maskedTextBox8.Text);
                        command.Parameters.AddWithValue("@ImageURL", maskedTextBox9.Text);
                        command.Parameters.AddWithValue("@CategoryID", maskedTextBox10.Text);
                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product inserted successfully!");
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Handle duplicate key error (assuming ProductID is unique)
                    if (ex.Number == 2627) // SQL Server error code for unique constraint violation
                    {
                        MessageBox.Show("Error: A product with this ID already exists.");
                    }
                    else
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox3_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox4_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox5_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox6_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox7_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox9_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox10_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
