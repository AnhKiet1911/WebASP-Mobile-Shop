using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LTW.Models
{
    public class UserRegister
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public string g_recaptcha_response { get; set; }
    }
}