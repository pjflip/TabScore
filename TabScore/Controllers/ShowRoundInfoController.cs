using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRoundInfoController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "StartScreen");
            }

            RoundClass round = Round.GetRoundInfo(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString());
            Session["LowBoard"] = round.LowBoard;
            Session["HighBoard"] = round.HighBoard;
            Session["PairNS"] = round.PairNS;
            Session["PairEW"] = round.PairEW;

            if (Session["Round"].ToString() == "1")
            {
                ViewData["BackButton"] = "FALSE";
            }
            else 
            {
                ViewData["BackButton"] = "TRUE";
            }
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]}";

            if (round.PairNS == 0 || round.PairNS.ToString() == Session["MissingPair"].ToString())
            {
                ViewData["PlayerNameEast"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "E", true);
                ViewData["PlayerNameWest"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "W", true);
                return View("NSMissing");
            }
            else if (round.PairEW == 0 || round.PairEW.ToString() == Session["MissingPair"].ToString())
            {
                ViewData["PlayerNameNorth"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "N", true);
                ViewData["PlayerNameSouth"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "S", true);
                return View("EWMissing");
            }
            else
            {
                ViewData["PlayerNameNorth"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "N", true);
                ViewData["PlayerNameSouth"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "S", true);
                ViewData["PlayerNameEast"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "E", true);
                ViewData["PlayerNameWest"] = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "W", true);
                return View();
            }
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult OKSitOutButtonClick()
        {
            return RedirectToAction("Index", "ShowRankingList", new { round = Session["Round"].ToString(), finalRound = "No" });
        }

        public ActionResult BackButtonClick()
        {
            return RedirectToAction("Index", "ShowMove", new { newRound = Session["Round"].ToString() });
        }
    }
}