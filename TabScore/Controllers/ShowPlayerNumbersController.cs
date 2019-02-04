using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowPlayerNumbersController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "StartScreen");
            }

            RoundClass round = Round.GetRoundInfo(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString());
            ViewData["PairNS"] = round.PairNS.ToString();
            ViewData["PairEW"] = round.PairEW.ToString();

            ViewData["BackButton"] = "FALSE";
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"].ToString()}";

            if (round.PairNS == 0 || round.PairNS.ToString() == Session["MissingPair"].ToString())
            {
                ViewData["PlayerNameEast"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "E", false);
                ViewData["PlayerNameWest"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "W", false);
                return View("NSMissing");
            }
            else if (round.PairEW == 0 || round.PairEW.ToString() == Session["MissingPair"].ToString())
            {
                ViewData["PlayerNameNorth"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "N", false);
                ViewData["PlayerNameSouth"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "S", false);
                return View("EWMissing");
            }
            else
            {
                ViewData["PlayerNameNorth"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "N", false);
                ViewData["PlayerNameSouth"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "S", false);
                ViewData["PlayerNameEast"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "E", false);
                ViewData["PlayerNameWest"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "W", false);
                return View();
            }
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowRoundInfo");
        }
    }
}