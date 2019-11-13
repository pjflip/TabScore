using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterContractController : Controller
    {
        public ActionResult Index(string board)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Session["Board"] = board;

            if (Session["ContractLevel"].ToString() == "")
            {
                /* No session data, so check database */
                Result res = new Result
                {
                    SectionID = Convert.ToInt32(Session["SectionID"]),
                    Table = Convert.ToInt32(Session["Table"]),
                    Round = Convert.ToInt32(Session["CurrentRound"]),
                    Board = Convert.ToInt32(board),
                    ContractLevel = "",
                    ContractSuit = "",
                    ContractX = "NONE",
                    NSEW = "",
                    TricksTakenNumber = -1,
                    LeadCard = ""
                };
                if(!res.ReadFromDB(DBConnectionString)) return RedirectToAction("Index", "ErrorScreen"); 
                Session["ContractLevel"] = res.ContractLevel;
                Session["ContractSuit"] = res.ContractSuit;
                Session["ContractX"] = res.ContractX;
                Session["NSEW"] = res.NSEW;
                Session["TricksTakenNumber"] = res.TricksTakenNumber;
                Session["LeadCard"] = res.LeadCard;

            }

            if (Session["IndividualEvent"].ToString() == "YES")
            {
                Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["CurrentRound"]} - {Vulnerability.SetPairString("NS", Convert.ToInt32(Session["Board"]), $"{Session["PairNS"]}+{Session["South"]}")} v {Vulnerability.SetPairString("EW", Convert.ToInt32(Session["Board"]), $"{Session["PairEW"]}+{Session["West"]}")}";
            }
            else
            {
                Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["CurrentRound"]} - {Vulnerability.SetPairString("NS", Convert.ToInt32(Session["Board"]), $"NS {Session["PairNS"]}")} v {Vulnerability.SetPairString("EW", Convert.ToInt32(Session["Board"]), $"EW {Session["PairEW"]}")}";
            }
            ViewData["BackButton"] = "TRUE";
            return View();
        }

        public ActionResult OKButtonContract(string cLevel, string cSuit, string cX, string cNSEW)
        {
            Session["ContractLevel"] = cLevel;
            Session["ContractSuit"] = cSuit;
            Session["ContractX"] = cX;
            Session["NSEW"] = cNSEW;
            return RedirectToAction("Index", "EnterLead", new { secondPass = "FALSE" });
        }

        public ActionResult OKButtonPass()
        {
            Session["ContractLevel"] = "PASS";
            Session["ContractSuit"] = "";
            Session["ContractX"] = "";
            Session["NSEW"] = "";
            Session["LeadCard"] = "";
            Session["TricksTakenNumber"] = -1;
            return RedirectToAction("Index", "ConfirmResult");
        }

        public ActionResult BackButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }
    }
}
