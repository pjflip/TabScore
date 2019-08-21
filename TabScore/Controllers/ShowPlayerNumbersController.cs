using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowPlayerNumbersController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            RoundClass round = Round.GetRoundInfo(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString());
            if (round == null) return RedirectToAction("Index", "ErrorScreen");
            ViewData["PairNS"] = round.PairNS.ToString();
            ViewData["PairEW"] = round.PairEW.ToString();

            ViewData["BackButton"] = "FALSE";
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"].ToString()}";

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
            return RedirectToAction("Index", "ShowRoundInfo");
        }
    }
}