using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("LichSuDangNhap")]
    public class LichSuDangNhap
    {
        [Key]
        public int MaLSDN { get; set; }

        [Required]
        public int MaND { get; set; }

        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [StringLength(45)]
        public string IPAddress { get; set; }

        [StringLength(500)]
        public string UserAgent { get; set; }

        [Required]
        [StringLength(20)]
        public string TrangThai { get; set; }

        public DateTime ThoiGian { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("MaND")]
        public virtual NguoiDung NguoiDung { get; set; }
    }
}