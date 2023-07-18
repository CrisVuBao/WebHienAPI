using System.Collections.Generic;
using System.Linq;
using WebApiNet5.Data;
using WebApiNet5.Models;

namespace WebApiNet5.Services
{
    public class HangHoaReponsitory : IHangHoaReponsitory
    {
        private MyDbContext _context;

        // Cần database, nên phải inject MyDbContext để lấy dữ liệu
        public HangHoaReponsitory(MyDbContext context)
        {
            _context = context;
        }

        public List<HangHoaModel> GetFull()
        {
            var resultFull = _context.HangHoas.Select(hhFull => new HangHoaModel
            {
                MaHH = hhFull.MaHHdb, // MaHH là thuộc tính, trường(field) của class C#, còn MaHHdb là của bảng trong database
                TenHH = hhFull.TenHHdb,
                MoTa = hhFull.MoTadb,
                DonGia  = hhFull.DonGiadb,
                GiamGia = hhFull.GiamGiadb,
            });
            return resultFull.ToList();
        }

        public List<HangHoaModel> GetAll(string search)
        {
            var allProduct = _context.HangHoas.Where(hh => hh.TenHHdb.Contains(search));

            var result = allProduct.Select(hh => new HangHoaModel { 
                MaHH = hh.MaHHdb,
                TenHH = hh.TenHHdb,
                DonGia = hh.DonGiadb,
                TenLoai = hh.Loaidb.TenLoai
            });

            return result.ToList();
        }

        public HangHoa Add(HangHoaModel hangHoaModel)
        {
            var _hangHoa = new HangHoaEntity // tạo mới 1 cái HangHoaEntiy bên trong class này để gọi, và dùng các thuộc tính trong HangHoaEntity
            {
                TenHHdb = hangHoaModel.TenHH, // TenHHdb là của HangHoaEntity (của sql server)
                MoTadb = hangHoaModel.MoTa,
                DonGiadb = hangHoaModel.DonGia,
                GiamGiadb = hangHoaModel.GiamGia
            };
            _context.Add(_hangHoa);
            _context.SaveChanges();


            return new HangHoa // Trả ra dữ liệu, hiển thị trên phần response của swagger
            {
                MaHangHoa = _hangHoa.MaHHdb,
                TenHangHoa = _hangHoa.TenHHdb,
                MoTa = _hangHoa.MoTadb,
                DonGia = _hangHoa.DonGiadb,
                GiamGia = _hangHoa.GiamGiadb
            };
        }
    }    
}
