using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiNet5.Models;

namespace WebApiNet5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoaController : ControllerBase
    {
        public static List<HangHoa> hangHoas = new List<HangHoa>();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(hangHoas);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                // LINQ [Object] : truy vấn trên mảng object. Query
                var hangHoa = hangHoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
                if (hangHoa == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(hangHoa);
                }
            } catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Create(HangHoaVM hangHoaVM)
        {
            var hanghoa = new HangHoa()
            {
                MaHangHoa = Guid.NewGuid(),
                TenHangHoa = hangHoaVM.TenHangHoa,
                DonGia = hangHoaVM.DonGia
            };
            hangHoas.Add(hanghoa);
            return Ok(new
            {
                Success = true, Data = hanghoa
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, HangHoa hangHoaUpdate)
        {
            try
            {
                var hangHoa = hangHoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
                if(hangHoa == null)
                {
                    return NotFound();
                }
                else {
                    // Update

                    // khi qua biến hangHoaUpdate được tạo trong phương thức update, thì tên hàng hóa cũ sẽ sửa thành tên hàng hóa mới
                    hangHoa.TenHangHoa = hangHoaUpdate.TenHangHoa;
                    // cũng giống tên hàng hóa
                    hangHoa.DonGia = hangHoaUpdate.DonGia;
                    return Ok(
                        new
                            {
                                data = hangHoa
                            }
                        ); 
                }
            } catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var hangHoaDel = hangHoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id)); // hangHoaDel = với id,mã hh. là nơi chứa dữ liệu của hàng hóa
                if(hangHoaDel == null)
                {
                    return BadRequest();
                }
                else
                {
                    hangHoas.Remove(hangHoaDel);
                    return Ok();
                }
            } catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
