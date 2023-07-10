using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiNet5.Data
{
    [Table("Loai")]
    public class Loai
    {
        [Key]
        public int Maloai { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenLoai { get; set; }

        public virtual ICollection<HangHoaEntity> HangHoas { get; set; }
    }
}
