using LTW.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW.Areas.Admin.Lib
{
    public static class Helper
    {
        //lấy danh sách category để truyền vào dropdown
        public static IList<SelectListItem> GetSLICat(this HtmlHelper html)
        {
            var l = new List<SelectListItem>();
            using (var pro = new MobileShopEntities())
            {
                foreach(var c in pro.HangSanXuats.ToList())
                {
                    l.Add(new SelectListItem {
                        Value = c.ID.ToString(),
                        Text = c.TenHang
                    });
                }
            }
            return l;
        }
        //tạo 2 biến img để lưu tên hình minh họa
        static string imgMainName = "lon.png";
        static string imgThumsName = "nho.png";
        public static void SaveProductImg(int pId,string path, HttpPostedFileBase imgLg, HttpPostedFileBase imgSm)
        {
            //tạo đường dẫn để lưu ảnh
            string pathProductImg = Path.Combine(path, "Public/images/products/sp", pId.ToString());
            Directory.CreateDirectory(pathProductImg);
            //tiến hành lưu ảnh
            imgLg.SaveAs(Path.Combine(pathProductImg, imgMainName));
            imgSm.SaveAs(Path.Combine(pathProductImg, imgThumsName));
        }
        //xóa hình ảnh khi xóa sản phầm
        public static void DeleteProductImg(int pId, string path)
        {
            //tạo đường dẫn tới ảnh cần xóa
            string pathProductImg = Path.Combine(path, "Public/images/products/sp", pId.ToString());
            //tiến hành xóa
            Directory.Delete(pathProductImg, true);
 
        }
    }
}