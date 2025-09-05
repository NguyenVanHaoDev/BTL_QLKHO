using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class DanhMucSanPhamConfiguration : EntityTypeConfiguration<DanhMucSanPham>
    {
        public DanhMucSanPhamConfiguration()
        {
            // Table mapping
            ToTable("DanhMucSanPham");

            // Primary key
            HasKey(d => d.MaDM);

            // Properties
            Property(d => d.MaDM)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(d => d.TenDM)
                .IsRequired()
                .HasMaxLength(100);

            Property(d => d.MoTa)
                .HasMaxLength(500);

            // Indexes
            HasIndex(d => d.TenDM)
                .IsUnique();

            HasIndex(d => d.MaDMCapTren);

            HasIndex(d => d.TrangThai);

            HasIndex(d => d.ThuTuHienThi);

            // Self-referencing relationship
            HasOptional(d => d.DanhMucCha)
                .WithMany(d => d.DanhMucCons)
                .HasForeignKey(d => d.MaDMCapTren);

            // Relationships
            HasMany(d => d.SanPhams)
                .WithRequired(s => s.DanhMucSanPham)
                .HasForeignKey(s => s.MaDM);

            HasOptional(d => d.NguoiDung)
                .WithMany(n => n.DanhMucSanPhams)
                .HasForeignKey(d => d.NguoiTao);
        }
    }
}
