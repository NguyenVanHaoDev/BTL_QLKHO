using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class PhieuNhapConfiguration : EntityTypeConfiguration<PhieuNhap>
    {
        public PhieuNhapConfiguration()
        {
            // Table mapping
            ToTable("PhieuNhap");

            // Primary key
            HasKey(p => p.MaPN);

            // Properties
            Property(p => p.MaPN)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(p => p.TrangThai)
                .IsRequired()
                .HasMaxLength(20);

            Property(p => p.GhiChu)
                .HasMaxLength(500);

            Property(p => p.TongTien)
                .HasPrecision(18, 2);

            // Indexes
            HasIndex(p => p.NgayNhap);

            HasIndex(p => p.MaNCC);

            HasIndex(p => p.TrangThai);

            HasIndex(p => p.NguoiTao);

            HasIndex(p => p.TongTien);

            // Relationships
            HasRequired(p => p.NhaCungCap)
                .WithMany(n => n.PhieuNhaps)
                .HasForeignKey(p => p.MaNCC);

            HasRequired(p => p.NguoiDung)
                .WithMany(n => n.PhieuNhaps)
                .HasForeignKey(p => p.NguoiTao);

            HasMany(p => p.ChiTietPhieuNhaps)
                .WithRequired(c => c.PhieuNhap)
                .HasForeignKey(c => c.MaPN);
        }
    }
}
