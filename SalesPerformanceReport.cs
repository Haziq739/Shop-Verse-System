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
    public partial class SalesPerformanceReport : Form
    {
        public SalesPerformanceReport()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FirstReportForm_Load(object sender, EventArgs e)
        {
            

          
        }

        private void button1_Click(object sender, EventArgs e)
        {


            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        pt.ProductName,                   
        pt.Price,                         
        pt.StockQuantity,                  
        SUM(pt.Price * pt.StockQuantity) AS TotalSales   
    FROM ProductsTab pt
    JOIN Sellstab st ON st.ProductID = pt.ProductID
    GROUP BY 
        pt.ProductName, 
        pt.Price, 
        pt.StockQuantity;";


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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        s.SellDate,                                      -- Using SellDate to group by sale date
        p.ProductName,                                   -- Product name from ProductsTab
        SUM(p.Price * p.StockQuantity) AS TotalOrderValue,  -- Calculating total order value (SellPrice * Quantity)
        AVG(SUM(p.Price * p.StockQuantity)) OVER () AS AverageOrderValue  -- Calculating the average of total order values
    FROM 
        ProductsTab p
    JOIN 
        Sellstab s ON s.ProductID = p.ProductID       -- Joining with ProductsTab to get ProductName
    GROUP BY 
        s.SellDate,                                      -- Grouping by SellDate to calculate for each sale date
        p.ProductName;";    // Grouping by ProductName to calculate per product


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

        private void button3_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        p.ProductID, 
        p.ProductName, 
        SUM(p.stockQuantity) AS TotalQuantitySold
    FROM ProductsTab p
    JOIN Sellstab s ON s.ProductID = p.ProductID
    GROUP BY 
        p.ProductID, 
        p.ProductName
    ORDER BY 
        TotalQuantitySold DESC;";


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
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        c.CategoryName,                             -- Category name from CategoriesTab
        SUM(p.stockQuantity * p.Price) AS TotalSales     -- Total sales calculated by multiplying quantity and price
    FROM 
        ProductsTab p
    JOIN 
        Sellstab s ON s.ProductID = p.ProductID  -- Joining Sellstab and ProductsTab by ProductID
    JOIN 
        CategoriesTab c ON p.CategoryID = c.CategoryID -- Joining ProductsTab and CategoriesTab by CategoryID
    GROUP BY 
        c.CategoryName                               -- Grouping by CategoryName to calculate total sales per category
    ORDER BY 
        TotalSales DESC;                             -- Sorting the result by TotalSales in descending order";



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
    }
}
