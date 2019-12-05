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
            if (result == null || result.BoardNumber != boardNumber)   // No session result data for this board, so create new result object and check database
            {
                result = new Result
                {
                    SectionID = Convert.ToInt32(Session["SectionID"]),
                    Table = Convert.ToInt32(Session["Table"]),
                    RoundNumber = round.RoundNumber,
                    BoardNumber = boardNumber,
                    PairNS = round.PairNS,
                    PairEW = round.PairEW,
                    ContractLevel = -1,
                    ContractSuit = "",
                    ContractX = "NONE",
                    NSEW = "",
                    TricksTakenNumber = -1,
                    LeadCard = ""
                };
                if (Convert.ToBoolean(Session["IndividualEvent"]))
                {
                    result.South = round.South;
                    result.West = round.West;
                }
                result.ReadFromDB(DBConnectionString);
                Session["Result"] = result;
            }

            if (Convert.ToBoolean(Session["IndividualEvent"]))
            {
                Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {round.RoundNumber} - {UtilityFunctions.ColourPairByVulnerability("NS", boardNumber, $"{round.PairNS}+{round.South}")} v {UtilityFunctions.ColourPairByVulnerability("EW", boardNumber, $"{round.PairEW}+{round.West}")}";
            }
            else
            {
                Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {round.RoundNumber} - {UtilityFunctions.ColourPairByVulnerability("NS", boardNumber, $"NS {round.PairNS}")} v {UtilityFunctions.ColourPairByVulnerability("EW", boardNumber, $"EW {round.PairEW}")}";
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

        public ActionResult BackButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }
    }
}
