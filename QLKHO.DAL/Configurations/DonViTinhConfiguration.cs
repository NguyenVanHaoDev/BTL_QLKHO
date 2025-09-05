using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class DonViTinhConfiguration : EntityTypeConfiguration<DonViTinh>
    {
        public DonViTinhConfiguration()
        {
            // Table mapping
            ToTable("DonViTinh");

            // Primary key
            HasKey(d => d.MaDVT);

            // Properties
            Property(d => d.MaDVT)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(d => d.TenDVT)
                .IsRequired()
                .HasMaxLength(50);

            Property(d => d.KyHieu)
                .IsRequired()
                .HasMaxLength(10);

            Property(d => d.MoTa)
                .HasMaxLength(200);

            // Indexes
            HasIndex(d => d.TenDVT)
                .IsUnique();

            HasIndex(d => d.KyHieu)
                .IsUnique();

            HasIndex(d => d.TrangThai);

            // Relationships
            HasMany(d => d.SanPhams)
                .WithRequired(s => s.DonViTinh)
                .HasForeignKey(s => s.MaDVT);
        }
    }
}
