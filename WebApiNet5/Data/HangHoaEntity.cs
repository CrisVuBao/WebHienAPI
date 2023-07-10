using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiNet5.Data
{
    [Table("HangHoa")]
    public class HangHoaEntity
    {
        [Key]
        public Guid MaHH { get; set; }

        [Required] // bắt buộc nhập
        [MaxLength(100)]
        public string TenHH { get; set; }
        public string MoTa { get; set; }

        [Range(0, double.MaxValue)]
        public int DonGia { get; set; }
        public byte GiamGia { get; set; }
        public int? MaLoai { get; set; }
        [ForeignKey("MaLoai")]
        public Loai Loai { get; set; }
    }
}
