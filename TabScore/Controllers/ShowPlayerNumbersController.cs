using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowPlayerNumbersController : Controller
    {
        public ActionResult Index()
        {
            // Check if results data exist - if it does go to last round entered
            int round = Round.GetLastEnteredRound(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString());
            if (round > 0)
            {
                Session["Round"] = round.ToString();
                return RedirectToAction("Index", "ShowRoundInfo");
            }
            Session["Round"] = "1";

            RoundClass r = Round.GetRoundInfo(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), "1");
            Session["LowBoard"] = r.LowBoard;
            Session["HighBoard"] = r.HighBoard;
            ViewData["PairNS"] = r.PairNS.ToString();
            ViewData["PairEW"] = r.PairEW.ToString();

            // Get names if there are any
            NamesClass pn = PairNames.GetNamesForStartTableNumber(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), "NS");
            ViewData["PlayerNameNorth"] = pn.NameNE;
            ViewData["PlayerNameSouth"] = pn.NameSW;
            pn = PairNames.GetNamesForStartTableNumber(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), "EW");
            ViewData["PlayerNameEast"] = pn.NameNE;
            ViewData["PlayerNameWest"] = pn.NameSW;

            ViewData["CancelButton"] = "FALSE";
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]}";
            if (r.PairNS == 0 || r.PairNS == Convert.ToInt32(Session["MissingPair"].ToString()))
            {
                return View("NSMissing");
            }
            if (r.PairEW == 0 || r.PairEW == Convert.ToInt32(Session["MissingPair"].ToString()))
            {
                return View("EWMissing");
            }
            return View();
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowRoundInfo");
        }
    }
}