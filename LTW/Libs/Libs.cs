using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LTW.Libs
{
    public class Libs
    {
        public static string GetMD5(string str)
        {
            using (var md5 = MD5.Create())
            {
                byte[] arr = Encoding.ASCII.GetBytes(str);
                byte[] arrMd5 = md5.ComputeHash(arr);
                StringBuilder sb = new StringBuilder();
                foreach (var b in arrMd5)
                {
                    sb.AppendFormat("{0:X}", b);
                }
                return sb.ToString();
            }
        }
    }
}