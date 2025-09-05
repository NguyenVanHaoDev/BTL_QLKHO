using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKHO.DAL.Models
{
    [Table("DonViTinh")]
    public class DonViTinh
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaDVT { get; set; }

        [Required]
        [MaxLength(50)]
        public string TenDVT { get; set; }

        [Required]
        [MaxLength(10)]
        public string KyHieu { get; set; }

        [MaxLength(200)]
        public string MoTa { get; set; }

        public bool TrangThai { get; set; } = true;

        public System.DateTime NgayTao { get; set; } = System.DateTime.Now;

        public System.DateTime NgayCapNhat { get; set; } = System.DateTime.Now;

        // Navigation properties
        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
