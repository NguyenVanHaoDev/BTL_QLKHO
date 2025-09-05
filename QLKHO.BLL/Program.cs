using System;
using System.Windows.Forms;
using QLKHO.BLL.Demo;

namespace QLKHO.BLL
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Không set encoding để tránh lỗi "The handle is invalid"
            
            Console.WriteLine("=== CHUONG TRINH QUAN LY KHO - BUSINESS LOGIC LAYER ===");
            Console.WriteLine();

            try
            {
                // Chạy demo business logic
                BusinessDemo.RunDemo();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Loi: {ex.Message}");
                Console.WriteLine("Chuong trinh se tu dong thoat sau 3 giay...");
                System.Threading.Thread.Sleep(3000);
            }

            Console.WriteLine();
            Console.WriteLine("Chuong trinh hoan tat!");
            System.Threading.Thread.Sleep(2000);

            // Uncomment để chạy Windows Forms
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Form1());
        }
    }
}
