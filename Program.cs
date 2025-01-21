using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Project
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SalesPerformanceReport());
            //Application.Run(new CustomerPurchaseBehavior());
            // Application.Run(new InventoryManagementReport());
            // Application.Run(new RevenueByProductCategory());
            //Application.Run(new CustomerFeedbackAndProductRating());
            //Application.Run(new SellerPerformanceReport());
            //Application.Run(new Order_Fulfillment_and_Shipping());
            //Application.Run(new AbandonedCart());
            //Application.Run(new PlatformGrowthAndUserEngagement());
            // Application.Run(new UserDemographicReport());
            //Application.Run(new AdminPage());
            // Application.Run(new ProductForm());
            // Application.Run(new Seller());
            // Application.Run(new LogisticsTable());
            //Application.Run(new homepage());
          Application.Run(new LoginForm());
            //Application.Run(new LoginForm());
            //Application.Run(new SellerDeleteForm());

        }
    }
}
