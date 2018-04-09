using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW.Areas.Admin.Controllers
{
    public class MemberController : Controller
    {
        // GET: Admin/Member
        public ActionResult Index()
        {
            return View();
        }
    }
}