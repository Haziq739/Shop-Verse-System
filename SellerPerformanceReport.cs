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
    public partial class SellerPerformanceReport : Form
    {
        public SellerPerformanceReport()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        s.SellerName,
        COUNT(DISTINCT p.ProductID) AS TotalProducts,
        AVG(r.Rating) AS AverageRating
    FROM 
        SellersTab s
    JOIN 
        ProductsTab p ON p.ProductID IN (SELECT ProductID FROM ProductReviewsTab WHERE ProductID = p.ProductID)
    JOIN 
        ProductReviewsTab r ON p.ProductID = r.ProductID
    GROUP BY 
        s.SellerName
    ORDER BY 
        AverageRating DESC;";



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable; // Bind data to DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        p.ProductName, 
        COUNT(*) AS PositiveReviews,
        (SELECT COUNT(*) 
         FROM ProductReviewsTab r2 
         WHERE r2.ProductID = p.ProductID AND r2.Rating <= 2) AS NegativeReviews,
        (SELECT COUNT(*) 
         FROM ProductReviewsTab r2 
         WHERE r2.ProductID = p.ProductID AND r2.Rating = 3) AS NeutralReviews
    FROM 
        ProductsTab p
    JOIN 
        ProductReviewsTab r ON p.ProductID = r.ProductID
    WHERE 
        r.Rating >= 4
    GROUP BY 
        p.ProductName, p.ProductID;";




            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable; // Bind data to DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        p.ProductName,
        AVG(r.Rating) AS AverageRating
    FROM 
        ProductsTab p
    JOIN 
        ProductReviewsTab r ON p.ProductID = r.ProductID
    GROUP BY 
        p.ProductName;";



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable; // Bind data to DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
