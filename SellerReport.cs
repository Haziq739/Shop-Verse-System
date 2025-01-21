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
    public partial class SellerReport : Form
    {
        public SellerReport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            Order_Fulfillment_and_Shipping orderFullForm = new Order_Fulfillment_and_Shipping();
            orderFullForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            orderFullForm.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            SalesPerformanceReport salesPerfReport = new SalesPerformanceReport();
            salesPerfReport.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            salesPerfReport.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            InventoryManagementReport inventManageReport = new InventoryManagementReport();
            inventManageReport.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            inventManageReport.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            PlatformGrowthAndUserEngagement platReport = new PlatformGrowthAndUserEngagement();
            platReport.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            platReport.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            UserDemographicReport userReport = new UserDemographicReport();
            userReport.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            userReport.Show();

            // Hide the current form (homepage)
            this.Hide();
        }
        private void AdminPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
    }
}
