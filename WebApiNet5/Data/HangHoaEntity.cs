using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiNet5.Data
{
    [Table("HangHoa")]
    public class HangHoaEntity
    {
        [Key]
        public Guid MaHHdb { get; set; }

        [Required] // bắt buộc nhập
        [MaxLength(100)]
        public string TenHHdb { get; set; }
        public string MoTadb { get; set; }

        [Range(0, double.MaxValue)]
        public int DonGiadb { get; set; }
        public byte GiamGiadb { get; set; }
        public int? MaLoaidb { get; set; }
        [ForeignKey("MaLoai")]
        public Loai Loaidb { get; set; }

        public ICollection<ChiTietDonHang> DonHangChiTiets { get; set; }

        public HangHoaEntity()
        {
            DonHangChiTiets = new HashSet<ChiTietDonHang>(); // t ko muốn nó hiện null, nên nếu chưa có dữ liệu thì để là danh sách rỗng
        }
    }
}
