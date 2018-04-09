using LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTW.Areas.Admin.Lib;
using System.Data.Entity;

namespace LTW.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Admin/Products
        public ActionResult Index()
        {
            return View();
        }
        //lấy tất cả sản phẩm là mobile
        public ActionResult getAllMobile()
        {
            using (var pro = new MobileShopEntities())
            {
                //lấy danh sách theo ngày cập nhật rồi truyền qua view getAllMobile
                var list = pro.SanPhams
                    .OrderByDescending(p => p.NgayCapNhat)
                    .Where(p => p.IDLoai == 1)
                    .Where(p => p.TinhTrang == 0)
                    .ToList();
                return PartialView("getAllMobile", list);
            }
        }
        //lấy tất cả sản phẩm là tablet
        public ActionResult getAllTablet()
        {
            using (var pro = new MobileShopEntities())
            {
                //lấy danh sách theo ngày cập nhật rồi truyền qua view getAllTablet
                var list = pro.SanPhams
                    .OrderByDescending(p => p.NgayCapNhat)
                    .Where(p => p.IDLoai == 2)
                    .Where(p => p.TinhTrang == 0)
                    .ToList();
                return PartialView("getAllTablet", list);
            }
        }
        public ActionResult addProductMobile()
        {
            return View();
        }
        //Thêm một sản phẩm mobile mới
        [HttpPost]
        public ActionResult addProductMobile(SanPham p, HttpPostedFileBase imgLg, HttpPostedFileBase imgSm)
        {
            //tiến hành validate 
            if (p.MoTaNgan == null)
            {
                p.MoTaNgan = string.Empty;
            }
            if (p.FullDesRaw == null)
            {
                p.FullDesRaw = string.Empty;
            }
            if (string.IsNullOrEmpty(p.TenSanPham))
            {
                ModelState.AddModelError("TenSanPham", "Bạn cần nhập tên hãng");
            }
            if (string.IsNullOrEmpty(p.SoLuong.ToString()))
            {
                ModelState.AddModelError("SoLuong", "Bạn cần nhập số lượng");
            }
            if (string.IsNullOrEmpty(p.Gia.ToString()))
            {
                ModelState.AddModelError("Gia", "Bạn cần nhập giá");
            }
            if (string.IsNullOrEmpty(p.MoTaNgan))
            {
                ModelState.AddModelError("MoTaNgan", "Bạn cần nhập mô tả");
            }
            if (string.IsNullOrEmpty(p.FullDesRaw))
            {
                ModelState.AddModelError("FullDesRaw", "Bạn cần nhập mô tả chi tiết");
            }
            p.MoTaChiTiet = p.FullDesRaw;
            //kiểm tra người dùng đã nhập hình minh họa chưa
            if (imgLg == null || imgSm == null)
            {
                ViewBag.img = "Vui lòng nhập đủ 2 hình";
            }
            else
            {
                using (var dc = new MobileShopEntities())
                {
                    //lấy danh sách sản phẩm đã có để so sánh xem sản phẩm mới đã tồn tại chưa
                    var l = dc.SanPhams
                   .ToList();
                    foreach (var item in l)
                    {
                        if (p.TenSanPham == item.TenSanPham)
                        {
                            ModelState.AddModelError("TenSanPham", "Tên sản phẩm đã tồn tại");
                            return View();
                        }
                    }
                    //tiến hành add sản phẩm với IDLoai = 1
                    p.MoTaChiTiet = p.FullDesRaw;
                    p.IDLoai = 1;
                    p.NgayCapNhat = DateTime.Now;
                    dc.SanPhams.Add(p);
                    dc.SaveChanges();
                }
                //Lưu ảnh minh họa
                Helper.SaveProductImg(p.ID, Server.MapPath("~"), imgLg, imgSm);
                return RedirectToAction("getAllMobile");
            }
            return View();
        }
        public ActionResult addProductTablet()
        {
            return View();
        }
        //tương tự addProductMobile  thay thế IDLoai = 2
        [HttpPost]
        public ActionResult addProductTablet(SanPham p, HttpPostedFileBase imgLg, HttpPostedFileBase imgSm)
        {
            if (p.MoTaNgan == null)
            {
                p.MoTaNgan = string.Empty;
            }
            if (p.FullDesRaw == null)
            {
                p.FullDesRaw = string.Empty;
            }
            if (string.IsNullOrEmpty(p.TenSanPham))
            {
                ModelState.AddModelError("TenSanPham", "Bạn cần nhập tên hãng");
            }
            if (string.IsNullOrEmpty(p.SoLuong.ToString()))
            {
                ModelState.AddModelError("SoLuong", "Bạn cần nhập số lượng");
            }
            if (string.IsNullOrEmpty(p.Gia.ToString()))
            {
                ModelState.AddModelError("Gia", "Bạn cần nhập giá");
            }
            if (string.IsNullOrEmpty(p.MoTaNgan))
            {
                ModelState.AddModelError("MoTaNgan", "Bạn cần nhập mô tả");
            }
            if (string.IsNullOrEmpty(p.FullDesRaw))
            {
                ModelState.AddModelError("FullDesRaw", "Bạn cần nhập mô tả chi tiết");
            }
            p.MoTaChiTiet = p.FullDesRaw;
            if (imgLg == null || imgSm == null)
            {
                ViewBag.img = "Vui lòng nhập đủ 2 hình";
            }
            else
            {
                using (var dc = new MobileShopEntities())
                {
                    var l = dc.SanPhams
                   .ToList();
                    foreach (var item in l)
                    {
                        if (p.TenSanPham == item.TenSanPham)
                        {
                            ModelState.AddModelError("TenSanPham", "Tên sản phẩm đã tồn tại");
                            return View();
                        }
                    }
                    
                    p.IDLoai = 2;
                    dc.SanPhams.Add(p);
                    dc.SaveChanges();
                }
                Helper.SaveProductImg(p.ID, Server.MapPath("~"), imgLg, imgSm);
                return RedirectToAction("getAllTablet");
            }
            return View();
        }
        //xem thông tin của sản phẩm
        public ActionResult Details(int ID)
        {

            using (var pro = new MobileShopEntities())
            {   
                //lấy thông tin sản phẩm truyền qua view Details
                var list = pro.SanPhams
                    .Where(p => p.ID == ID)
                    .FirstOrDefault();
                return PartialView("Details", list);
            }
            
        }
        //cập nhật lại sản phẩm
        [HttpPost]
        public ActionResult Details(SanPham sp)
        {   
            // biến flag dùng để đánh dấu sau đó xuất thông báo thành công hay không
            int flag = 0;
            //tiến hành validate nếu thất bại gán flag = 1
            if (string.IsNullOrEmpty(sp.Gia.ToString()))
            {
                ModelState.AddModelError("Gia", "Bạn cần nhập giá cả");
                flag = 1;
            }
            if (string.IsNullOrEmpty(sp.SoLuong.ToString()))
            {
                ModelState.AddModelError("SoLuong", "Bạn cần nhập số lượng");
                flag = 1;
            }
            else { 
                using (var pro = new MobileShopEntities())
                {
                    //lấy sản phẩm
                    List<HangSanXuat> list = null;
                    var spOld= pro.SanPhams
                        .Where(p => p.ID == sp.ID)
                        .FirstOrDefault();
                    //kiểm tra lấy thành công hay không
                    if(spOld != null)
                    {
                        //thay đổi thông tin
                        spOld.Gia = sp.Gia;
                        spOld.SoLuong = sp.SoLuong;
                        pro.Entry(spOld).State = EntityState.Modified;
                        pro.SaveChanges();
                    }
                    //lấy thông tin sản phẩm mới cập nhật
                    var l = pro.SanPhams
                        .Where(p => p.ID == sp.ID)
                        .FirstOrDefault();
                    //nếu không có lỗi gán ViewBag.comple = "Thay đổi thành công" và gọi ra bển view Details
                    if (flag == 0)
                    {
                        ViewBag.comple = "Thay đổi thành công";
                    }
                    return View("Details", l);
                }
            }
            return View();

        }
        //delete sản phẩm mobile
        public ActionResult DeleteMobile(int ID)
        {

            using (var pro = new MobileShopEntities())
            {
                //lấy sản phẩm cần xóa
                var list = pro.SanPhams
                    .Where(p => p.ID == ID)
                    .FirstOrDefault();
                //kiểm tra lấy thành công hay không
                if (list != null)
                {
                    //tiến hành remove sản phẩm đó
                    list.TinhTrang = 1;
                    pro.Entry(list).State = EntityState.Modified;
                    pro.SaveChanges();
                }
                //lấy danh sách sản phẩm và truyền qua view getAllmobile
                var l = pro.SanPhams.OrderByDescending(p => p.NgayCapNhat)
                    .Where(p => p.IDLoai == 1).Where(p => p.TinhTrang == 0).ToList();
                return PartialView("getAllMobile", l);
            }

        }
        //Tương tự deleteMobile
        public ActionResult DeleteTablet(int ID)
        {

            using (var pro = new MobileShopEntities())
            {
                var list = pro.SanPhams
                    .Where(p => p.ID == ID)
                    .FirstOrDefault();
                if (list != null)
                {
                    list.TinhTrang = 1;
                    pro.Entry(list).State = EntityState.Modified;
                    pro.SaveChanges();
                }
                var l = pro.SanPhams.OrderByDescending(p => p.NgayCapNhat)
                    .Where(p => p.IDLoai == 2).Where(p => p.TinhTrang == 0).ToList();
                return PartialView("getAllTablet", l);
            }

        }
    }
}