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
    public partial class InventoryManagementReport : Form
    {
        public InventoryManagementReport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        p.ProductID,
        p.ProductName,
        p.StockQuantity
    FROM 
        ProductsTab p
    WHERE 
        p.StockQuantity < 50;  -- Adjust the threshold as needed";



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
        p.ProductID,
        p.ProductName,
        p.StockQuantity
    FROM 
        ProductsTab p
    LEFT JOIN 
        OrderItemDetailsTab od ON p.ProductID = od.ProductID
    WHERE 
        od.ProductID IS NULL;  -- Products with no sales";



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
        SUM(od.Quantity) AS TotalSold,
        p.StockQuantity,
        (SUM(od.Quantity) / p.StockQuantity) AS StockTurnoverRate
    FROM 
        ProductsTab p
    JOIN 
        OrderItemDetailsTab od ON p.ProductID = od.ProductID
    GROUP BY 
        p.ProductID, 
        p.ProductName, 
        p.StockQuantity;";



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
        p.ProductID,
        p.ProductName,
        COUNT(od.OrderItemID) AS TotalReturns  -- Assuming returns are tracked with this count
    FROM 
        OrderItemDetailsTab od
    JOIN 
        ProductsTab p ON od.ProductID = p.ProductID
    -- If there is another mechanism (like an external table or a column) to indicate returns, you can filter here.
    GROUP BY 
        p.ProductID, 
        p.ProductName
    ORDER BY 
        TotalReturns DESC;";



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
