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
            Session.Add("SectionLetter", "");
            Session.Add("SectionID", 0);
            Session.Add("NumTables", 0);
            Session.Add("Table", 0);
            Session.Add("Round", null);
            Session.Add("MissingPair", 0);
            Session.Add("MaxRounds", 0);
            Session.Add("Result", null); 
        }
    }
}
