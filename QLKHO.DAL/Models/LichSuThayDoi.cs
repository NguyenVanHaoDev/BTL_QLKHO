using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("LichSuThayDoi")]
    public class LichSuThayDoi
    {
        [Key]
        public int MaLSTD { get; set; }

        [Required]
        [StringLength(50)]
        public string Bang { get; set; }

        [Required]
        public int MaBanGhi { get; set; }

        [Required]
        [StringLength(10)]
        public string ThaoTac { get; set; }

        public string DuLieuCu { get; set; }

        public string DuLieuMoi { get; set; }

        public int? NguoiThucHien { get; set; }

        [StringLength(45)]
        public string IPAddress { get; set; }

        public DateTime ThoiGian { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("NguoiThucHien")]
        public virtual NguoiDung NguoiDung { get; set; }
    }
}