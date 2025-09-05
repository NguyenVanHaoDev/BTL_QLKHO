using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaSP { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenSP { get; set; }

        [Required]
        public int MaDM { get; set; }

        [Required]
        public int MaDVT { get; set; }

        [Required]
        public decimal GiaNhap { get; set; }

        [Required]
        public decimal GiaBan { get; set; }

        public int SoLuongTon { get; set; } = 0;

        public int SoLuongToiThieu { get; set; } = 10;

        [MaxLength(500)]
        public string MoTa { get; set; }

        public bool TrangThai { get; set; } = true;

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public int? NguoiTao { get; set; }

        // Navigation properties
        [ForeignKey("MaDM")]
        public virtual DanhMucSanPham DanhMucSanPham { get; set; }

        [ForeignKey("MaDVT")]
        public virtual DonViTinh DonViTinh { get; set; }

        [ForeignKey("NguoiTao")]
        public virtual NguoiDung NguoiDung { get; set; }

        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; } = new List<ChiTietPhieuXuat>();
    }
}
