using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterContractController : Controller
    {
        public ActionResult Index(int boardNumber)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Round round = Session["Round"] as Round;
            Result result = Session["Result"] as Result;
            Section section = Session["Section"] as Section;

            if (result == null || result.BoardNumber != boardNumber)   // No session result data for this board
            {
                result = new Result(DBConnectionString, section.ID, Convert.ToInt32(Session["TableNumber"]), round.RoundNumber, boardNumber)
                {
                    PairNS = round.PairNS,
                    PairEW = round.PairEW
                };
                if (Convert.ToBoolean(Session["IndividualEvent"]))
                {
                    result.South = round.South;
                    result.West = round.West;
                }
                Session["Result"] = result;
            }

            if (Convert.ToBoolean(Session["IndividualEvent"]))
            {
                Session["Header"] = $"Table {section.Letter}{Session["TableNumber"]} - Round {round.RoundNumber} - {UtilityFunctions.ColourPairByVulnerability("NS", boardNumber, $"{round.PairNS}+{round.South}")} v {UtilityFunctions.ColourPairByVulnerability("EW", boardNumber, $"{round.PairEW}+{round.West}")}";
            }
            else
            {
                Session["Header"] = $"Table {section.Letter}{Session["TableNumber"]} - Round {round.RoundNumber} - {UtilityFunctions.ColourPairByVulnerability("NS", boardNumber, $"NS {round.PairNS}")} v {UtilityFunctions.ColourPairByVulnerability("EW", boardNumber, $"EW {round.PairEW}")}";
            }
            ViewData["BackButton"] = "TRUE";
            return View(result);
        }

        public ActionResult OKButtonContract(string cLevel, string cSuit, string cX, string cNSEW)
        {
            Result result = Session["Result"] as Result;
            result.ContractLevel = Convert.ToInt32(cLevel);
            result.ContractSuit = cSuit;
            result.ContractX = cX;
            result.NSEW = cNSEW;
            Session["Result"] = result;
            return RedirectToAction("Index", "EnterLead", new { validateWarning = "Validate" });
        }

        public ActionResult OKButtonPass()
        {
            Result result = Session["Result"] as Result;
            result.ContractLevel = 0;
            result.ContractSuit = "";
            result.ContractX = "";
            result.NSEW = "";
            result.LeadCard = "";
            result.TricksTakenNumber = -1;
            Session["Result"] = result;
            return RedirectToAction("Index", "ConfirmResult");
        }
        public ActionResult OKButtonSkip()
        {
            Result result = Session["Result"] as Result;
            result.ContractLevel = -1;
            result.ContractSuit = "";
            result.ContractX = "";
            result.NSEW = "";
            result.LeadCard = "";
            result.TricksTakenNumber = -1;
            Session["Result"] = result;

            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");
            result.UpdateDB(DBConnectionString, Convert.ToBoolean(Session["IndividualEvent"]));
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult BackButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }
    }
}
