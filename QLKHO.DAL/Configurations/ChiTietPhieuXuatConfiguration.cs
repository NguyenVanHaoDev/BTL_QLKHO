using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class ChiTietPhieuXuatConfiguration : EntityTypeConfiguration<ChiTietPhieuXuat>
    {
        public ChiTietPhieuXuatConfiguration()
        {
            // Table mapping
            ToTable("ChiTietPhieuXuat");

            // Composite primary key
            HasKey(c => new { c.MaPX, c.MaSP });

            // Properties
            Property(c => c.DonGia)
                .HasPrecision(18, 2);

            Property(c => c.ThanhTien)
                .HasPrecision(18, 2)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            // Indexes
            HasIndex(c => c.MaSP);

            HasIndex(c => c.MaPX);

            // Relationships
            HasRequired(c => c.PhieuXuat)
                .WithMany(p => p.ChiTietPhieuXuats)
                .HasForeignKey(c => c.MaPX);

            HasRequired(c => c.SanPham)
                .WithMany(s => s.ChiTietPhieuXuats)
                .HasForeignKey(c => c.MaSP);
        }
    }
}
