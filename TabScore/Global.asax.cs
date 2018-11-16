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
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Session.Add("DBConnectionString", "");
            Session.Add("SectionLetter", "");
            Session.Add("SectionID", "0");
            Session.Add("NumTables", "0");
            Session.Add("Table", "0");
            Session.Add("Round", "0");
            Session.Add("PairNS", "0");
            Session.Add("PairEW", "0");
            Session.Add("MissingPair", "0");
            Session.Add("Winners", "0");
            Session.Add("LowBoard", "0");
            Session.Add("HighBoard", "0");
            Session.Add("Board", "0");
            Session.Add("ContractLevel", "");
            Session.Add("ContractSuit", "");
            Session.Add("ContractX", "NONE");
            Session.Add("NSEW", "");
            Session.Add("TricksTakenNumber", "0");
            Session.Add("LeadCard", "");
            Session.Add("Score", "0");
            Session.Add("ControlReturnScreen", "StartScreen");
        }
    }
}
