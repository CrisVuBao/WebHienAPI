using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiNet5.Data
{
    // Entity tạo bảng người dùng
    [Table("NguoiDung")]
    public class NguoiDungEntity
    {
        [Key] // khóa chính
        public int Id { get; set; }
        [Required] // bắt buộc nhập
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        
    }
}
