using System.Net.NetworkInformation;

namespace WebApiNet5.Models
{
    // Tạo class AppSetting này, để inject(nhúng) vào class controller, hoặc class service cho dễ dàng
    public class AppSetting
    {
        public string SecretKey { get; set; }
    }
}
