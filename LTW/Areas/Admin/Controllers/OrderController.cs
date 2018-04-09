using LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace LTW.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            return View();
        }
        //lấy danh sách đơn hàng
        public ActionResult getAllOrder()
        {
            using (var pro = new MobileShopEntities())
            {
                //tiến hành kết bảng order và user
                //lấy danh sách order
                List<Order> OrderList = pro.Orders.ToList();

                //tạo một bảng orderUser gồm các thông tin cần thiết của order và user
                OrderUser OrderUser = new OrderUser();
                List<OrderUser> OrderUserList = OrderList.Select(x=> new OrderUser { OrderDate = x.OrderDate, Total = x.Total, Fullname=x.User.FullName, Username = x.User.Username, OrderId = x.OrderID, TrangThai= x.trangthai }).ToList();
                return View(OrderUserList);
            }
        }
        public ActionResult OrderDetail(int ID)
        {
            using (var pro = new MobileShopEntities())
            {
                //lấy danh sách Orderdetail
                List<OrderDetail> OrderDL = pro.OrderDetails.Where(p=>p.OrderID == ID).ToList();

                //tạo bảng orderdetailpro gồn các thông tin cần thiết của orderdetail và product
                OrderDetailPro ODP = new OrderDetailPro();

                List<OrderDetailPro> ODPList = OrderDL.Select(x => new OrderDetailPro { Amount = x.Amount, OrderId = x.OrderID, Price = x.Price, Soluong = x.Quantity, Tensanpham = x.SanPham.TenSanPham, ProId =x.ProID }).ToList();

                
                return View(ODPList);
            }
               
        }
        //Thay đổi trạng thái của đơn hàng
        public ActionResult Change(int ID)
        {
            using (var pro = new MobileShopEntities())
            {
                //cột order.trangthai mặc định bằng 0
                //lấy đơn hàng muốn thay đổi và thay đổi thuộc tính trangthai bằng 1
                List<Order> list = null;
                var order = pro.Orders
                    .Where(p => p.OrderID == ID)
                    .FirstOrDefault();
                order.trangthai = 1;
                pro.Entry(order).State = EntityState.Modified;
                pro.SaveChanges();

                //lấy dánh sách orderUser và load ra view getAllOrder
                List<Order> OrderList = pro.Orders.ToList();

                OrderUser OrderUser = new OrderUser();

                List<OrderUser> OrderUserList = OrderList.Select(x => new OrderUser { OrderDate = x.OrderDate, Total = x.Total, Fullname = x.User.FullName, Username = x.User.Username, OrderId = x.OrderID, TrangThai = x.trangthai }).ToList();
                return PartialView("getAllOrder", OrderUserList);
            }
            
        }
    }
}