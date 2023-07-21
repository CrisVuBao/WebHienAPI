using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApiNet5.Data;
using WebApiNet5.Models;

namespace WebApiNet5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaisController : ControllerBase
    {
        private readonly MyDbContext _contextDB;

        public LoaisController(MyDbContext context) {
            _contextDB = context;
        }

        [HttpGet]
        public IActionResult GetAll() { 
            var dsLoai = _contextDB.Loais.ToList();
            return Ok(dsLoai);
        }

        [HttpGet("{id}")] // id là cái trên swagger, để nhập vào ô id và tìm đến mã loại
        public IActionResult GetById(int id)
        {
            var loaiOnly = _contextDB.Loais.SingleOrDefault(loaiSingle => loaiSingle.Maloai == id);
            if(loaiOnly != null)
            {
                return Ok(loaiOnly);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize] // phải phân quyền, để khi đăng nhập đúng là admin thì mới được thêm loại hàng hóa
        public IActionResult CreateNew(LoaiModel model)
        {
            try
            {
                var loaiNew = new Loai
                {
                    // đoạn mã này lấy giá trị của model.TenLoai và gán nó cho thuộc tính TenLoai của đối tượng Loai mới được tạo.
                    TenLoai = model.TenLoai
                };
                _contextDB.Add(loaiNew); // thêm dữ liệu ở trong loaiNew vào database
                _contextDB.SaveChanges();
                return Ok(loaiNew);
            } catch
            {
                return BadRequest("Is not ADD");
            }

        }

        [HttpPut("{id}")]
        public IActionResult UpdateLoaiById(int id, LoaiModel model)
        {
            // updateLoai để tìm TenLoai theo id, MaLoai
            var updateLoai = _contextDB.Loais.SingleOrDefault(loaiSingle => loaiSingle.Maloai == id);
            if (updateLoai != null)
            {
                updateLoai.TenLoai = model.TenLoai;
                _contextDB.SaveChanges();
                return Ok(updateLoai);
            }
            return BadRequest("Not Update");
        }

        [HttpDelete("{name}")]
        public IActionResult DeleteLoaiByName(string name) {
            var getDelName = _contextDB.Loais.SingleOrDefault(loaiSingle => loaiSingle.TenLoai == name);

            if(getDelName != null)
            {
                _contextDB.Remove(getDelName);
                _contextDB.SaveChanges();
                return Ok("Remove Ok :)");
            }
            return BadRequest("Not remove !!!");
        }
    }
}
