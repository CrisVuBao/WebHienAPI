using Microsoft.EntityFrameworkCore;
using WebApiNet5.Models;

namespace WebApiNet5.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        #region DbSet 
        public DbSet<HangHoaEntity> HangHoas { get; set; } // dbset đại diện cho một tập các thực thể hàng hóa
        
        public DbSet<Loai> Loais { get; set; }
        #endregion
    }
}
