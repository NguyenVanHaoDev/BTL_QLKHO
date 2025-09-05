using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class PhieuXuatConfiguration : EntityTypeConfiguration<PhieuXuat>
    {
        public PhieuXuatConfiguration()
        {
            // Table mapping
            ToTable("PhieuXuat");

            // Primary key
            HasKey(p => p.MaPX);

            // Properties
            Property(p => p.MaPX)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(p => p.NguoiNhan)
                .IsRequired()
                .HasMaxLength(100);

            Property(p => p.TrangThai)
                .IsRequired()
                .HasMaxLength(20);

            Property(p => p.GhiChu)
                .HasMaxLength(500);

            Property(p => p.TongTien)
                .HasPrecision(18, 2);

            // Indexes
            HasIndex(p => p.NgayXuat);

            HasIndex(p => p.TrangThai);

            HasIndex(p => p.NguoiTao);

            HasIndex(p => p.TongTien);

            // Relationships
            HasRequired(p => p.NguoiDung)
                .WithMany(n => n.PhieuXuats)
                .HasForeignKey(p => p.NguoiTao);

            HasMany(p => p.ChiTietPhieuXuats)
                .WithRequired(c => c.PhieuXuat)
                .HasForeignKey(c => c.MaPX);
        }
    }
}
