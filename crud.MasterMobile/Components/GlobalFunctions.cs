using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crud.MasterMobile.Components
{
    public class GlobalFunctions
    {
        public static string GetIPAddress()
        {
            return HttpContext.Current.Request.UserHostAddress.ToString();
        }
    }
}