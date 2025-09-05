using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("ChiTietPhieuXuat")]
    public class ChiTietPhieuXuat
    {
        [Key, Column(Order = 0)]
        public int MaPX { get; set; }

        [Key, Column(Order = 1)]
        public int MaSP { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public decimal DonGia { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal ThanhTien { get; set; }

        // Navigation properties
        [ForeignKey("MaPX")]
        public virtual PhieuXuat PhieuXuat { get; set; }

        [ForeignKey("MaSP")]
        public virtual SanPham SanPham { get; set; }
    }
}
