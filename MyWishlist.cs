using DB_Project;
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
    public partial class MyWishlist : Form
    {
        public MyWishlist()
        {
            InitializeComponent();
        }



        private void MyWishlist_Load(object sender, EventArgs e)
        {
            LoadProductImages();
        }

        private void LoadProductImages()
        {
            int customerID = CustomerSession.CustomerID;
            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False;MultipleActiveResultSets=True";

            // SQL query to get ProductID from WishListTab and their corresponding ImageURL from ProductsTab
            string query = @"
        SELECT p.ProductID, p.ProductName, p.ImageURL 
        FROM WishListTab w
        INNER JOIN ProductsTab p ON w.ProductID = p.ProductID
        WHERE w.CustomerID = @CustomerID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerID", customerID);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string imageName = reader["ImageURL"].ToString() + ".jpg"; // Image name with extension
                        int productId = Convert.ToInt32(reader["ProductID"]);
                        string productName = reader["ProductName"].ToString();

                        // Create a Panel for each product
                        Panel productPanel = new Panel
                        {
                            Width = flowLayoutPanel1.Width - 30, // Adjust width for better alignment
                            Height = 120,
                            BorderStyle = BorderStyle.FixedSingle,
                            Padding = new Padding(5)
                        };

                        // Create PictureBox for the product image
                        PictureBox productPictureBox = new PictureBox
                        {
                            Size = new Size(100, 100),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Margin = new Padding(5),
                            BorderStyle = BorderStyle.FixedSingle,
                            Cursor = Cursors.Hand,
                            Tag = productId // Store product ID for reference
                        };

                        string imagesFolderPath = Application.StartupPath + "\\Images"; // Path to the Images folder
                        string imagePath = System.IO.Path.Combine(imagesFolderPath, imageName);

                        try
                        {
                            if (System.IO.File.Exists(imagePath))
                            {
                                productPictureBox.Image = Image.FromFile(imagePath);
                            }
                            else
                            {
                                productPictureBox.Image = GeneratePlaceholderImage();
                            }
                        }
                        catch
                        {
                            productPictureBox.Image = GeneratePlaceholderImage();
                        }

                        // Add click event to PictureBox
                        productPictureBox.Click += (s, e) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        // Create Remove Button (optional, for removing from wishlist)
                        Button btnRemove = new Button
                        {
                            Text = "Remove",
                            Font = new Font("Arial", 8, FontStyle.Regular),
                            Width = 60,
                            Height = 30,
                            Location = new Point(110, 40),
                            BackColor = Color.LightCoral,
                            ForeColor = Color.White
                        };

                        btnRemove.Click += (s, e) => RemoveFromWishlist(customerID, productId);

                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(btnRemove);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading wishlist products: {ex.Message}");
            }
        }

        private void RemoveFromWishlist(int customerID, int productID)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

            string deleteQuery = "DELETE FROM WishListTab WHERE CustomerID = @CustomerID AND ProductID = @ProductID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", customerID);
                        command.Parameters.AddWithValue("@ProductID", productID);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Product removed from wishlist!");

                        MyWishlist mywishlistForm = new MyWishlist();
                        mywishlistForm.Show();
                        // Hide the current form (homepage)
                        this.Hide();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing product from wishlist: {ex.Message}");
            }
        }

        private void PictureBox_Click(int productId, string productName)
        {
            MessageBox.Show($"Clicked Product: {productName} (ID: {productId})");
        }

        private Image GeneratePlaceholderImage()
        {
            Bitmap placeholder = new Bitmap(120, 120); // Create a blank image
            using (Graphics g = Graphics.FromImage(placeholder))
            {
                g.Clear(Color.Gray); // Fill with gray color
                g.DrawString("No Image", new Font("Arial", 12), Brushes.White, new PointF(10, 50));
            }
            return placeholder;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            homepage homepageForm = new homepage();
            homepageForm.Show();
            // Hide the current form (homepage)
            this.Hide();

            //panel2.Visible = !panel2.Visible; // Toggle visibility
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyCart mycartForm = new MyCart();
            mycartForm.Show();
            // Hide the current form (homepage)
            this.Hide();
            //panel2.Visible = !panel2.Visible; // Toggle visibility
        }
        private void button4_Click(object sender, EventArgs e)
        {
            MyOrder myorderForm = new MyOrder();
            myorderForm.Show();
            // Hide the current form (homepage)
            this.Hide();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
