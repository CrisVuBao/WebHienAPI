using System;

namespace WebApiNet5.Data
{
    public class ChiTietDonHang
    {
        public Guid MaHH { get; set; }
        public Guid MaDH { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public byte GiamGia { get; set; }

        // Relationship
        public DonHang DonHang { get; set; }
        public HangHoaEntity HangHoa { get; set; }
    }
}
