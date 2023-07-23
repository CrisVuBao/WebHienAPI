using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiNet5.Data
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public NguoiDungEntity NguoiDung { get; set; }

        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; } // được thu hồi hay chưa ?
        public DateTime IssuedAt { get; set; } // time tạo lúc nào
        public DateTime ExpitedAt { get; set; } // hết hạn lúc nào
    }
}   
