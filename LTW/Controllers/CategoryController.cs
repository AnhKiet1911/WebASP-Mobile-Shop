using LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult getListCategory()
        {
            using (var cat = new MobileShopEntities())
            {
                var list = cat.HangSanXuats.ToList();
                return PartialView("_getListCategory", list);
            }
            
        }
    }
}