using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Nustache.Core;
using Nustache.Mvc;

namespace Nustache.Mvc3.Example
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterViewEngines(ViewEngines.Engines);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        public static void RegisterViewEngines(ViewEngineCollection engines)
        {
            Helpers.Register("link", LinkHelper);
            engines.RemoveAt(0);
            engines.Add(new NustacheViewEngine(fileExtensions: new[] {"mustache", "handlebars"}, additionalLocations: new[] {"~/_templates/*"})
            {
                // Comment out this line to require Model in front of all your expressions.
                // This makes it easier to share templates between the client and server.
                // But it also means that ViewData/ViewBag is inaccessible.
                RootContext = NustacheViewEngineRootContext.Model
            });
        }

        private static void LinkHelper(RenderContext context, IList<object> arguments, IDictionary<string, object> options, RenderBlock fn, RenderBlock inverse)
        {
            context.Write(string.Format("<a href=\"{1}\">{0}</a>", arguments[0], arguments[1]));
        }
    }
}