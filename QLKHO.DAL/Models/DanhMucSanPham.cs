using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("DanhMucSanPham")]
    public class DanhMucSanPham
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaDM { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenDM { get; set; }

        public int? MaDMCapTren { get; set; }

        [MaxLength(500)]
        public string MoTa { get; set; }

        public int ThuTuHienThi { get; set; } = 0;

        public bool TrangThai { get; set; } = true;

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public int? NguoiTao { get; set; }

        // Navigation properties
        [ForeignKey("MaDMCapTren")]
        public virtual DanhMucSanPham DanhMucCha { get; set; }

        public virtual ICollection<DanhMucSanPham> DanhMucCons { get; set; } = new List<DanhMucSanPham>();

        [ForeignKey("NguoiTao")]
        public virtual NguoiDung NguoiDung { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();

        public override string ToString()
        {
            return TenDM;
        }
    }
}
