using System.Collections.Generic;
using WebApiNet5.Models;

namespace WebApiNet5.Services
{
    public interface IHangHoaReponsitory
    {
        List<HangHoaModel> GetFull();
        List<HangHoaModel> GetAll(string search);
        HangHoa Add(HangHoaModel hangHoaModel);
    }
}
