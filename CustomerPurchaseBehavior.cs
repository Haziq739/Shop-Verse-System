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
    public partial class CustomerPurchaseBehavior : Form
    {
        public CustomerPurchaseBehavior()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        c.CustomerID,
        c.CustomerName,
        COUNT(o.OrderID) AS TotalOrders
    FROM 
        CustomersTab c
    JOIN 
        OrderTab o ON c.CustomerID = o.CustomerID
    GROUP BY 
        c.CustomerID, 
        c.CustomerName
    ORDER BY 
        TotalOrders DESC;";


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
        c.CustomerID,
        c.CustomerName,
        AVG(o.TotalAmount) AS AverageSpend
    FROM 
        CustomersTab c
    JOIN 
        OrderTab o ON c.CustomerID = o.CustomerID
    GROUP BY 
        c.CustomerID, 
        c.CustomerName;";



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
        COUNT(DISTINCT o.CustomerID) AS RepeatPurchasers,
        (SELECT COUNT(DISTINCT CustomerID) FROM OrderTab) AS TotalCustomers,
        (COUNT(DISTINCT o.CustomerID) * 100.0) / (SELECT COUNT(DISTINCT CustomerID) FROM OrderTab) AS RepeatPurchaseRate
    FROM 
        OrderTab o
    GROUP BY 
        o.CustomerID
    HAVING 
        COUNT(o.OrderID) > 1;";



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

        private void CustomerPurchaseBehavior_Load(object sender, EventArgs e)
        {

        }
    }
}
