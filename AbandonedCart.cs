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
    public partial class AbandonedCart : Form
    {
        public AbandonedCart()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        ShippingTab.ShippingID,
        OrderTab.OrderID,
        ShippingTab.EstimatedDeliveryDate,
        ShippingTab.ActualDeliveryDate,
        DATEDIFF(DAY, ShippingTab.EstimatedDeliveryDate, ShippingTab.ActualDeliveryDate) AS DelayInDays
    FROM 
        ShippingTab
    JOIN 
        OrderTab ON ShippingTab.OrderID = OrderTab.OrderID
    WHERE 
        ShippingTab.ActualDeliveryDate > ShippingTab.EstimatedDeliveryDate;
";




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
        OrderTab.OrderID,
        OrderTab.OrderDate,
        ShippingTab.ActualDeliveryDate,
        AVG(DATEDIFF(DAY, OrderTab.OrderDate, ShippingTab.ActualDeliveryDate)) OVER () AS AvgFulfillmentTime
    FROM 
        OrderTab
    JOIN 
        ShippingTab ON OrderTab.OrderID = ShippingTab.OrderID
    WHERE 
        ShippingTab.ActualDeliveryDate IS NOT NULL;
";



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
        ProviderName,
        AVG(DATEDIFF(DAY, EstimatedDeliveryDate, ActualDeliveryDate)) AS AvgDelay
    FROM 
        ShippingTab
    WHERE 
        ActualDeliveryDate IS NOT NULL
    GROUP BY 
        ProviderName
    ORDER BY 
        AvgDelay ASC;
";




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

        private void AbandonedCart_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        ShippingTab.OrderID,
        ShippingTab.Status,
        (COUNT(*) * 100.0 / (SELECT COUNT(*) FROM ShippingTab)) AS OrderCompRate
    FROM 
        ShippingTab
    JOIN 
        OrderTab ON ShippingTab.OrderID = OrderTab.OrderID
    WHERE 
        ShippingTab.Status = 'Delivered'
    GROUP BY 
        ShippingTab.OrderID, ShippingTab.Status;
";



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
