using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BudgeterAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

           routes.MapRoute(
               name: "ApiByName",
               url: "api/{controller}/{action}/{name}",
               defaults: null,
               constraints: new { name = @"^[a-z]+$" }
            );

           routes.MapRoute(
            name: "ApiByAction",
            url: "api/{controller}/{action}",
            defaults: new { action = "Get" }
        );
        }
    }
}
