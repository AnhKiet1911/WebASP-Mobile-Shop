using LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTW.Libs;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace LTW.Controllers
{
    /**
     * class User controller 
     * Handle: function login, register, logout, ...
     **/
    public class UserController : Controller
    {
        // GET: User
        /**
         * Function login: handle user login and set session.
         * return [json] [send json data to client]
         * */
        [HttpPost]
        public JsonResult Login(UserInfo u)
        {
            var password = Libs.Libs.GetMD5(u.Password);
            using (var dc = new MobileShopEntities())
            {
                var user = dc.Users
                    .Where(ui => ui.Username == u.Username && ui.Password == password)
                    .FirstOrDefault();
                if(user != null)
                {
                    Session["Logged"] = user;
                
                    return Json(new { data="true"});
                }
                else
                {
                    return Json(new { data = "false"});
                }
            } 
        }

        /**
         * Function logout: handle user logout and unset session.
         * return [redirect] [redirect to home page]
         * */
        public ActionResult Logout()
        {
            Session["Logged"] = null;
            Session["Cart"] = null;

            return RedirectToAction("Index", "Home");
        }

        /**
         * Function register: handle register user.
         * return [json] [send json data to client]
         * */
        [HttpPost]
        public JsonResult Register(UserRegister user)
        {
            if (user.g_recaptcha_response == null)
            {
                return Json(new { captcha = "false", data = "false"});
            }
            else
            {
                var u = new User
                {
                    Username = user.Username,
                    Password = Libs.Libs.GetMD5(user.Password),
                    Email = user.Email,
                    FullName = user.FullName,
                    Birthday = user.Birthday,
                    Level = 0
                };

                using (var ui = new MobileShopEntities())
                {
                    var user_exist = ui.Users
                    .Where(x => x.Username == user.Username || x.Email == user.Email)
                    .FirstOrDefault();
                    if(user_exist != null)
                    {
                        return Json(new { captcha = "true", data = "exist" });
                    }
                    ui.Users.Add(u);
                    int a = ui.SaveChanges();
                    return Json(new { captcha = "true", data = a });
                }

            }


        }
        public ActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePass(ChangePass userchange)
        {
            string Password = string.Empty;
            string newPassword = string.Empty;
            int flag = 0;
            using (var ui = new MobileShopEntities())
            {
                if(string.IsNullOrEmpty(userchange.Username))
                {
                    ModelState.AddModelError("Username", "Không được để trống");
                }
                if (string.IsNullOrEmpty(userchange.Password))
                {
                    ModelState.AddModelError("Password", "Không được để trống");
                }
                else
                {
                   Password = Libs.Libs.GetMD5(userchange.Password);
                }
                if (string.IsNullOrEmpty(userchange.NewPassword))
                {
                    ModelState.AddModelError("NewPassword", "Không được để trống");
                }
                else
                {
                    newPassword = Libs.Libs.GetMD5(userchange.NewPassword);
                }
                if (string.IsNullOrEmpty(userchange.RePassword))
                {
                    ModelState.AddModelError("RePassword", "Không được để trống");
                }
                var list = ui.Users
                    .Where(p => p.Username == userchange.Username)
                    .FirstOrDefault();
                if(list != null)
                {
                    
                    if (list.Password != Password)
                    {
                        ModelState.AddModelError("Password", "Sai mật khẩu");
                    }
                    else
                    {
                        if (userchange.NewPassword != userchange.RePassword || (userchange.NewPassword == null || userchange.RePassword == null))
                        {
                            ModelState.AddModelError("NewPassword", "Password không trùng khớp");
                        }
                        else
                        {
                            list.Password = newPassword;
                            ui.Entry(list).State = EntityState.Modified;
                            ui.SaveChanges();
                            flag = 1;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("Username", "Tên tài khoản không đúng");
                }
                if(flag == 1)
                {
                    ViewBag.comple = "Thay đổi thành công";
                }
               
                return View();
            }

            return View();
        }

        public ActionResult InfoUser(string user)
        {
            using (var pro = new MobileShopEntities())
            {
                var list = pro.Users
                    .Where(p => p.Username == user)
                    .FirstOrDefault();
                return View(list);
            }
        }
        [HttpPost]
        public ActionResult InfoUser(User user)
        {
            int flag = 0;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(user.Email);
            if (match.Success)
            {
                flag = 0;
            }
            else
            {
                ModelState.AddModelError("Email", "Email chưa đúng");
                flag = 1;
            }
                
            if (string.IsNullOrEmpty(user.FullName))
            {
                ModelState.AddModelError("FullName", "Bạn cần nhập tên đầy đủ");
                flag = 1;
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError("Email", "Bạn cần nhập Email");
                flag = 1;
            }
            if (string.IsNullOrEmpty(user.Birthday.ToString()))
            {
                ModelState.AddModelError("Birthday", "Bạn cần nhập ngày sinh");
                flag = 1;
            }
            else
            {
                using (var pro = new MobileShopEntities())
                {
                    List<HangSanXuat> list = null;
                    var userOld = pro.Users
                        .Where(p => p.Username == user.Username)
                        .FirstOrDefault();

                    if (userOld != null)
                    {
                        userOld.FullName = user.FullName;
                        userOld.Email = user.Email;
                        userOld.Birthday = user.Birthday;
                        pro.Entry(userOld).State = EntityState.Modified;
                        pro.SaveChanges();
                    }
                    if (flag == 0)
                    {
                        ViewBag.comple = "Thay đổi thành công";
                    }
                    var l = pro.Users
                   .Where(p => p.Username == user.Username)
                   .FirstOrDefault();
                    return View(list);
                }
            }

            return View();
        }


    }
}