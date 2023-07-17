using System.Collections.Generic;
using System.Linq;
using WebApiNet5.Data;
using WebApiNet5.Models;

namespace WebApiNet5.Services
{
    // REPONSITORY PATTERN
    public class LoaiResponsitory : ILoaiResponsitory
    {
        private MyDbContext _context;

        public LoaiResponsitory(MyDbContext context)
        {
            _context = context;
        }

        public LoaiMV Add(LoaiModel loai)
        {
            var _loai = new Loai
            {
                TenLoai = loai.TenLoai // TenLoai là thuộc tính của bảng trong Entity(SQL) , loai.TenLoai là thuộc tính của c#(của Swagger), dùng để nhập dữ liệu mới trên Swaggger
            };
            _context.Add(_loai);
            _context.SaveChanges();

            return new LoaiMV // khi Add xong thì đoạn này sẽ trả về phần dữ liệu mới
            {
                Maloai = _loai.Maloai,
                TenLoai = _loai.TenLoai
            };
        }

        public void Delete(int id)
        {
            var _loai = _context.Loais.SingleOrDefault(lo => lo.Maloai == id);
            if (_loai != null)
            {
                _context.Remove(_loai);
                _context.SaveChanges() ;
            }
        }

        public List<LoaiMV> GetAll() // đoạn này để lấy hết data trong csdl
        {
            var loais = _context.Loais.Select(lo => new LoaiMV
            {
                Maloai = lo.Maloai,// MaLoai là mã mới (null) , còn lo.MaLoai là biến đã có dữ liệu từ csdl
                TenLoai = lo.TenLoai // TenLoai là tên mới (null), còn lo.TenLoai là đã có dữ liệu từ cơ sở dữ liệu(SQL Server)
            });
            return loais.ToList();
        }

        public LoaiMV GetById(int id)
        {
            var loai = _context.Loais.SingleOrDefault(lo => lo.Maloai == id);
            if (loai != null)
            {
                return new LoaiMV
                {
                    Maloai = loai.Maloai, // MaLoai của LoaiMV sẽ là dữ liệu từ database, bằng với loai.MaLoai khi nhập id trên swagger (loai.MaLoai là phần tìm theo id trên swagger)
                    TenLoai = loai.TenLoai
                };
            }
            return null;
        }

        public void Update(LoaiMV loai)
        {
            var loaiUpdate = _context.Loais.SingleOrDefault(loaiSingle => loaiSingle.Maloai == loai.Maloai);
            loai.TenLoai = loai.TenLoai;
            _context.SaveChanges();
        }
    }
}
