using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowPlayerNumbersController : Controller
    {
        public ActionResult Index()
        {

            RoundClass round = Round.GetRoundInfo(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString());
            ViewData["PairNS"] = round.PairNS.ToString();
            ViewData["PairEW"] = round.PairEW.ToString();

            ViewData["CancelButton"] = "FALSE";
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"].ToString()}";

            if (round.PairNS == 0 || round.PairNS == Convert.ToInt32(Session["MissingPair"].ToString()))
            {
                ViewData["PlayerNameEast"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "E", false);
                ViewData["PlayerNameWest"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "W", false);
                return View("NSMissing");
            }
            else if (round.PairEW == 0 || round.PairEW == Convert.ToInt32(Session["MissingPair"].ToString()))
            {
                ViewData["PlayerNameNorth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "N", false);
                ViewData["PlayerNameSouth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "S", false);
                return View("EWMissing");
            }
            else
            {
                ViewData["PlayerNameNorth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "N", false);
                ViewData["PlayerNameSouth"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairNS.ToString(), "S", false);
                ViewData["PlayerNameEast"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "E", false);
                ViewData["PlayerNameWest"] = Player.GetName(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), round.PairEW.ToString(), "W", false);
                return View();
            }
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowRoundInfo");
        }
    }
}