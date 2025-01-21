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
    public partial class UserDemographicReport : Form
    {
        public UserDemographicReport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT 
        CASE 
            WHEN Age BETWEEN 0 AND 18 THEN '0-18'
            WHEN Age BETWEEN 19 AND 30 THEN '19-30'
            WHEN Age BETWEEN 31 AND 40 THEN '31-40'
            WHEN Age BETWEEN 41 AND 50 THEN '41-50'
            WHEN Age BETWEEN 51 AND 60 THEN '51-60'
            WHEN Age BETWEEN 61 AND 70 THEN '61-70'
            WHEN Age BETWEEN 71 AND 80 THEN '71-80'
            ELSE '81+' 
        END AS AgeRange,
        COUNT(*) AS UserCount
    FROM 
        DemographicsTab
    GROUP BY 
        CASE 
            WHEN Age BETWEEN 0 AND 18 THEN '0-18'
            WHEN Age BETWEEN 19 AND 30 THEN '19-30'
            WHEN Age BETWEEN 31 AND 40 THEN '31-40'
            WHEN Age BETWEEN 41 AND 50 THEN '41-50'
            WHEN Age BETWEEN 51 AND 60 THEN '51-60'
            WHEN Age BETWEEN 61 AND 70 THEN '61-70'
            WHEN Age BETWEEN 71 AND 80 THEN '71-80'
            ELSE '81+' 
        END
    ORDER BY 
        AgeRange;
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
        Gender, 
        COUNT(*) AS UserCount
    FROM 
        DemographicsTab
    GROUP BY 
        Gender;
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
        Location, 
        COUNT(*) AS UserCount
    FROM 
        DemographicsTab
    GROUP BY 
        Location
    ORDER BY 
        UserCount DESC;
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
        D.Age, 
        D.Gender, 
        D.Location, 
        C.PaymentPreferences, 
        COUNT(*) AS UserCount
    FROM 
        DemographicsTab D
    JOIN 
        CustomersTab C ON D.CustomerID = C.CustomerID
    GROUP BY 
        D.Age, D.Gender, D.Location, C.PaymentPreferences
    ORDER BY 
        UserCount DESC;
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

        private void UserDemographicReport_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
