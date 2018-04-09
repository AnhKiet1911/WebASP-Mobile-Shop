using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LTW.Models
{
    public class OrderUser
    {
        public int OrderId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal? Total { get; set; }
        public int? TrangThai { get; set; }
    }
}