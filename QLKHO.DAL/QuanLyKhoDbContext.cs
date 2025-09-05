using System.Data.Entity;
using QLKHO.DAL.Models;
using QLKHO.DAL.Configurations;

namespace QLKHO.DAL
{
    public class QuanLyKhoDbContext : DbContext
    {
        public QuanLyKhoDbContext() : base("name=QuanLyKhoConnectionString")
        {
            // Cấu hình Entity Framework
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ValidateOnSaveEnabled = true;
            
            // Cấu hình Database Initializer cho EF6
            Database.SetInitializer<QuanLyKhoDbContext>(new QuanLyKhoDatabaseInitializer());
        }

        // DbSets cho các bảng
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<DonViTinh> DonViTinhs { get; set; }
        public DbSet<DanhMucSanPham> DanhMucSanPhams { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public DbSet<PhieuXuat> PhieuXuats { get; set; }
        public DbSet<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; }
        public DbSet<LichSuDangNhap> LichSuDangNhaps { get; set; }
        public DbSet<LichSuThayDoi> LichSuThayDois { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.Configurations.Add(new NguoiDungConfiguration());
            modelBuilder.Configurations.Add(new DonViTinhConfiguration());
            modelBuilder.Configurations.Add(new DanhMucSanPhamConfiguration());
            modelBuilder.Configurations.Add(new SanPhamConfiguration());
            modelBuilder.Configurations.Add(new NhaCungCapConfiguration());
            modelBuilder.Configurations.Add(new PhieuNhapConfiguration());
            modelBuilder.Configurations.Add(new ChiTietPhieuNhapConfiguration());
            modelBuilder.Configurations.Add(new PhieuXuatConfiguration());
            modelBuilder.Configurations.Add(new ChiTietPhieuXuatConfiguration());
            modelBuilder.Configurations.Add(new LichSuDangNhapConfiguration());
            modelBuilder.Configurations.Add(new LichSuThayDoiConfiguration());
        }
    }
}
