using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class NhaCungCapConfiguration : EntityTypeConfiguration<NhaCungCap>
    {
        public NhaCungCapConfiguration()
        {
            // Table mapping
            ToTable("NhaCungCap");

            // Primary key
            HasKey(n => n.MaNCC);

            // Properties
            Property(n => n.MaNCC)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(n => n.TenNCC)
                .IsRequired()
                .HasMaxLength(100);

            Property(n => n.DiaChi)
                .HasMaxLength(200);

            Property(n => n.DienThoai)
                .HasMaxLength(15);

            Property(n => n.Email)
                .HasMaxLength(100);

            // Indexes
            HasIndex(n => n.TenNCC);

            HasIndex(n => n.TrangThai);

            // Relationships
            HasMany(n => n.PhieuNhaps)
                .WithRequired(p => p.NhaCungCap)
                .HasForeignKey(p => p.MaNCC);
        }
    }
}
