using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApiNet5.Models;

namespace WebApiNet5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XuLyXeController : ControllerBase
    {
        public static List<Xe> dsXe = new List<Xe>();

        [HttpPost]
        public IActionResult Create(Xe xe)
        {
            var xeVip = new Xe();
            if (xe == null)
            {
                return BadRequest();
            }
            else
            {
                xe.MaXe = xeVip.MaXe;
                xe.TenXe = xeVip.TenXe;
                xe.GiaTien = xeVip.GiaTien;
                dsXe.Add(xeVip);
                return Ok(
                    new
                    {
                        data = xeVip
                    }
                    );
            }
        }
    }
}
