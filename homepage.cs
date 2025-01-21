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
using DB_Project;



namespace DB_Project
{


    public partial class homepage : Form
    {
        public homepage()
        {
            InitializeComponent();

        }

        private void homepage_Load(object sender, EventArgs e)
        {
            LoadProductImages();
        }

        private void LoadProductImages()
        {
            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL FROM Productstab";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
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

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, e) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }
        }
        private int GetNewWishListID(string connectionString)
        {
            string query = "SELECT ISNULL(MAX(WishListID), 0) + 1 FROM WishListTab";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating new WishListID: {ex.Message}");
                return 0; // Default fallback
            }
        }
        private void buttonMyCart_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                // Step 1: Insert into CartTab
                int customerID = CustomerSession.CustomerID; // Replace with dynamic customer ID
                int cartID = GetNewCartID(connectionString);
                DateTime createdDate = DateTime.Now;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert into CartTab
                    string insertCartQuery = "INSERT INTO CartTab (CartID, CustomerID, CreatedDate) VALUES (@CartID, @CustomerID, @CreationDate)";
                    using (SqlCommand cartCommand = new SqlCommand(insertCartQuery, connection))
                    {
                        cartCommand.Parameters.AddWithValue("@CartID", cartID);
                        cartCommand.Parameters.AddWithValue("@CustomerID", customerID);
                        cartCommand.Parameters.AddWithValue("@CreationDate", createdDate);

                        cartCommand.ExecuteNonQuery();
                    }

                    // Step 2: Insert into CartItemTab

                    foreach (Control control in flowLayoutPanel1.Controls)
                    {
                        if (control is Panel productPanel)
                        {
                            // Retrieve the product details from the Panel
                            var productPictureBox = productPanel.Controls.OfType<PictureBox>().FirstOrDefault();
                            var nudQuantity = productPanel.Controls.OfType<NumericUpDown>().FirstOrDefault();

                            if (productPictureBox != null && nudQuantity != null)
                            {
                                int productID = Convert.ToInt32(productPictureBox.Tag);
                                int quantity = (int)nudQuantity.Value;

                                // Insert into CartItemTab
                                string insertCartItemQuery = "INSERT INTO CartItemTab (CartID, ProductID, Quantity) VALUES (@CartID, @ProductID, @Quantity)";
                                using (SqlCommand cartItemCommand = new SqlCommand(insertCartItemQuery, connection))
                                {
                                    cartItemCommand.Parameters.AddWithValue("@CartID", cartID);
                                    cartItemCommand.Parameters.AddWithValue("@ProductID", productID);
                                    cartItemCommand.Parameters.AddWithValue("@Quantity", quantity);
                                    if (quantity <= 0)
                                    {
                                        // MessageBox.Show($"Invalid quantity for ProductID: {productID}. Quantity must be at least 1.");
                                        continue; // Skip inserting this product
                                    }
                                    else
                                    {
                                        cartItemCommand.ExecuteNonQuery();
                                    }
                                }

                            }
                        }
                    }
                }

                MessageBox.Show("Cart saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving cart: {ex.Message}");
            }


            // Open the MyCart form
            MyCart myCartForm = new MyCart();
            myCartForm.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private int GetNewCartID(string connectionString)
        {
            string query = "SELECT ISNULL(MAX(CartID), 0) + 1 FROM CartTab";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating new CartID: {ex.Message}");
                return 0; // Default fallback
            }
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



        private void PictureBox_Click(int productId, string productName)
        {
            MessageBox.Show($"Clicked Product: {productName} (ID: {productId})");
        }

        // Existing button click events remain unchanged...
        private void button1_Click(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e)
        {


        }
        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void button7_Click(object sender, EventArgs e)
        {


            ///////////////////////////////////////////////////////////////////
            ///


            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL FROM Productstab where CategoryID=4";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
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
                        productPictureBox.Click += (s, ee) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, ee) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }


            //////////////////////////////////////////////////////////////////




        }
        private void button13_Click(object sender, EventArgs e)
        {


            /*electronics myelectronicform = new electronics();
            myelectronicform.Show();

            // Hide the current form (homepage)
            this.Hide();*/
            ///////////////////////////////////////////////////////////////////
            ///


            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL FROM Productstab where CategoryID=10";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
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
                        productPictureBox.Click += (s, ee) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, ee) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }


            //////////////////////////////////////////////////////////////////




        }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void button6_Click(object sender, EventArgs e)
        {
            /*electronics myelectronicform = new electronics();
            myelectronicform.Show();

            // Hide the current form (homepage)
            this.Hide();*/
            ///////////////////////////////////////////////////////////////////
            ///


            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL FROM Productstab where CategoryID=1";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
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
                        productPictureBox.Click += (s, ee) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, ee) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }


            //////////////////////////////////////////////////////////////////


        }



        private void button9_Click(object sender, EventArgs e)
        {
            /*electronics myelectronicform = new electronics();
            myelectronicform.Show();

            // Hide the current form (homepage)
            this.Hide();*/
            ///////////////////////////////////////////////////////////////////
            ///


            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL FROM Productstab where CategoryID=2";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
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
                        productPictureBox.Click += (s, ee) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, ee) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }


            //////////////////////////////////////////////////////////////////


        }


        private void button8_Click(object sender, EventArgs e)
        {
            /*electronics myelectronicform = new electronics();
            myelectronicform.Show();

            // Hide the current form (homepage)
            this.Hide();*/
            ///////////////////////////////////////////////////////////////////
            ///


            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL FROM Productstab where CategoryID=3";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
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
                        productPictureBox.Click += (s, ee) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, ee) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }


            //////////////////////////////////////////////////////////////////


        }


        private void button10_Click(object sender, EventArgs e)
        {
            /*electronics myelectronicform = new electronics();
            myelectronicform.Show();

            // Hide the current form (homepage)
            this.Hide();*/
            ///////////////////////////////////////////////////////////////////
            ///


            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL FROM Productstab where CategoryID=5";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
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
                        productPictureBox.Click += (s, ee) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, ee) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }


            //////////////////////////////////////////////////////////////////


        }


        private void button11_Click(object sender, EventArgs e)
        {
            /*electronics myelectronicform = new electronics();
            myelectronicform.Show();

            // Hide the current form (homepage)
            this.Hide();*/
            ///////////////////////////////////////////////////////////////////
            ///


            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL FROM Productstab where CategoryID=6";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
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
                        productPictureBox.Click += (s, ee) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, ee) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }


            //////////////////////////////////////////////////////////////////


        }

        private void button12_Click(object sender, EventArgs e)
        {
            /*electronics myelectronicform = new electronics();
            myelectronicform.Show();

            // Hide the current form (homepage)
            this.Hide();*/
            ///////////////////////////////////////////////////////////////////
            ///


            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL FROM Productstab where CategoryID=7";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
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
                        productPictureBox.Click += (s, ee) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, ee) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }


            //////////////////////////////////////////////////////////////////


        }


        private void button14_Click(object sender, EventArgs e)
        {
            /*electronics myelectronicform = new electronics();
            myelectronicform.Show();

            // Hide the current form (homepage)
            this.Hide();*/
            ///////////////////////////////////////////////////////////////////
            ///


            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL FROM Productstab where CategoryID=8";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
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
                        productPictureBox.Click += (s, ee) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, ee) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }


            //////////////////////////////////////////////////////////////////


        }


        private void button15_Click(object sender, EventArgs e)
        {
            /*electronics myelectronicform = new electronics();
            myelectronicform.Show();

            // Hide the current form (homepage)
            this.Hide();*/
            ///////////////////////////////////////////////////////////////////
            ///


            // Clear existing controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = "SELECT ProductID, ProductName, ImageURL, Price,Description FROM Productstab where CategoryID=9";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string imageName = reader["ImageURL"].ToString() + ".jpg"; // Image name with extension
                        int productId = Convert.ToInt32(reader["ProductID"]);
                        string productName = reader["ProductName"].ToString();
                        string price = reader["Price"].ToString();
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
                        productPictureBox.Click += (s, ee) => PictureBox_Click(productId, productName);

                        // Create Label for product name
                        Label lblProductName = new Label
                        {
                            Text = productName,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(110, 10)
                        };

                        Label lblPrice = new Label
                        {
                            Text = price,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(220, 10)
                        };

                        // Create NumericUpDown for quantity selection
                        NumericUpDown nudQuantity = new NumericUpDown
                        {
                            Minimum = 0,
                            Maximum = 10,
                            Value = 0,
                            Location = new Point(110, 40)
                        };

                        // Create Wishlist Button
                        Button btnWishlist = new Button
                        {
                            Text = "❤",
                            ForeColor = Color.Red,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Width = 40,
                            Height = 40,
                            Location = new Point(300, 40)
                        };
                        /*
                        btnWishlist.Click += (s, e) =>
                        {
                            //query to add in wishlist table

                            MessageBox.Show($"{productName} added to wishlist!");
                        };*/

                        btnWishlist.Click += (s, ee) =>
                        {
                            try
                            {
                                // Define connection string
                                string connectionString1 = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";

                                // Define the SQL INSERT query
                                string insertQuery = "INSERT INTO WishListTab (WishListID, CustomerID, ProductID, DateAdded) " +
                                                     "VALUES (@WishListID, @CustomerID, @ProductID, @DateAdded)";

                                // Replace these with the appropriate CustomerID and auto-generated WishListID

                                //MUST CHANGE THIS VALUE INTO THE GERNERIC CUSTOMER VARIABLE
                                int customerID = CustomerSession.CustomerID; // Replace with the actual customer ID dynamically
                                int wishListID = GetNewWishListID(connectionString1); // A helper function to get a new WishListID
                                DateTime dateAdded = DateTime.Now; // Current date and time

                                using (SqlConnection connection1 = new SqlConnection(connectionString1))
                                {
                                    connection1.Open(); // Open the correct connection here
                                    using (SqlCommand command1 = new SqlCommand(insertQuery, connection1))
                                    {
                                        // Add parameter values
                                        command1.Parameters.AddWithValue("@WishListID", wishListID);
                                        command1.Parameters.AddWithValue("@CustomerID", customerID);
                                        command1.Parameters.AddWithValue("@ProductID", productId); // 'productId' comes from the button context
                                        command1.Parameters.AddWithValue("@DateAdded", dateAdded);

                                        // Execute the query
                                        command1.ExecuteNonQuery();
                                    }
                                }

                                // Show success message
                                MessageBox.Show($"{productName} added to wishlist!");
                            }
                            catch (Exception ex)
                            {
                                // Show error message
                                MessageBox.Show($"Error adding to wishlist: {ex.Message}");
                            }
                        };


                        // Add controls to the productPanel
                        productPanel.Controls.Add(productPictureBox);
                        productPanel.Controls.Add(lblProductName);
                        productPanel.Controls.Add(lblPrice);
                        productPanel.Controls.Add(nudQuantity);
                        productPanel.Controls.Add(btnWishlist);

                        // Add the productPanel to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(productPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}");
            }


            //////////////////////////////////////////////////////////////////


        }
        private void button5_Click(object sender, EventArgs e)
        {
            homepage homepageForm = new homepage();
            homepageForm.Show();
            // Hide the current form (homepage)
            this.Hide();

            //panel2.Visible = !panel2.Visible; // Toggle visibility
        }
        private void button3_Click(object sender, EventArgs e)
        {
            MyWishlist wishlistForm = new MyWishlist();
            wishlistForm.Show();
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
    }
}
