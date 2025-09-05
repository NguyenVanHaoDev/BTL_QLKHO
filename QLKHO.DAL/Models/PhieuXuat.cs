using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("PhieuXuat")]
    public class PhieuXuat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaPX { get; set; }

        [Required]
        public DateTime NgayXuat { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(100)]
        public string NguoiNhan { get; set; }

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
        [ForeignKey("NguoiTao")]
        public virtual NguoiDung NguoiDung { get; set; }

        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; } = new List<ChiTietPhieuXuat>();
    }
}
