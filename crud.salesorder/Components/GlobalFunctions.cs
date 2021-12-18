using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crud.salesorder.Components
{
    public class GlobalFunctions
    {
        public static string GetIPAddress()
        {
            return HttpContext.Current.Request.UserHostAddress.ToString();
        }
    }
}