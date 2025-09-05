using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("NhaCungCap")]
    public class NhaCungCap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaNCC { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenNCC { get; set; }

        [MaxLength(200)]
        public string DiaChi { get; set; }

        [MaxLength(15)]
        public string DienThoai { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public bool TrangThai { get; set; } = true;

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();
    }
}
