using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTricksTakenController : Controller
    {
        public ActionResult Index()
        {
            ResultClass res = new ResultClass()
            {
                ContractLevel = Session["ContractLevel"].ToString(),
                ContractSuit = Session["ContractSuit"].ToString(),
                ContractX = Session["ContractX"].ToString(),
                NSEW = Session["NSEW"].ToString()
            };
            ViewData["DisplayContract"] = res.DisplayContract(1);

            ViewData["Board"] = Session["Board"];
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - {Vulnerability.SetPairString("NS", Session["Board"].ToString(), Session["PairNS"].ToString())} v {Vulnerability.SetPairString("EW", Session["Board"].ToString(), Session["PairEW"].ToString())}";
            ViewData["CancelButton"] = "TRUE";
            return View();
        }

        public ActionResult OKButtonClick(string numTricks)
        {
            Session["TricksTakenNumber"] = numTricks;
            return RedirectToAction("Index", "ConfirmResult");
        }

        public ActionResult CancelButtonClick()
        {
            return RedirectToAction("Index", "EnterLead", new { secondPass = "FALSE" });
        }
    }
}