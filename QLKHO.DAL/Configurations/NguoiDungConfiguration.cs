using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class NguoiDungConfiguration : EntityTypeConfiguration<NguoiDung>
    {
        public NguoiDungConfiguration()
        {
            // Table name
            ToTable("NguoiDung");

            // Primary key
            HasKey(nd => nd.MaND);

            // Properties
            Property(nd => nd.MaND)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(nd => nd.TenDangNhap)
                .IsRequired()
                .HasMaxLength(50);

            Property(nd => nd.MatKhau)
                .IsRequired()
                .HasMaxLength(255);

            Property(nd => nd.HoTen)
                .IsRequired()
                .HasMaxLength(100);

            Property(nd => nd.Email)
                .HasMaxLength(100);

            Property(nd => nd.DienThoai)
                .HasMaxLength(15);

            Property(nd => nd.VaiTro)
                .IsRequired()
                .HasMaxLength(20);

            Property(nd => nd.TrangThai)
                .IsRequired();

            Property(nd => nd.SoLanDangNhapSai)
                .IsRequired();

            Property(nd => nd.KhoaTaiKhoan)
                .IsRequired();

            Property(nd => nd.NgayTao)
                .IsRequired()
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            Property(nd => nd.NgayCapNhat)
                .IsRequired()
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            // Relationships
            HasMany(nd => nd.DanhMucSanPhams)
                .WithOptional(dm => dm.NguoiDung)
                .HasForeignKey(dm => dm.NguoiTao);

            HasMany(nd => nd.SanPhams)
                .WithOptional(sp => sp.NguoiDung)
                .HasForeignKey(sp => sp.NguoiTao);

            HasMany(nd => nd.PhieuNhaps)
                .WithRequired(pn => pn.NguoiDung)
                .HasForeignKey(pn => pn.NguoiTao);

            HasMany(nd => nd.PhieuXuats)
                .WithRequired(px => px.NguoiDung)
                .HasForeignKey(px => px.NguoiTao);

            HasMany(nd => nd.LichSuDangNhaps)
                .WithRequired(ls => ls.NguoiDung)
                .HasForeignKey(ls => ls.MaND);

            HasMany(nd => nd.LichSuThayDois)
                .WithOptional(ls => ls.NguoiDung)
                .HasForeignKey(ls => ls.NguoiThucHien);
        }
    }
}