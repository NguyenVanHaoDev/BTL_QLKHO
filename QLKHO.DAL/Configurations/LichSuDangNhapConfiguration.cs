using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class LichSuDangNhapConfiguration : EntityTypeConfiguration<LichSuDangNhap>
    {
        public LichSuDangNhapConfiguration()
        {
            // Table name
            ToTable("LichSuDangNhap");

            // Primary key
            HasKey(ls => ls.MaLSDN);

            // Properties
            Property(ls => ls.MaLSDN)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(ls => ls.MaND)
                .IsRequired();

            Property(ls => ls.TenDangNhap)
                .IsRequired()
                .HasMaxLength(50);

            Property(ls => ls.IPAddress)
                .HasMaxLength(45);

            Property(ls => ls.UserAgent)
                .HasMaxLength(500);

            Property(ls => ls.TrangThai)
                .IsRequired()
                .HasMaxLength(20);

            Property(ls => ls.ThoiGian)
                .IsRequired()
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            // Relationships
            HasRequired(ls => ls.NguoiDung)
                .WithMany(nd => nd.LichSuDangNhaps)
                .HasForeignKey(ls => ls.MaND);
        }
    }
}