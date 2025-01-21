using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class AdminReport : Form
    {
        public AdminReport()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SalesPerformanceReport saleAdminRep = new SalesPerformanceReport();
            saleAdminRep.FormClosed += new FormClosedEventHandler(AdminReport_FormClosed);
            saleAdminRep.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RevenueByProductCategory revAdminRep = new RevenueByProductCategory();
            revAdminRep.FormClosed += new FormClosedEventHandler(AdminReport_FormClosed);
            revAdminRep.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SellerPerformanceReport sellPerfRep = new SellerPerformanceReport();
            sellPerfRep.FormClosed += new FormClosedEventHandler(AdminReport_FormClosed);
            sellPerfRep.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Order_Fulfillment_and_Shipping orderFullfRep = new Order_Fulfillment_and_Shipping();
            orderFullfRep.FormClosed += new FormClosedEventHandler(AdminReport_FormClosed);
            orderFullfRep.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PlatformGrowthAndUserEngagement plat_FormfRep = new PlatformGrowthAndUserEngagement();
            plat_FormfRep.FormClosed += new FormClosedEventHandler(AdminReport_FormClosed);
            plat_FormfRep.Show();

            // Hide the current form (homepage)
            this.Hide();
        }
        private void AdminReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
    }
}
