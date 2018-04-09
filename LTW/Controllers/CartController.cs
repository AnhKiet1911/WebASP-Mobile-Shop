using LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult detailCart()
        {
            var cart = Session["Cart"] as Cart;
            return View(cart);
        }

        [HttpPost]
        public ActionResult addItems(int proId, int sltQuantity)
        {
            if(Session["Cart"] == null)
            {
                Session["Cart"] = new Cart();
            }
            var c = Session["Cart"] as Cart;
            c.addItems(proId, sltQuantity);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult Remove(int proId)
        {
            var c = Session["Cart"] as Cart;
            c.RemoveItem(proId);
            return RedirectToAction("detailCart", "Cart");
        }

        [HttpPost]
        public ActionResult Update(int proId, int quantity)
        {
            var c = Session["Cart"] as Cart;
            SanPham pro = null;
            using (var dc = new MobileShopEntities())
            {
                pro = dc.SanPhams.Where(p => p.ID == proId).FirstOrDefault();

                if (pro.SoLuong < quantity)
                {
                    c.Update(proId, pro.SoLuong);
                    return RedirectToAction("detailCart", "Cart");
                }
            }
            c.Update(proId, quantity);
            return RedirectToAction("detailCart", "Cart");
        }

        public ActionResult CheckOut()
        {
            var c = Session["Cart"] as Cart;
            var ui = Session["Logged"] as User;

            using (var dc = new MobileShopEntities())
            {
                var user = dc.Users.Where(u => u.ID == ui.ID).FirstOrDefault();
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    User = user,
                };
                dc.Orders.Add(order);
                decimal? amount = 0;
                decimal? total = 0;
                foreach (var ci in c.Item)
                {
                    var p = dc.SanPhams.Where(i => i.ID == ci.Product.ID).FirstOrDefault();
                    p.LuotMua += ci.Quantity;
                    p.SoLuong -= ci.Quantity;
                    dc.SaveChanges();

                    amount = p.Gia * ci.Quantity;
                    total += amount;
                    var od = new OrderDetail
                    {
                        Order = order,
                        SanPham = p,
                        Quantity = ci.Quantity,
                        Price = p.Gia,
                        Amount = amount,
                    };
                    dc.OrderDetails.Add(od);
                }
                order.Total = total;
                dc.SaveChanges();
            }
            c.CheckOut();
            return RedirectToAction("detailCart", "Cart");
        }
        
    }
}