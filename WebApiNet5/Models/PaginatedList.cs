using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiNet5.Models
{
    public class PaginatedList<T> : List<T> // PaginatedList ánh xạ tới kiểu T
    {
        // Field
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }

        // Contructor
        public PaginatedList(List<T> item, int count, int pageIndex, int pageSize) 
        {
            PageIndex = pageIndex;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);  
            AddRange(item); // thêm Collection of elements
        }

        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize) 
        {
            // phần các lệnh truy vấn từ CSDL
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
