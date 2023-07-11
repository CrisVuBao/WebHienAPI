using System;
using System.Collections;
using System.Collections.Generic;

namespace WebApiNet5.Data
{
    public enum TinhTrangDonHang
    {
        New = 0, Payment = 1, Complete = 2, Cancel = -1
    }
    public class DonHang
    {
        public Guid MaDH { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public int TinhTrangDonHang { get; set; }
        public string NguoiNhan { get; set; }
        public string DiaChiGiao { get; set; }
        public string SoDienThoai { get; set; }

        public ICollection<ChiTietDonHang> DonHangChiTiets { get; set; }

        public DonHang()
        {
            DonHangChiTiets = new List<ChiTietDonHang>();
        }
    }
}
