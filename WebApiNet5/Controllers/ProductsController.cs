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

        [HttpGet]
        public IActionResult GetFull() 
        {
            try
            {
                var resultFull = _hangHoaReponsitory.GetFull();
                return Ok(resultFull);
            } catch
            {
                return BadRequest("Can not get product");
            }
        }

        [HttpGet("{search}")]
        public IActionResult GetAllProducts(string search)
        {
            try
            {
                var resutl = _hangHoaReponsitory.GetAll(search);
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
