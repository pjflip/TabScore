using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRoundInfoController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Round"] = Session["Round"];

            RoundClass round = Round.GetRoundInfo(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString());
            Session["LowBoard"] = round.LowBoard;
            Session["HighBoard"] = round.HighBoard;
            Session["PairNS"] = round.PairNS;
            Session["PairEW"] = round.PairEW;
            ViewData["LowBoard"] = round.LowBoard;
            ViewData["HighBoard"] = round.HighBoard;
            ViewData["PairNS"] = round.PairNS;
            ViewData["PairEW"] = round.PairEW;

            ViewData["BackButton"] = "FALSE";
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]}";

            if (round.PairNS == 0 || round.PairNS.ToString() == Session["MissingPair"].ToString())
            {
                ViewData["PlayerNameEast"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "E", true);
                ViewData["PlayerNameWest"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "W", true);
                return View("NSMissing");
            }
            else if (round.PairEW == 0 || round.PairEW.ToString() == Session["MissingPair"].ToString())
            {
                ViewData["PlayerNameNorth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "N", true);
                ViewData["PlayerNameSouth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "S", true);
                return View("EWMissing");
            }
            else
            {
                ViewData["PlayerNameNorth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "N", true);
                ViewData["PlayerNameSouth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "S", true);
                ViewData["PlayerNameEast"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "E", true);
                ViewData["PlayerNameWest"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "W", true);
                return View();
            }
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult OKSitOutButtonClick()
        {
            return RedirectToAction("Index", "ShowRankingList", new { finalRound = "No" });
        }
    }
}