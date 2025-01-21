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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Open the MyCart form
            AdminLoginForm adminForm = new AdminLoginForm();
            adminForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            adminForm.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Open the MyCart form
            SellerLoginForm sellerloginForm = new SellerLoginForm();
            sellerloginForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            sellerloginForm.Show();

            // Hide the current form (homepage)
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            // Open the MyCart form
            CustomerLoginForm customerForm = new CustomerLoginForm();
            customerForm.FormClosed += new FormClosedEventHandler(AdminPage_FormClosed);
            customerForm.Show();

            // Hide the current form (homepage)
            this.Hide();

        }
        private void AdminPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
    }
}
