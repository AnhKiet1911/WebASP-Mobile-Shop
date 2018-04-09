using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LTW.Models
{
    public class reCapcha
    {
        //attribute có tên trùng với field của json trả về
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        //attribute có tên trùng với field của json trả về
        [DataMember(Name = "error-codes")]
        public string[] ErrorCodes { get; set; }

        
    }
}