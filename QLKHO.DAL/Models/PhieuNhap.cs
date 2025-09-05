using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("PhieuNhap")]
    public class PhieuNhap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaPN { get; set; }

        [Required]
        public DateTime NgayNhap { get; set; } = DateTime.Now;

        [Required]
        public int MaNCC { get; set; }

        [Required]
        public int NguoiTao { get; set; }

        public decimal TongTien { get; set; } = 0;

        [Required]
        [MaxLength(20)]
        public string TrangThai { get; set; } = "ChuaXacNhan";

        [MaxLength(500)]
        public string GhiChu { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MaNCC")]
        public virtual NhaCungCap NhaCungCap { get; set; }

        [ForeignKey("NguoiTao")]
        public virtual NguoiDung NguoiDung { get; set; }

        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
    }
}
