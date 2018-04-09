using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LTW.Models
{
    public class OrderDetailPro
    {
        public int OrderId { get; set; }
        public string Tensanpham { get; set; }
        public int? Soluong { get; set; }
        public Decimal? Price { get; set; }
        public Decimal? Amount { get; set; }
        public int ProId { get; set; }
        
    }
}