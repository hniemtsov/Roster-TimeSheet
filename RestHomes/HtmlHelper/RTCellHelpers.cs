using RestHomes.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestHomes.HtmlHelpers
{
    public static class RosterHelpers
    {
        public static MvcHtmlString GetUserName(this HtmlHelper html, string id)
        {
           //AppUserManager mgr = HttpContext.Current..GetOwinContext()..GetUserManager<AppUserManager>();
           return new MvcHtmlString("ddsfs");
        }
    }

    public static class RTCellHelpers
    {
        public static MvcHtmlString RosterCells()
        {
            
            return MvcHtmlString.Create("sdfsf");
        }
    }
}