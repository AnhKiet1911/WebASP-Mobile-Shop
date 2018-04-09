using LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW.Libs
{
    public static class AddHelpers
    {

        public static Cart getCartItems(this HtmlHelper html)
        {
            if(HttpContext.Current.Session["Cart"] == null)
            {
                HttpContext.Current.Session["Cart"] = new Cart();
            }
            
            return (Cart)HttpContext.Current.Session["Cart"];
        }
        public static bool isLogged(this HtmlHelper html)
        {
            return HttpContext.Current.Session["Logged"] != null;
        }
        public static string getUsername(this HtmlHelper html)
        {
            var user = HttpContext.Current.Session["Logged"] as User;
            if(user != null)
            {
                return user.FullName;
            }
            return "";
        }
        public static string getUser(this HtmlHelper html)
        {
            var user = HttpContext.Current.Session["Logged"] as User;
            if (user != null)
            {
                return user.Username;
            }
            return "";
        }
        public static string getPer(this HtmlHelper html)
        {
                var user = HttpContext.Current.Session["Logged"] as User;
                if (user != null)
                {
                    return user.Level.ToString();
                }
                return "";
        }
    }
}