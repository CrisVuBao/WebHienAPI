using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApiNet5.Data;
using WebApiNet5.Models;

namespace WebApiNet5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context ;
        private readonly AppSetting _appSettings;

        // Contructor
        public UserController(MyDbContext context, IOptionsMonitor<AppSetting> optionsMonitor) 
        {
            _context = context; // tạo biến _context bên UserController gán = context để lấy dữ liệu bên database(MyDbContext)
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        public IActionResult Validate(LoginModel model) 
        {
            var user = _context.NguoiDungs.SingleOrDefault(p => p.UserName == model.UserName && p.Password == model.Password);
            if (user == null) // không đúng người dùng
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username/password"
                });
            }

            // Cấp Token     Ngược lại(nếu ko null) thì cấp token
            return Ok(new ApiResponse {
                Success = true,
                Message = "Authenticate Success",
                Data = GenerateToken(user) // Phần này sẽ trả ra dòng token trên swagger
            });
        }

        private string GenerateToken(NguoiDungEntity nguoiDungEntity) // Tạo ra token
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                // Identity: là API hỗ trợ trong giao diện người dùng về chức năng đăng nhập
                Subject = new ClaimsIdentity(new[] { // new []: là tạo mới 1 mảng danh sách các Claim

                    new Claim(ClaimTypes.Name, nguoiDungEntity.HoTen), // Claim: xác nhận
                    new Claim(ClaimTypes.Email, nguoiDungEntity.Email),
                    new Claim("UserName", nguoiDungEntity.UserName),
                    new Claim("Id", nguoiDungEntity.Id.ToString()),

                    // Roles
                    new Claim("TokenId", Guid.NewGuid().ToString())

                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                
                // Ký xác nhận
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                    (secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

    }
}
