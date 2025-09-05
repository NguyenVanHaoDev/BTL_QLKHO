using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("ChiTietPhieuNhap")]
    public class ChiTietPhieuNhap
    {
        [Key, Column(Order = 0)]
        public int MaPN { get; set; }

        [Key, Column(Order = 1)]
        public int MaSP { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public decimal DonGia { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal ThanhTien { get; set; }

        // Navigation properties
        [ForeignKey("MaPN")]
        public virtual PhieuNhap PhieuNhap { get; set; }

        [ForeignKey("MaSP")]
        public virtual SanPham SanPham { get; set; }
    }
}
