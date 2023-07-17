using System.Collections.Generic;
using WebApiNet5.Models;

namespace WebApiNet5.Services
{
    public interface ILoaiResponsitory
    {
        List<LoaiMV> GetAll();
        LoaiMV GetById(int id);
        LoaiMV Add(LoaiModel loai);
        void Update(LoaiMV loai);
        void Delete(int id);
    }
}
