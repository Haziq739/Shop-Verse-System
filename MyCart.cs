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
    public partial class MyCart : Form
    {
        public MyCart()
        {
            InitializeComponent();
        }
        private void MyCart_Load(object sender, EventArgs e)
        {
            LoadProductImages();
        }



        //////////////////////////////////////////////////////////////////

        private void LoadProductImages()
        {
            // Clear existing controls in the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            int customerID = CustomerSession.CustomerID; // Retrieve CustomerID from session

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Step 1: Get CartID for the current CustomerID
                    string cartQuery = "SELECT CartID FROM CartTab WHERE CustomerID = @CustomerID";
                    SqlCommand cartCommand = new SqlCommand(cartQuery, connection);
                    cartCommand.Parameters.AddWithValue("@CustomerID", customerID);
                    object cartIDObj = cartCommand.ExecuteScalar();

                    if (cartIDObj == null)
                    {
                        MessageBox.Show("No active cart found for this customer.");
                        return;
                    }

                    int cartID = Convert.ToInt32(cartIDObj);

                    // Step 2: Retrieve cart items for the CartID
                    string cartItemQuery = "SELECT ProductID, Quantity, DateAdded FROM CartItemTab WHERE CartID = @CartID";
                    SqlCommand cartItemCommand = new SqlCommand(cartItemQuery, connection);
                    cartItemCommand.Parameters.AddWithValue("@CartID", cartID);

                    SqlDataReader cartItemReader = cartItemCommand.ExecuteReader();
                    List<(int ProductID, int Quantity, DateTime DateAdded)> cartItems = new List<(int, int, DateTime)>();

                    while (cartItemReader.Read())
                    {
                        cartItems.Add((
                            ProductID: Convert.ToInt32(cartItemReader["ProductID"]),
                            Quantity: Convert.ToInt32(cartItemReader["Quantity"]),
                            DateAdded: Convert.ToDateTime(cartItemReader["DateAdded"])
                        ));
                    }

                    cartItemReader.Close();

                    // Step 3: Retrieve product details for each ProductID and display them
                    foreach (var cartItem in cartItems)
                    {
                        string productQuery = "SELECT ProductName, ImageURL FROM ProductsTab WHERE ProductID = @ProductID";
                        SqlCommand productCommand = new SqlCommand(productQuery, connection);
                        productCommand.Parameters.AddWithValue("@ProductID", cartItem.ProductID);

                        SqlDataReader productReader = productCommand.ExecuteReader();

                        if (productReader.Read())
                        {
                            string productName = productReader["ProductName"].ToString();
                            string imageUrl = productReader["ImageURL"].ToString() + ".jpg"; // Add extension
                            string imagesFolderPath = Application.StartupPath + "\\Images"; // Path to the Images folder
                            string imagePath = System.IO.Path.Combine(imagesFolderPath, imageUrl);

                            // Create Panel for each cart item
                            Panel cartItemPanel = new Panel
                            {
                                Width = flowLayoutPanel1.Width - 30,
                                Height = 150,
                                BorderStyle = BorderStyle.FixedSingle,
                                Padding = new Padding(5)
                            };

                            // PictureBox for product image
                            PictureBox productPictureBox = new PictureBox
                            {
                                Size = new Size(100, 100),
                                SizeMode = PictureBoxSizeMode.StretchImage,
                                Margin = new Padding(5),
                                BorderStyle = BorderStyle.FixedSingle
                            };

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

                            // Label for product name
                            Label lblProductName = new Label
                            {
                                Text = productName,
                                AutoSize = true,
                                Font = new Font("Arial", 10, FontStyle.Bold),
                                Location = new Point(110, 10)
                            };

                            // Label for quantity
                            Label lblQuantity = new Label
                            {
                                Text = $"Quantity: {cartItem.Quantity}",
                                AutoSize = true,
                                Font = new Font("Arial", 9),
                                Location = new Point(110, 40)
                            };

                            // Label for DateAdded
                            Label lblDateAdded = new Label
                            {
                                Text = $"Added On: {cartItem.DateAdded.ToShortDateString()}",
                                AutoSize = true,
                                Font = new Font("Arial", 9),
                                Location = new Point(110, 70)
                            };

                            // Add controls to the cartItemPanel
                            cartItemPanel.Controls.Add(productPictureBox);
                            cartItemPanel.Controls.Add(lblProductName);
                            cartItemPanel.Controls.Add(lblQuantity);
                            cartItemPanel.Controls.Add(lblDateAdded);

                            // Add the cartItemPanel to the FlowLayoutPanel
                            flowLayoutPanel1.Controls.Add(cartItemPanel);
                        }

                        productReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cart items: {ex.Message}");
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


        private decimal CalculateCartTotal(int customerID, SqlConnection connection)
        {
            string query = @"
        SELECT SUM(p.Price * ci.Quantity) AS TotalAmount
        FROM CartItemTab ci
        JOIN ProductsTab p ON ci.ProductID = p.ProductID
        WHERE ci.CartID IN (SELECT CartID FROM CartTab WHERE CustomerID = @CustomerID)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerID);
                return (decimal)command.ExecuteScalar();
            }
        }
        private string GetCustomerShippingAddress(int customerID, SqlConnection connection)
        {
            string query = "SELECT Address FROM CustomersTab WHERE CustomerID = @CustomerID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerID);
                return (string)command.ExecuteScalar();
            }
        }
        private decimal GetProductPrice(int productID, SqlConnection connection)
        {
            string query = "SELECT Price FROM ProductsTab WHERE ProductID = @ProductID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductID", productID);
                return (decimal)command.ExecuteScalar();
            }
        }
        private void ClearCart(int customerID, SqlConnection connection)
        {
            string deleteCartItemsQuery = @"
        DELETE FROM CartItemTab WHERE CartID IN 
        (SELECT CartID FROM CartTab WHERE CustomerID = @CustomerID)";
            using (SqlCommand deleteCartItemsCommand = new SqlCommand(deleteCartItemsQuery, connection))
            {
                deleteCartItemsCommand.Parameters.AddWithValue("@CustomerID", customerID);
                deleteCartItemsCommand.ExecuteNonQuery();
            }

            string deleteCartQuery = "DELETE FROM CartTab WHERE CustomerID = @CustomerID";
            using (SqlCommand deleteCartCommand = new SqlCommand(deleteCartQuery, connection))
            {
                deleteCartCommand.Parameters.AddWithValue("@CustomerID", customerID);
                deleteCartCommand.ExecuteNonQuery();
            }
        }
        ///////////////////
        private void CheckoutCart(int customerID)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False;MultipleActiveResultSets=True";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Step 1: Insert into OrderTab (without OrderID as it's auto-incremented)
                    string insertOrderQuery = @"
                INSERT INTO OrderTab (AdminID, CustomerID, OrderDate, Status, TotalAmount, ShippingAddress, PaymentStatus, ShippingMethod) 
                OUTPUT INSERTED.OrderID
                VALUES (@AdminID, @CustomerID, @OrderDate, @Status, @TotalAmount, @ShippingAddress, @PaymentStatus, @ShippingMethod)";

                    decimal totalAmount = CalculateCartTotal(customerID, connection); // Calculate total amount from cart
                    string shippingAddress = GetCustomerShippingAddress(customerID, connection); // Fetch shipping address
                    DateTime orderDate = DateTime.Now;

                    // Fetch the OrderID right after the insert
                    int orderID = 0;
                    using (SqlCommand orderCommand = new SqlCommand(insertOrderQuery, connection))
                    {
                        orderCommand.Parameters.AddWithValue("@AdminID", 1); // AdminID is fixed as 1
                        orderCommand.Parameters.AddWithValue("@CustomerID", customerID);
                        orderCommand.Parameters.AddWithValue("@OrderDate", orderDate);
                        orderCommand.Parameters.AddWithValue("@Status", "Pending");
                        orderCommand.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        orderCommand.Parameters.AddWithValue("@ShippingAddress", shippingAddress);
                        orderCommand.Parameters.AddWithValue("@PaymentStatus", "Unpaid");
                        orderCommand.Parameters.AddWithValue("@ShippingMethod", "Standard");

                        // Execute the command and get the OrderID
                        orderID = (int)orderCommand.ExecuteScalar();
                    }

                    // Step 2: Insert into OrderItemDetailsTab for each item in CartItemTab
                    string fetchCartItemsQuery = @"
                SELECT ci.ProductID, ci.Quantity 
                FROM CartItemTab ci
                WHERE ci.CartID IN (SELECT CartID FROM CartTab WHERE CustomerID = @CustomerID)";

                    using (SqlCommand fetchCartItemsCommand = new SqlCommand(fetchCartItemsQuery, connection))
                    {
                        fetchCartItemsCommand.Parameters.AddWithValue("@CustomerID", customerID);

                        using (SqlDataReader reader = fetchCartItemsCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int productID = reader.GetInt32(0);
                                int quantity = reader.GetInt32(1);

                                // Step 3: Fetch sellerID from SellsTab for the productID
                                int sellerID = 0;
                                string fetchSellerQuery = "SELECT SellerID FROM SellsTab WHERE ProductID = @ProductID";
                                using (SqlCommand fetchSellerCommand = new SqlCommand(fetchSellerQuery, connection))
                                {
                                    fetchSellerCommand.Parameters.AddWithValue("@ProductID", productID);
                                    sellerID = Convert.ToInt32(fetchSellerCommand.ExecuteScalar());
                                }

                                decimal priceAtPurchase = GetProductPrice(productID, connection); // Get price from Product table
                                //Console.WriteLine("ProductID is "+ orderID);
                                // Step 4: Insert into OrderItemDetailsTab
                                string insertOrderItemQuery = @"
                            INSERT INTO OrderItemDetailsTab (OrderID, ProductID, SellerID, Quantity, PriceAtPurchase) 
                            VALUES (@OrderID, @ProductID, @SellerID, @Quantity, @PriceAtPurchase)";

                                using (SqlCommand orderItemCommand = new SqlCommand(insertOrderItemQuery, connection))
                                {
                                    orderItemCommand.Parameters.AddWithValue("@OrderID", orderID);
                                    orderItemCommand.Parameters.AddWithValue("@ProductID", productID);
                                    orderItemCommand.Parameters.AddWithValue("@SellerID", sellerID);
                                    orderItemCommand.Parameters.AddWithValue("@Quantity", quantity);
                                    orderItemCommand.Parameters.AddWithValue("@PriceAtPurchase", priceAtPurchase);

                                    orderItemCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    // Step 5: Clear the CartTab and CartItemTab for the customer
                    ClearCart(customerID, connection);

                    MessageBox.Show("Checkout completed successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during checkout: {ex.Message}");
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            CheckoutCart(1);

            // Open the MyCart form
            MyCart myCart = new MyCart();
            myCart.Show();
            // Hide the current form (homepage)
            this.Hide();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            homepage homepageForm = new homepage();
            homepageForm.Show();
            // Hide the current form (homepage)
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False;MultipleActiveResultSets=True";
            int customerID = CustomerSession.CustomerID;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    ClearCart(customerID, connection);
                }

                MyCart mycartForm = new MyCart();
                mycartForm.Show();
                // Hide the current form (homepage)
                this.Hide();
            }
            catch (Exception ex)
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MyOrder myorderForm = new MyOrder();
            myorderForm.Show();
            // Hide the current form (homepage)
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }




        /////////////////////////////////////////////////////////////////

    }
}
