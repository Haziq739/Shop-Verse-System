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
    public partial class SellerManagementForm : Form
    {
        public SellerManagementForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-V1459M2\\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True;Encrypt=False";
            string query = @" SELECT * from SellersTab;";



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
            // Open the MyCart form
            SellerSignUpForm sellsignUpForm = new SellerSignUpForm();
            sellsignUpForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            sellsignUpForm.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            SellerDeleteForm sellDelForm = new SellerDeleteForm();
            sellDelForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            sellDelForm.Show();

            // Hide the current form (homepage)
            this.Hide();

        }
        private void AdminPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
    }
}
