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
    public partial class PlatformGrowthAndUserEngagement : Form
    {
        public PlatformGrowthAndUserEngagement()
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
        COUNT(DISTINCT CustomersTab.CustomerID) * 100.0 / 
        (SELECT COUNT(DISTINCT OrderTab.CustomerID) 
         FROM OrderTab 
         WHERE OrderTab.OrderDate BETWEEN '2023-01-01' AND '2023-12-31') AS ChurnRate
    FROM 
        OrderTab
    LEFT JOIN 
        CustomersTab ON OrderTab.CustomerID = CustomersTab.CustomerID
    LEFT JOIN 
        UserEngagementTab ON CustomersTab.CustomerID = UserEngagementTab.CustomerID
    WHERE 
        OrderTab.OrderDate BETWEEN '2023-01-01' AND '2023-12-31'
        AND (UserEngagementTab.EngagementDate IS NULL OR UserEngagementTab.EngagementDate NOT BETWEEN '2024-01-01' AND '2024-12-31')
    HAVING 
        (SELECT COUNT(DISTINCT OrderTab.CustomerID) 
         FROM OrderTab 
         WHERE OrderTab.OrderDate BETWEEN '2023-01-01' AND '2023-12-31') > 0;
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

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        CustomerID, 
        COUNT(*) AS EngagementCount
    FROM 
        UserEngagementTab
    WHERE 
        EngagementDate BETWEEN '2024-01-01' AND '2024-12-31'
    GROUP BY 
        CustomerID;
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
        UsersTab.UserID, 
        CustomersTab.RegistrationDate, 
        COUNT(UsersTab.UserID) AS NewUserRegistrations
    FROM 
        UsersTab
    JOIN 
        CustomersTab ON UsersTab.UserID = CustomersTab.UserID
    WHERE 
        CustomersTab.RegistrationDate BETWEEN '2024-01-01' AND '2024-12-31'
    GROUP BY 
        UsersTab.UserID, CustomersTab.RegistrationDate;
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

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        (COUNT(DISTINCT UserEngagementTab.CustomerID) * 100.0 / COUNT(DISTINCT CustomersTab.CustomerID)) AS ActiveUserRatio
    FROM 
        CustomersTab
    LEFT JOIN 
        UserEngagementTab ON CustomersTab.CustomerID = UserEngagementTab.CustomerID
    WHERE 
        UserEngagementTab.EngagementDate BETWEEN '2024-01-01' AND '2024-12-31';
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
