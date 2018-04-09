using LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getSlider()
        {
            IList<SanPham> list = null ;
            using (var pro = new MobileShopEntities())
            {
               list = pro.SanPhams
                    .OrderByDescending(p => p.NgayCapNhat)
                    .Take(3)
                    .ToList();
                return PartialView("_getSlider", list);
            }
            
        }
    }
}