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
            Session.Add("IndividualEvent", "");
            Session.Add("SectionLetter", "");
            Session.Add("SectionID", 0);
            Session.Add("NumTables", 0);
            Session.Add("Table", 0);
            Session.Add("Round", 0);
            Session.Add("PairNS", 0);   // Doubles as North for individuals
            Session.Add("PairEW", 0);   // Doubles as East for individuals
            Session.Add("South", 0);
            Session.Add("West", 0);
            Session.Add("MissingPair", 0);
            Session.Add("LowBoard", 0);
            Session.Add("HighBoard", 0);
            Session.Add("Board", 0);
            Session.Add("ContractLevel", 0);
            Session.Add("ContractSuit", "");
            Session.Add("ContractX", "NONE");
            Session.Add("NSEW", "");
            Session.Add("TricksTakenNumber", 0);
            Session.Add("LeadCard", "");
            Session.Add("Score", 0);
        }
    }
}
