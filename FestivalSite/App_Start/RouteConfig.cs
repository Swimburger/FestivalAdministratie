﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FestivalSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "BandByName",
                url: "Bands/{name}",
                defaults: new { controller = "Band", action = "Details"}
            );

            routes.MapRoute(
                name: "Bands",
                url: "Bands",
                defaults: new { controller = "Band", action = "Index" }
            );

            routes.MapRoute(
                name: "StageByName",
                url: "Stages/{name}",
                defaults: new { controller = "Stage", action = "Details" }
            );

            routes.MapRoute(
                name: "Stages",
                url: "Stages",
                defaults: new { controller = "Band", action = "Index" }
            );


            routes.MapRoute(
                name: "DagenByDag",
                url: "Dagen/{dag}",
                defaults: new { controller = "Dag", action = "Details" }
            );

            routes.MapRoute(
                name: "Dagen",
                url: "Dagen",
                defaults: new { controller = "Dag", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            MvcSiteMapProvider.Web.Mvc.XmlSiteMapController.RegisterRoutes(routes);
        }
    }
}