using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiNet5.Models;
using WebApiNet5.Services;

namespace WebApiNet5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IHangHoaReponsitory _hangHoaReponsitory;

        public ProductsController(IHangHoaReponsitory hangHoaReponsitory)
        {
            _hangHoaReponsitory = hangHoaReponsitory; // _hangHoaRepository là biến được tạo để lấy các thuộc tính bên trong Interface IHangHoaRepository
        }

        [HttpGet("{id}")] // nếu thêm {id} thì khi trên swagger, phải bắt buộc nhập id
        public IActionResult GetFull(double? from, double? to, string sortBy) 
        {
            try
            {
                var resultFull = _hangHoaReponsitory.GetFull(from, to, sortBy);
                return Ok(resultFull);
            } catch
            {
                return BadRequest("Can not get product");
            }
        }

        [HttpGet]
        public IActionResult GetAllProducts(string search, double? from, double? to, string sortBy, int page = 1)
        {
            try
            {
                var resutl = _hangHoaReponsitory.GetAll(search, from, to, sortBy, page);
                return Ok(resutl);
            } catch
            {
                return BadRequest("Can not request!!!");
            }
        }

        [HttpPost]
        public IActionResult AddProducts(HangHoaModel hangHoaModel) { // Tham số hangHoaModel để nhập thông tin trên swagger
            try
            {
                var resultAdd = _hangHoaReponsitory.Add(hangHoaModel);
                return Ok(resultAdd);

            } catch
            {
                return BadRequest("Not Add Product!!!");
            }
        }

        
    }
}
