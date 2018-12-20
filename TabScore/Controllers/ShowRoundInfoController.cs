using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRoundInfoController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Round"] = Session["Round"];

            RoundClass r = Round.GetRoundInfo(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString());
            Session["LowBoard"] = r.LowBoard;
            Session["HighBoard"] = r.HighBoard;
            Session["PairNS"] = r.PairNS;
            Session["PairEW"] = r.PairEW;
            ViewData["LowBoard"] = r.LowBoard;
            ViewData["HighBoard"] = r.HighBoard;
            ViewData["PairNS"] = r.PairNS;
            ViewData["PairEW"] = r.PairEW;

            ViewData["CancelButton"] = "FALSE";
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]}";

            if (r.PairNS == 0 || r.PairNS == Convert.ToInt32(Session["MissingPair"].ToString()))
            {
                ViewData["PlayerNameEast"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), r.PairEW.ToString(), "E", true);
                ViewData["PlayerNameWest"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), r.PairEW.ToString(), "W", true);
                return View("NSMissing");
            }
            else if (r.PairEW == 0 || r.PairEW == Convert.ToInt32(Session["MissingPair"].ToString()))
            {
                ViewData["PlayerNameNorth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), r.PairNS.ToString(), "N", true);
                ViewData["PlayerNameSouth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), r.PairNS.ToString(), "S", true);
                return View("EWMissing");
            }
            else
            {
                ViewData["PlayerNameNorth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), r.PairNS.ToString(), "N", true);
                ViewData["PlayerNameSouth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), r.PairNS.ToString(), "S", true);
                ViewData["PlayerNameEast"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), r.PairEW.ToString(), "E", true);
                ViewData["PlayerNameWest"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), r.PairEW.ToString(), "W", true);
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