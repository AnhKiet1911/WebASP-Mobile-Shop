using LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace LTW.Areas.Admin.Controllers
{
    public class CateController : Controller
    {
        // GET: Admin/Cate
        public ActionResult Index()
        {
            return View();
        }
        // lấy danh sách categrory và mở View getCate
        public ActionResult getCate()
        {
            using (var pro = new MobileShopEntities())
            {
                var list = pro.HangSanXuats
                    .OrderByDescending(p => p.ID)
                    .ToList();
                return PartialView("getCate", list);
            }
        }
        public ActionResult addCate()
        {
            return View();
        }

        //Thêm categrory
        [HttpPost]
        public ActionResult addCate(string TenHang)
        {
            //kiểm tra tên hãng nhập vào có bằng null hay không
            if(string.IsNullOrEmpty(TenHang))
            {
                ModelState.AddModelError("TenHang", "Bạn cần nhập tên hãng");
            }
            if(ModelState.IsValid)
            {
                //tạo list cate để xuất ra view khi add thành công
                List<HangSanXuat> list = null;
                using (var pro = new MobileShopEntities())
                {
                    //lấy danh sách cate ra để so sánh xem đã tồn tại chưa
                    var l = pro.HangSanXuats
                   .OrderByDescending(p => p.ID)
                   .ToList();
                    foreach (var item in l)
                    {
                        if (TenHang == item.TenHang)
                        {
                            ModelState.AddModelError("TenHang", "Tên hãng đã tồn tại");
                            return View();
                        }
                    }
                    // tiến hành add cate và truyền qua view
                    HangSanXuat hang = new HangSanXuat { TenHang = TenHang };
                    pro.HangSanXuats.Add(hang);
                    pro.SaveChanges();
                    list = pro.HangSanXuats.ToList();
                    return PartialView("getCate", list);
                }
            }
            return View();

        }
        //xóa category
        public ActionResult DeleteCat(int CatID)
        {
            using (var pro = new MobileShopEntities())
            {   
                //lấy cate cần xóa bằng id
                var list = pro.HangSanXuats
                    .Where(p => p.ID == CatID)
                    .FirstOrDefault();
                //kiểm tra lấy thành công hay không
                if (list != null)
                {   
                    //tiến hành remove
                    pro.HangSanXuats.Remove(list);
                    pro.SaveChanges();
                }
                //lấy list cate và truyền qua view
                var l = pro.HangSanXuats.ToList();
                return PartialView("getCate", l);
            }
        }
        //thay đổi catefory
        //Hiện thị cate cần thay đổi
        public ActionResult EditCat(int CatID)
        {
            HangSanXuat hang = null;
            using (var pro = new MobileShopEntities())
            {
                hang = pro.HangSanXuats
                    .Where(p => p.ID == CatID)
                    .FirstOrDefault();
              
            }
            return View(hang);
        }
        //nhận vào cate mới để tiến hành thay đổi
        [HttpPost]
        public ActionResult EditCat(HangSanXuat hangNew)
        {
            //kiểm tra null hay không 
            if (string.IsNullOrEmpty(hangNew.TenHang))
            {
                //truyền vào validate
                ModelState.AddModelError("TenHang", "Bạn cần nhập tên hãng");
            }
            else
            {
                using (var pro = new MobileShopEntities())
                {
                    //kiểm tra tồn tại của cate mới
                    var l = pro.HangSanXuats
                    .OrderByDescending(p => p.ID)
                    .ToList();
                    foreach (var item in l)
                    {
                        if (hangNew.TenHang == item.TenHang)
                        {
                            ModelState.AddModelError("TenHang", "Tên hãng đã tồn tại");
                            return View();
                        }
                    }
                    //lấy cate cần sửa
                    List<HangSanXuat> list = null;
                    var hangOld = pro.HangSanXuats
                        .Where(p => p.ID == hangNew.ID)
                        .FirstOrDefault();
                    //kiểm tra lấy thành công hay không
                    if (hangOld != null)
                    {
                        //tiến hành thay đổi
                        hangOld.TenHang = hangNew.TenHang;
                        pro.Entry(hangOld).State = EntityState.Modified;
                        pro.SaveChanges();
                    }
                    list = pro.HangSanXuats.ToList();

                    return View("getCate", list);
                }
            }
           
            return View();
        }
    }
}