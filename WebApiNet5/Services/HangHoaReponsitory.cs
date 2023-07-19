using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebApiNet5.Data;
using WebApiNet5.Models;

namespace WebApiNet5.Services
{
    public class HangHoaReponsitory : IHangHoaReponsitory
    {
        private MyDbContext _context;
        public static int PAGE_SIZE {get; set; } = 5; // cho phép thay đổi value của properties

        // Cần database, nên phải inject MyDbContext để lấy dữ liệu
        public HangHoaReponsitory(MyDbContext context)
        {
            _context = context;
        }

        public List<HangHoaModel> GetFull(double? from, double? to, string sortBy)
        {
            var resultFull = _context.HangHoas.Select(hhFull => new HangHoaModel
            {
                MaHH = hhFull.MaHH, // MaHH là thuộc tính, trường(field) của class C#, còn MaHHdb là của bảng trong database
                TenHH = hhFull.TenHH,
                MoTa = hhFull.MoTa,
                DonGia  = hhFull.DonGia,
                GiamGia = hhFull.GiamGia,
            });
            return resultFull.ToList();
        }

        public List<HangHoaModel> GetAll(string search, double? from, double? to, string sortBy, int page = 1)
        {
            var allProduct = _context.HangHoas.Include(hh => hh.Loai).AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(search))
            { // Ko excute nếu ko nhập vào ô search
                allProduct = allProduct.Where(hh => hh.TenHH.Contains(search));
            }
            if (from.HasValue)
            {
                allProduct = allProduct.Where(hh => hh.DonGia >= from); // lấy đơn giá từ entity(database sql) hangHoas | lấy giá >= số nhập vào
            }
            if(to.HasValue)
            {
                allProduct = allProduct.Where(hh => hh.DonGia <= to); // lấy đơn giá nhỏ <= số nhập vào
            }
            #endregion

            #region Sorting
            // asc: tăng dần
            // desc: giảm dần

            allProduct = allProduct.OrderBy(hh => hh.TenHH);

            if(!string.IsNullOrEmpty(sortBy))
            {
                switch(sortBy)
                {
                    case "gia_asc":
                        allProduct = allProduct.OrderBy(hh => hh.DonGia); // hiện danh sách giá hàng hóa tăng dần
                        break;
                    case "gia_desc":
                        allProduct = allProduct.OrderByDescending(hh => hh.DonGia); // hiện danh sách giá hàng hóa giảm dần
                        break;
                }
            }
            #endregion

            //#region Paging
            //allProduct = allProduct.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE); // skip là bỏ qua index = 0, chỉ lấy từ 1
            //#endregion

            //var result = allProduct.Select(hh => new HangHoaModel { 
            //    MaHH = hh.MaHH,
            //    TenHH = hh.TenHH,
            //    DonGia = hh.DonGia,
            //    TenLoai = hh.Loai.TenLoai
            //});
            // return result.ToList();



            // đây là kiểu tạo paging để hiểu cách hoạt động của NugetPagkage 
            var result = PaginatedList<HangHoaEntity>.Create(allProduct, page, PAGE_SIZE); // trả về các hàng hóa trong sql server
            return result.Select(hh => new HangHoaModel
            {
                MaHH = hh.MaHH,
                TenHH = hh.TenHH,
                MoTa = hh.MoTa,
                DonGia = hh.DonGia,
                GiamGia = hh.GiamGia,
                TenLoai = hh.Loai?.TenLoai
            }).ToList();
        }

        public HangHoa Add(HangHoaModel hangHoaModel)
        {
            var _hangHoa = new HangHoaEntity // tạo mới 1 cái HangHoaEntiy bên trong class này để gọi, và dùng các thuộc tính trong HangHoaEntity
            {
                TenHH = hangHoaModel.TenHH, // TenHHdb là của HangHoaEntity (của sql server)
                MoTa = hangHoaModel.MoTa,
                DonGia = hangHoaModel.DonGia,
                GiamGia = hangHoaModel.GiamGia
            };
            _context.Add(_hangHoa);
            _context.SaveChanges();


            return new HangHoa // Trả ra dữ liệu, hiển thị trên phần response của swagger
            {
                MaHangHoa = _hangHoa.MaHH,
                TenHangHoa = _hangHoa.TenHH,
                MoTa = _hangHoa.MoTa,
                DonGia = _hangHoa.DonGia,
                GiamGia = _hangHoa.GiamGia
            };
        }


        //public void Update(HangHoaModel _hangHoaModel)
        //{
        //    var resultUpdate = _context.HangHoas.SingleOrDefault(hh => hh.MaHH == _hangHoaModel.MaHH); // tạo hh.MaHH là của bảng HangHoaEntity để so sánh với _hangHoaModel.MaHH của C# được nhập trên swagger
        //    _hangHoaModel.TenHH = _hangHoaModel.TenHH;
        //    _hangHoaModel.MoTa = _hangHoaModel.MoTa;
        //    _hangHoaModel.DonGia = _hangHoaModel.DonGia;
        //    _hangHoaModel.GiamGia = _hangHoaModel.GiamGia;
        //}
    }    
}
