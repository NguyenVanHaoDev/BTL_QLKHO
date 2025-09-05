using System;

namespace QLKHO.DAL
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Không set encoding để tránh lỗi "The handle is invalid"
            
            Console.WriteLine("=== DATA ACCESS LAYER - KHOI TAO DATABASE ===");
            Console.WriteLine();

            try
            {
                // Khởi tạo database
                DatabaseHelper.InitializeDatabase();
                Console.WriteLine("✓ Database da san sang!");
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
        }
    }
}