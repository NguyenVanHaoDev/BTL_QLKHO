using System.Data.Entity.ModelConfiguration;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Configurations
{
    public class LichSuThayDoiConfiguration : EntityTypeConfiguration<LichSuThayDoi>
    {
        public LichSuThayDoiConfiguration()
        {
            // Table name
            ToTable("LichSuThayDoi");

            // Primary key
            HasKey(ls => ls.MaLSTD);

            // Properties
            Property(ls => ls.MaLSTD)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(ls => ls.Bang)
                .IsRequired()
                .HasMaxLength(50);

            Property(ls => ls.MaBanGhi)
                .IsRequired();

            Property(ls => ls.ThaoTac)
                .IsRequired()
                .HasMaxLength(10);

            Property(ls => ls.IPAddress)
                .HasMaxLength(45);

            Property(ls => ls.ThoiGian)
                .IsRequired()
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            // Relationships
            HasOptional(ls => ls.NguoiDung)
                .WithMany(nd => nd.LichSuThayDois)
                .HasForeignKey(ls => ls.NguoiThucHien);
        }
    }
}