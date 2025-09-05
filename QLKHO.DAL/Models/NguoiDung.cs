using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key]
        public int MaND { get; set; }

        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(15)]
        public string DienThoai { get; set; }

        [Required]
        [StringLength(20)]
        public string VaiTro { get; set; } = "NhanVien";

        public bool TrangThai { get; set; } = true;

        public int SoLanDangNhapSai { get; set; } = 0;

        public bool KhoaTaiKhoan { get; set; } = false;

        public DateTime? ThoiGianKhoa { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public DateTime? NgayDangNhapCuoi { get; set; }

        // Navigation properties
        public virtual ICollection<DanhMucSanPham> DanhMucSanPhams { get; set; }
        public virtual ICollection<SanPham> SanPhams { get; set; }
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }
        public virtual ICollection<PhieuXuat> PhieuXuats { get; set; }
        public virtual ICollection<LichSuDangNhap> LichSuDangNhaps { get; set; }
        public virtual ICollection<LichSuThayDoi> LichSuThayDois { get; set; }
    }
}