using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class SanPhamConfiguration : EntityTypeConfiguration<SanPham>
    {
        public SanPhamConfiguration()
        {
            // Table mapping
            ToTable("SanPham");

            // Primary key
            HasKey(s => s.MaSP);

            // Properties
            Property(s => s.MaSP)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(s => s.TenSP)
                .IsRequired()
                .HasMaxLength(100);

            Property(s => s.MoTa)
                .HasMaxLength(500);

            Property(s => s.GiaNhap)
                .HasPrecision(18, 2);

            Property(s => s.GiaBan)
                .HasPrecision(18, 2);

            // Indexes
            HasIndex(s => s.TenSP);

            HasIndex(s => s.MaDM);

            HasIndex(s => s.MaDVT);

            HasIndex(s => s.TrangThai);

            HasIndex(s => s.SoLuongTon);

            HasIndex(s => s.NguoiTao);

            // Relationships
            HasRequired(s => s.DanhMucSanPham)
                .WithMany(d => d.SanPhams)
                .HasForeignKey(s => s.MaDM);

            HasRequired(s => s.DonViTinh)
                .WithMany(d => d.SanPhams)
                .HasForeignKey(s => s.MaDVT);

            HasOptional(s => s.NguoiDung)
                .WithMany(n => n.SanPhams)
                .HasForeignKey(s => s.NguoiTao);

            HasMany(s => s.ChiTietPhieuNhaps)
                .WithRequired(c => c.SanPham)
                .HasForeignKey(c => c.MaSP);

            HasMany(s => s.ChiTietPhieuXuats)
                .WithRequired(c => c.SanPham)
                .HasForeignKey(c => c.MaSP);
        }
    }
}
