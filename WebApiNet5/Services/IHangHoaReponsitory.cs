using System.Collections.Generic;
using WebApiNet5.Models;

namespace WebApiNet5.Services
{
    public interface IHangHoaReponsitory
    {
        List<HangHoaModel> GetFull(double? from, double? to, string sortBy);
        List<HangHoaModel> GetAll(string search, double? from, double? to, string sortBy, int page);
        HangHoa Add(HangHoaModel hangHoaModel);
        //void Update(int id);
    }
}
