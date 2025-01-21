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
    public partial class RevenueByProductCategory : Form
    {
        public RevenueByProductCategory()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        c.CategoryID,
        c.CategoryName,
        SUM(oi.Quantity * oi.PriceAtPurchase) AS RevenuePerCategory
    FROM 
        CategoriesTab c
    LEFT JOIN 
        ProductsTab p ON c.CategoryID = p.CategoryID
    LEFT JOIN 
        OrderItemDetailsTab oi ON p.ProductID = oi.ProductID
    GROUP BY 
        c.CategoryID, 
        c.CategoryName
    ORDER BY 
        RevenuePerCategory DESC;";



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
        c.CategoryID,
        c.CategoryName,
        SUM(oi.Quantity * oi.PriceAtPurchase) AS RevenuePerCategory,
        (SUM(oi.Quantity * oi.PriceAtPurchase) / 
            (SELECT SUM(Quantity * PriceAtPurchase) FROM OrderItemDetailsTab)) * 100 AS PercentageContribution
    FROM 
        CategoriesTab c
    LEFT JOIN 
        ProductsTab p ON c.CategoryID = p.CategoryID
    LEFT JOIN 
        OrderItemDetailsTab oi ON p.ProductID = oi.ProductID
    GROUP BY 
        c.CategoryID, 
        c.CategoryName
    ORDER BY 
        RevenuePerCategory DESC;";



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
        c.CategoryID,
        c.CategoryName,
        SUM(oi.Quantity * oi.PriceAtPurchase) AS RevenuePerCategory
    FROM 
        OrderItemDetailsTab oi
    JOIN 
        ProductsTab p ON oi.ProductID = p.ProductID
    JOIN 
        CategoriesTab c ON p.CategoryID = c.CategoryID
    JOIN 
        OrderTab o ON oi.OrderID = o.OrderID
    WHERE 
        o.OrderDate >= '2024-09-01'  -- Replace with the start date for trending period
    GROUP BY 
        c.CategoryID, 
        c.CategoryName
    ORDER BY 
        RevenuePerCategory DESC;";





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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
