using Microsoft.EntityFrameworkCore;
using System;
using WebApiNet5.Services;

namespace WebApiNet5.Models
{
    [Keyless]
    public class HangHoaVM
    {
        public string TenHangHoa { get; set; }
        public string MoTa { get; set; }
        public double DonGia { get; set; }
        public byte GiamGia { get; set; }
    }

    public class HangHoa : HangHoaVM
    {
        public Guid MaHangHoa{ get; set; }
    }

    public class HangHoaModel
    {
        public Guid MaHH { get; set; }
        public string TenHH { get; set; }
        public string MoTa { get; set; }
        public int DonGia { get; set; }
        public byte GiamGia { get; set; }
        public string TenLoai { get; set; }
    }
}
