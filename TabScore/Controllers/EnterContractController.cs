using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterContractController : Controller
    {
        public ActionResult Index(string board)
        {
            Session["Board"] = board;
            ViewData["Board"] = board;
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - {Vulnerability.SetPairString("NS", Session["Board"].ToString(), Session["PairNS"].ToString())} v {Vulnerability.SetPairString("EW", Session["Board"].ToString(), Session["PairEW"].ToString())}";
            ViewData["CancelButton"] = "TRUE";
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

        public ActionResult CancelButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }
    }
}
