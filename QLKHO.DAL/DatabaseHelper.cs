using System;
using System.Data.Entity;
using System.Linq;

namespace QLKHO.DAL
{
    public static class DatabaseHelper
    {
        /// <summary>
        /// Khởi tạo database và tạo bảng nếu chưa tồn tại
        /// </summary>
        public static void InitializeDatabase()
        {
            try
            {
                // Thiết lập Database Initializer
                Database.SetInitializer<QuanLyKhoDbContext>(new QuanLyKhoDatabaseInitializer());
                Console.WriteLine("Checking database connection...");
                using (var context = new QuanLyKhoDbContext())
                {
                    // Kiểm tra và tạo database nếu chưa tồn tại
                    if (!context.Database.Exists())
                    {
                        context.Database.Create();
                        Console.WriteLine("Database đã được tạo thành công!");
                    }
                    else
                    {
                        Console.WriteLine("Database đã tồn tại!");
                    }

                    // Kiểm tra kết nối bằng cách thực hiện một query đơn giản
                    var count = context.DonViTinhs.Count();
                    Console.WriteLine($"Database schema đã sẵn sàng! Số đơn vị tính hiện tại: {count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi khởi tạo database: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        /// <returns>True nếu kết nối thành công</returns>
        public static bool TestConnection()
        {
            try
            {
                using (var context = new QuanLyKhoDbContext())
                {
                    // Test thực sự bằng cách thực hiện một query đơn giản
                    var count = context.DonViTinhs.Count();
                    Console.WriteLine($"✓ Database connection successful! Found {count} DonViTinh records.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Database connection failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Xóa và tạo lại database (chỉ dùng trong development)
        /// </summary>
        public static void RecreateDatabase()
        {
            try
            {
                // Thiết lập Database Initializer
                Database.SetInitializer<QuanLyKhoDbContext>(new DropCreateDatabaseAlways<QuanLyKhoDbContext>());
                
                using (var context = new QuanLyKhoDbContext())
                {
                    // Force database creation/recreation
                    context.Database.Initialize(true);
                    Console.WriteLine("Database đã được tạo lại thành công!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo lại database: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy thông tin database
        /// </summary>
        /// <returns>Thông tin database</returns>
        public static string GetDatabaseInfo()
        {
            try
            {
                using (var context = new QuanLyKhoDbContext())
                {
                    var connectionString = context.Database.Connection.ConnectionString;
                    var databaseName = context.Database.Connection.Database;
                    var serverName = context.Database.Connection.DataSource;
                    
                    return $"Server: {serverName}\nDatabase: {databaseName}\nConnection: {connectionString}";
                }
            }
            catch (Exception ex)
            {
                return $"Lỗi khi lấy thông tin database: {ex.Message}";
            }
        }
    }

    /// <summary>
    /// Database Initializer cho Entity Framework 6
    /// </summary>
    public class QuanLyKhoDatabaseInitializer : CreateDatabaseIfNotExists<QuanLyKhoDbContext>
    {
        protected override void Seed(QuanLyKhoDbContext context)
        {
            // Có thể thêm dữ liệu mẫu ở đây nếu cần
            base.Seed(context);
        }
    }
}
