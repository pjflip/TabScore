using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterContractController : Controller
    {
        public ActionResult Index(string board)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "ErrorScreen");
            }
            Session["Board"] = board;

            if (Session["ContractLevel"].ToString() == "")
            {
                /* No session data, so check database */
                ResultClass res = new ResultClass
                {
                    SectionID = Session["SectionID"].ToString(),
                    Table = Session["Table"].ToString(),
                    Round = Session["Round"].ToString(),
                    Board = board,
                    ContractLevel = "",
                    ContractSuit = "",
                    ContractX = "NONE",
                    NSEW = "",
                    TricksTakenNumber = -1,
                    LeadCard = ""
                };
                res.GetDBResult(DBConnectionString);
                Session["ContractLevel"] = res.ContractLevel;
                Session["ContractSuit"] = res.ContractSuit;
                Session["ContractX"] = res.ContractX;
                Session["NSEW"] = res.NSEW;
                Session["TricksTakenNumber"] = res.TricksTakenNumber.ToString();
                Session["LeadCard"] = res.LeadCard;

            }

            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - {Vulnerability.SetPairString("NS", Session["Board"].ToString(), Session["PairNS"].ToString())} v {Vulnerability.SetPairString("EW", Session["Board"].ToString(), Session["PairEW"].ToString())}";
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
            Session["TricksTakenNumber"] = "-1";
            return RedirectToAction("Index", "ConfirmResult");
        }

        public ActionResult BackButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }
    }
}
