using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiNet5.Models;
using WebApiNet5.Services;

namespace WebApiNet5.Controllers
{
    // xử lý thao tác với database bằng Dependency Injection(thêm các thành phần phụ thuộc)

    [Route("api/[controller]")]
    [ApiController]
    public class LoaiController : ControllerBase
    {
        private readonly ILoaiResponsitory _loaiReponsitory;

        // Constructor
        public LoaiController(ILoaiResponsitory loaiResponsitory)
        {
            _loaiReponsitory = loaiResponsitory;
        }

        [HttpGet]
        public IActionResult GetAll() {
            try
            {
                return Ok(_loaiReponsitory.GetAll());
            }catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) { 
            try
            {
                var data = _loaiReponsitory.GetById(id);
                if (data != null) { 
                    return Ok(data);
                }
                return NotFound("Not find!!!");
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Add(LoaiModel loai)
        {
            try
            {
                _loaiReponsitory.Add(loai);
                return Ok();
            }catch
            {
                return BadRequest("Is not Add!!!");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(LoaiMV loai, int id) 
        { 
            try
            {
                _loaiReponsitory.Update(loai);
                return Ok();
            }catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _loaiReponsitory.Delete(id);
                return Ok();
            }catch
            {
                return StatusCode(500);
            }
        }
    }
}
