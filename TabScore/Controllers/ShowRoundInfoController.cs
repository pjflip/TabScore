using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRoundInfoController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            RoundClass round = Round.GetRoundInfo(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString());
            if (round == null) return RedirectToAction("Index", "ErrorScreen");
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

            string tempName;
            if (round.PairNS == 0 || round.PairNS.ToString() == Session["MissingPair"].ToString())
            {
                tempName = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "E", false);
                if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                ViewData["PlayerNameEast"] = tempName;
                tempName = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "W", false);
                if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                ViewData["PlayerNameWest"] = tempName;
                return View("NSMissing");
            }
            else if (round.PairEW == 0 || round.PairEW.ToString() == Session["MissingPair"].ToString())
            {
                tempName = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "N", false);
                if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                ViewData["PlayerNameNorth"] = tempName;
                tempName = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "S", false);
                if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                ViewData["PlayerNameSouth"] = tempName;
                return View("EWMissing");
            }
            else
            {
                tempName = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "N", false);
                if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                ViewData["PlayerNameNorth"] = tempName;
                tempName = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "S", false);
                if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                ViewData["PlayerNameSouth"] = tempName;
                tempName = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "E", false);
                if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                ViewData["PlayerNameEast"] = tempName;
                tempName = Player.GetName(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "W", false);
                if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                ViewData["PlayerNameWest"] = tempName;
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
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            // Need to reset pair and board numbers
            string previousRound = (Convert.ToInt32(Session["Round"]) - 1).ToString();      // Round > 1 else no Back button 
            RoundClass round = Round.GetRoundInfo(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), previousRound);
            if (round == null) return RedirectToAction("Index", "ErrorScreen");
            Session["LowBoard"] = round.LowBoard;
            Session["HighBoard"] = round.HighBoard;
            Session["PairNS"] = round.PairNS;
            Session["PairEW"] = round.PairEW;

            return RedirectToAction("Index", "ShowMove", new { newRound = Session["Round"].ToString() });
        }
    }
}