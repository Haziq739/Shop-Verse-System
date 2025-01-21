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
    public partial class AdminPage : Form
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            AddAndRemoveCustomer customerManageForm = new AddAndRemoveCustomer();
            customerManageForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            customerManageForm.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            SellerManagementForm sellManageForm = new SellerManagementForm();
            sellManageForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            sellManageForm.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            ProductTable productForm = new ProductTable();
            productForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            productForm.Show();

            // Hide the current form (homepage)
            this.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            AdminReport adminRepForm = new AdminReport();
            adminRepForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            adminRepForm.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            OrderOversight orderOversightForm = new OrderOversight();
            orderOversightForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            orderOversightForm.Show();

            // Hide the current form (homepage)
            this.Hide();
        }
        private void AdminPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            LogisticsTable orderOversightForm = new LogisticsTable();
            orderOversightForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            orderOversightForm.Show();

            // Hide the current form (homepage)
            this.Hide();
        }
    }
}
