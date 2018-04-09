using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW.Models
{
    public partial class SanPham
    {  
        [AllowHtml]
        public string FullDesRaw { get; set; }
    }
}