using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace TabScore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Session.Add("DBConnectionString", "");
            Session.Add("Header","");
            Session.Add("IndividualEvent", false);
            Session.Add("Section", null);
            Session.Add("TableNumber", 0);
            Session.Add("Round", null);
            Session.Add("Result", null); 
        }
    }
}
