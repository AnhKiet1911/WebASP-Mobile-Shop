using LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult getTop8NewProducts()
        {
            using (var pro = new MobileShopEntities())
            {
                var list = pro.SanPhams
                    .Where(p=>p.TinhTrang != 1)
                    .OrderByDescending(p => p.NgayCapNhat)
                    .Take(8)
                    .ToList();
                return PartialView("_getTop8NewProducts", list);
            }
            
        }
        public ActionResult getTop8ProductsManyViews()
        {
            using (var pro = new MobileShopEntities())
            {
                var list = pro.SanPhams
                    .Where(p => p.TinhTrang != 1)
                    .OrderByDescending(p => p.LuotXem)
                    .Take(8)
                    .ToList();
                return PartialView("_getTop8ProductsManyViews", list);
            }
        }
        public ActionResult getTop8ProductsSellLot()
        {
            using (var pro = new MobileShopEntities())
            {
                var list = pro.SanPhams
                    .Where(p => p.TinhTrang != 1)
                    .OrderByDescending(p => p.LuotMua)
                    .Take(8)
                    .ToList();
                return PartialView("_getTop8ProductsSellLot", list);
            }
        }
        public ActionResult getListProductsById(int subId, int? catId, int page=1)
        {
            int PerPage = 12;
            if (!catId.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            List<SanPham> list = null;
            using (var pro = new MobileShopEntities())
            {
                int TotalPro = pro.SanPhams.Where(p => p.IDHang == catId && p.IDLoai == subId && p.TinhTrang != 1).Count();

                int TotalPage = TotalPro / PerPage + (TotalPro % PerPage > 0 ? 1 : 0);
                if(page < 1)
                {
                    page = 1;
                }
                if(page > TotalPage)
                {
                    page = TotalPage;
                }
                ViewBag.totalPage = TotalPage;
                ViewBag.currentPage = page;
                ViewBag.subId = subId;
                ViewBag.catId = catId;
                if (TotalPro > 0)
                {
                    list = pro.SanPhams
                    .Where(p => p.IDHang == catId && p.IDLoai == subId && p.TinhTrang != 1)
                    .OrderBy(p => p.NgayCapNhat)
                    .Skip((page - 1) * PerPage)
                    .Take(PerPage)
                    .ToList();
                }
            }
            return View(list);
        }

        public ActionResult getAllProductsMobile(int page = 1)
        {
            int PerPage = 12;
            
            List<SanPham> list = null;
            using (var pro = new MobileShopEntities())
            {
                int TotalPro = pro.SanPhams.Where(p => p.IDLoai == 1 && p.TinhTrang != 1).Count();
                int TotalPage = TotalPro / PerPage + (TotalPro % PerPage > 0 ? 1 : 0);
                if (page < 1)
                {
                    page = 1;
                }
                if (page > TotalPage)
                {
                    page = TotalPage;
                }
                ViewBag.totalPage = TotalPage;
                ViewBag.currentPage = page;
                list = pro.SanPhams
                    .Where(p => p.IDLoai == 1)
                    .OrderBy(p => p.NgayCapNhat)
                    .Skip((page - 1) * PerPage)
                    .Take(PerPage)
                    .ToList();
            }
            return View(list);
        }
        public ActionResult getAllProductsTablet(int page = 1)
        {
            int PerPage = 12;

            List<SanPham> list = null;
            using (var pro = new MobileShopEntities())
            {
                int TotalPro = pro.SanPhams.Where(p => p.IDLoai == 2 && p.TinhTrang != 1).Count();
                int TotalPage = TotalPro / PerPage + (TotalPro % PerPage > 0 ? 1 : 0);
                if (page < 1)
                {
                    page = 1;
                }
                if (page > TotalPage)
                {
                    page = TotalPage;
                }
                ViewBag.totalPage = TotalPage;
                ViewBag.currentPage = page;
                list = pro.SanPhams
                    .Where(p => p.IDLoai == 2 && p.TinhTrang != 1)
                    .OrderBy(p => p.NgayCapNhat)
                    .Skip((page - 1) * PerPage)
                    .Take(PerPage)
                    .ToList();
            }
            return View(list);
        }
        public ActionResult getAllProducts(int page = 1)
        {
            int PerPage = 12;

            List<SanPham> list = null;
            using (var pro = new MobileShopEntities())
            {
                int TotalPro = pro.SanPhams.Where(p => p.TinhTrang != 1).Count();
                int TotalPage = TotalPro / PerPage + (TotalPro % PerPage > 0 ? 1 : 0);
                if (page < 1)
                {
                    page = 1;
                }
                if (page > TotalPage)
                {
                    page = TotalPage;
                }
                ViewBag.totalPage = TotalPage;
                ViewBag.currentPage = page;
                list = pro.SanPhams
                    .Where(p => p.TinhTrang != 1)
                    .OrderBy(p => p.NgayCapNhat)
                    .Skip((page - 1) * PerPage)
                    .Take(PerPage)
                    .ToList();
            }
            return View(list);
        }

        public ActionResult DetailsProduct(int proId)
        {
            SanPham pro = null;
            using (var p = new MobileShopEntities())
            {
                pro = p.SanPhams
                    .Where(c=>c.ID == proId)
                    .FirstOrDefault();
                pro.LuotXem += 1;
                p.SaveChanges();
            }
            return View(pro);
        }

        public ActionResult get5ProductsSameKind(int proId)
        {

            using (var dc = new MobileShopEntities())
            {
                var sanPham = dc.SanPhams.Where(p => p.ID == proId).FirstOrDefault();
                List<SanPham> listProducts = null;
                listProducts = dc.SanPhams.Where(p => p.IDLoai == sanPham.IDLoai && p.TinhTrang != 1).Take(4).ToList();

                return PartialView("_get5ProductsSameKind", listProducts);
            } 
        }

        public ActionResult get5ProductsSameBrand(int proId)
        {

            using (var dc = new MobileShopEntities())
            {
                var sanPham = dc.SanPhams.Where(p => p.ID == proId).FirstOrDefault();
                List<SanPham> listProducts = null;
                listProducts = dc.SanPhams.Where(p => p.IDHang == sanPham.IDHang && p.TinhTrang != 1).Take(4).ToList();

                return PartialView("_get5ProductsSameBrand", listProducts);
            }
        }

        public ActionResult findProducts(string query, int page = 1)
        {
            int PerPage = 12;
            List<SanPham> list = null;

            using (var pro = new MobileShopEntities())
            {
                int TotalPro = pro.SanPhams.Where(s=>s.TenSanPham.Contains(query) && s.TinhTrang != 1).Count();
                int TotalPage = TotalPro / PerPage + (TotalPro % PerPage > 0 ? 1 : 0);

                if (page < 1) {
                    page = 1;
                }

                if (page > TotalPage) {
                    page = TotalPage;
                }

                ViewBag.totalPage = TotalPage;
                ViewBag.currentPage = page;
                list = pro.SanPhams
                    .Where(s => s.TenSanPham.Contains(query) && s.TinhTrang != 1)
                    .OrderBy(p => p.NgayCapNhat)
                    .Skip((page - 1) * PerPage)
                    .Take(PerPage)
                    .ToList();
            }

            return View(list);
        }
    }
}