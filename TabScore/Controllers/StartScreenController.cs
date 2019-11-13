using System.Data.Odbc;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class StartScreenController : Controller
    {
        public ActionResult Index()
        {
            Session["Header"] = "";
            ViewData["BackButton"] = "FALSE";
            return View();
        }

        public ActionResult OKButtonClick()
        {
            // Check if DB connection has been set up
            string pathToTabScoreDB = System.Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
            string pathToDB = "";
            if (System.IO.File.Exists(pathToTabScoreDB))
            {
                pathToDB = System.IO.File.ReadAllText(pathToTabScoreDB);
            }
            if (pathToDB == "")
            {
                TempData["warningMessage"] = "Scoring database not yet started";
                return RedirectToAction("Index", "StartScreen");
            }
            else
            {
                // Check that we can open the DB
                OdbcConnectionStringBuilder cs = new OdbcConnectionStringBuilder();
                cs.Driver = "Microsoft Access Driver (*.mdb)";
                cs.Add("Dbq", pathToDB);
                cs.Add("Uid", "Admin");
                using (OdbcConnection connection = new OdbcConnection(cs.ToString()))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (OdbcException e)
                    {
                        TempData["warningMessage"] = e.Message;
                        return RedirectToAction("Index", "StartScreen");
                    }
                }
                
                // Set connection string for the session.  This is used to check if the session is interrupted
                Session["DBConnectionString"] = cs.ToString();

                // Check if session is for an individual event
                Session["IndividualEvent"] = DBInfo.IsIndividual(Session["DBConnectionString"].ToString());

                return RedirectToAction("Index", "EnterSection");
            }
        }
    }
}