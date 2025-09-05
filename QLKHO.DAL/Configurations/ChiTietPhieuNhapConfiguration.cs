using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class ChiTietPhieuNhapConfiguration : EntityTypeConfiguration<ChiTietPhieuNhap>
    {
        public ChiTietPhieuNhapConfiguration()
        {
            // Table mapping
            ToTable("ChiTietPhieuNhap");

            // Composite primary key
            HasKey(c => new { c.MaPN, c.MaSP });

            // Properties
            Property(c => c.DonGia)
                .HasPrecision(18, 2);

            Property(c => c.ThanhTien)
                .HasPrecision(18, 2)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            // Indexes
            HasIndex(c => c.MaSP);

            HasIndex(c => c.MaPN);

            // Relationships
            HasRequired(c => c.PhieuNhap)
                .WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(c => c.MaPN);

            HasRequired(c => c.SanPham)
                .WithMany(s => s.ChiTietPhieuNhaps)
                .HasForeignKey(c => c.MaSP);
        }
    }
}
