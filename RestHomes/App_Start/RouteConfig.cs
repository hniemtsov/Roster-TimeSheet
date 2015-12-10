using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RestHomes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("",
                            url: "{controller}",
                            defaults: new { action = "List", id = UrlParameter.Optional }
                            );

            routes.MapRoute(name: "Default", 
                            url: "{controller}/{action}/{id}",
                            defaults: new { controller = "Roster", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}