using System;
using System.Linq;
using QLKHO.BLL.Services;

namespace QLKHO.BLL.Demo
{
    public class BusinessDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== DEMO BUSINESS LOGIC LAYER ===");
            Console.WriteLine();

            try
            {
                // Test ket noi database
                Console.WriteLine("1. Kiem tra ket noi database...");
                if (QLKHO.DAL.DatabaseHelper.TestConnection())
                {
                    Console.WriteLine("✓ Ket noi database thanh cong!");
                }
                else
                {
                    Console.WriteLine("✗ Khong the ket noi database!");
                    return;
                }
                Console.WriteLine();

                // Test DonViTinh Service
                Console.WriteLine("2. Test DonViTinh Service...");
                TestDonViTinhService();
                Console.WriteLine();

                // Test SanPham Service
                Console.WriteLine("3. Test SanPham Service...");
                TestSanPhamService();
                Console.WriteLine();

                Console.WriteLine("=== DEMO BUSINESS LOGIC HOAN TAT ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Loi trong demo: {ex.Message}");
            }
        }

        private static void TestDonViTinhService()
        {
            using (var service = new DonViTinhService())
            {
                try
                {
                    // Lay tat ca don vi tinh
                    Console.WriteLine("Lay tat ca don vi tinh:");
                    var allDonViTinhs = service.GetAllActive().ToList();
                    foreach (var dvt in allDonViTinhs.Take(5))
                    {
                        Console.WriteLine($"  - {dvt.TenDVT} ({dvt.KyHieu})");
                    }
                    Console.WriteLine($"Tong cong: {allDonViTinhs.Count} don vi tinh");

                    // Tim kiem
                    Console.WriteLine("\nTim kiem don vi tinh co chua 'cai':");
                    var searchResults = service.Search("cai").ToList();
                    foreach (var dvt in searchResults)
                    {
                        Console.WriteLine($"  - {dvt.TenDVT} ({dvt.KyHieu})");
                    }

                    // Them don vi tinh moi (demo)
                    Console.WriteLine("\nThem don vi tinh moi (demo):");
                    var newDVT = new QLKHO.DAL.Models.DonViTinh
                    {
                        TenDVT = "Demo Unit",
                        KyHieu = "demo",
                        MoTa = "Don vi tinh demo"
                    };

                    try
                    {
                        service.Add(newDVT);
                        Console.WriteLine("✓ Them don vi tinh thanh cong!");
                        
                        // Xoa don vi tinh demo
                        service.Delete(newDVT.MaDVT);
                        Console.WriteLine("✓ Xoa don vi tinh demo thanh cong!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Loi khi thao tac don vi tinh: {ex.Message}");
                    }

                    Console.WriteLine("✓ Test DonViTinh Service thanh cong!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Loi trong DonViTinh Service: {ex.Message}");
                }
            }
        }

        private static void TestSanPhamService()
        {
            using (var service = new SanPhamService())
            {
                try
                {
                    // Lay tat ca san pham
                    Console.WriteLine("Lay tat ca san pham:");
                    var allSanPhams = service.GetAllActive().ToList();
                    foreach (var sp in allSanPhams.Take(3))
                    {
                        Console.WriteLine($"  - {sp.TenSP} (Ton: {sp.SoLuongTon})");
                    }
                    Console.WriteLine($"Tong cong: {allSanPhams.Count} san pham");

                    // Lay san pham sap het hang
                    Console.WriteLine("\nSan pham sap het hang:");
                    var sapHetHang = service.GetSanPhamSapHetHang().ToList();
                    foreach (var sp in sapHetHang.Take(3))
                    {
                        Console.WriteLine($"  - {sp.TenSP} (Ton: {sp.SoLuongTon}, Toi thieu: {sp.SoLuongToiThieu})");
                    }

                    // Lay san pham het hang
                    Console.WriteLine("\nSan pham het hang:");
                    var hetHang = service.GetSanPhamHetHang().ToList();
                    foreach (var sp in hetHang.Take(3))
                    {
                        Console.WriteLine($"  - {sp.TenSP} (Ton: {sp.SoLuongTon})");
                    }

                    Console.WriteLine("✓ Test SanPham Service thanh cong!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Loi trong SanPham Service: {ex.Message}");
                }
            }
        }
    }
}
