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
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Validate(LoginModel model) 
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


            var token = await GenerateToken(user);
            // Cấp Token     Ngược lại(nếu ko null) thì cấp token
            return Ok(new ApiResponse {
                Success = true,
                Message = "Authenticate Success",
                Data = token // Phần này sẽ trả ra dòng token trên swagger
            });
        }

        private async Task<TokenModel> GenerateToken(NguoiDungEntity nguoiDungEntity) // Tạo ra token
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                // Identity: là API hỗ trợ trong giao diện người dùng về chức năng đăng nhập
                Subject = new ClaimsIdentity(new[] { // new []: là tạo mới 1 mảng danh sách các Claim

                    new Claim(ClaimTypes.Name, nguoiDungEntity.HoTen), // Claim: xác nhận
                    new Claim(JwtRegisteredClaimNames.Email, nguoiDungEntity.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, nguoiDungEntity.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", nguoiDungEntity.UserName),
                    new Claim("Id", nguoiDungEntity.Id.ToString()),

                    // Roles

                }),
                Expires = DateTime.UtcNow.AddSeconds(20),
                
                // Ký xác nhận
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                    (secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken =  jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            // lưu vào database
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = nguoiDungEntity.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpitedAt = DateTime.UtcNow.AddHours(1)
            };

            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken() // phương thức này để khi kết hạn Token, thì sẽ tự refresh lại cung cấp token mới, để người dùng đỡ phải login lại nhiều lần
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                // Tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                // ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false, // ko kiểm tra token hết hạn

            };

            try
            {
                // Check 1: AccessToken valid format (xem access token có đúng format hay không)
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                // Check 2: Check alg (check thuật toán)
                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals
                        (SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result) { // if false
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                // Check 3: Check AccessToken expire (check xem token hết hiệu lực lúc nào)
                var utcExpiteDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpiteDate);
                if (expireDate > DateTime.UtcNow) // nếu time này > time hiện tại
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });
                }

                // Check 4: Check refreshtoken exist in DB (kiểm tra xem token trong DB có bằng với refresh token mới gửi lên lại ko)
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if(storedToken == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exits"
                    });
                }

                // Check 5: check refreshToken is used (check xem refreshToken đã được sử dụng hay chưa)
                if(storedToken.IsUsed) 
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }
                if(storedToken.IsRevoked)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                // Check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if(storedToken.JwtId != jti)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                // Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                // Create new token
                var user = await _context.NguoiDungs.SingleOrDefaultAsync(nd => nd.Id == storedToken.UserId);
                var token = await GenerateToken(user);

                // if true
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Renew token success",
                    Data = token
                });

            } catch(Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Something Wrong !!!"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpiteDate)
        {
            var dateTimeInterval = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpiteDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}
