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

            NamesClass pn = PairNames.GetNamesForPairNo(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), r.PairNS.ToString(), "NS");
            ViewData["PlayerNameNorth"] = pn.NameNE;
            ViewData["PlayerNameSouth"] = pn.NameSW;
            pn = PairNames.GetNamesForPairNo(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), r.PairEW.ToString(), "EW");
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
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult OKSitOutButtonClick()
        {
            return RedirectToAction("Index", "ShowMove");
        }

        public ActionResult ControlButtonClick()
        {
            Session["ControlReturnScreen"] = "ShowRoundInfo";
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}