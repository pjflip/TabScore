using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTricksTakenController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "StartScreen");
            }

            ResultClass res = new ResultClass()
            {
                ContractLevel = Session["ContractLevel"].ToString(),
                ContractSuit = Session["ContractSuit"].ToString(),
                ContractX = Session["ContractX"].ToString(),
                NSEW = Session["NSEW"].ToString()
            };
            ViewData["DisplayContract"] = res.DisplayContract(2);

            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - {Vulnerability.SetPairString("NS", Session["Board"].ToString(), Session["PairNS"].ToString())} v {Vulnerability.SetPairString("EW", Session["Board"].ToString(), Session["PairEW"].ToString())}";
            ViewData["BackButton"] = "TRUE";

            if (Settings.EnterResultsMethod(DBConnectionString) == 1)
            {
                return View("TotalTricks");
            }
            else
            {
                ViewData["ContractLevel"] = Session["ContractLevel"];
                return View("TricksPlusMinus");
            }
        }

        public ActionResult OKButtonClick(string numTricks)
        {
            Session["TricksTakenNumber"] = numTricks;
            return RedirectToAction("Index", "ConfirmResult");
        }

        public ActionResult BackButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "StartScreen");
            }

            if (Settings.EnterLeadCard(DBConnectionString))
            {
                return RedirectToAction("Index", "EnterLead", new { secondPass = "FALSE" });
            }
            else
            {
                return RedirectToAction("Index", "EnterContract", new { board = Session["Board"].ToString() } );
            }
        }
    }
}