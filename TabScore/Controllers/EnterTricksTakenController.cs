using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTricksTakenController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Result res = new Result()
            {
                ContractLevel = Session["ContractLevel"].ToString(),
                ContractSuit = Session["ContractSuit"].ToString(),
                ContractX = Session["ContractX"].ToString(),
                NSEW = Session["NSEW"].ToString()
            };
            ViewData["DisplayContract"] = res.DisplayContract();

            ViewData["BackButton"] = "TRUE";
            if (Settings.GetSetting<int>(DBConnectionString, SettingName.EnterResultsMethod) == 1)
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
            Session["TricksTakenNumber"] = Convert.ToInt32(numTricks);
            return RedirectToAction("Index", "ConfirmResult");
        }

        public ActionResult BackButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (Settings.GetSetting<bool>(DBConnectionString, SettingName.EnterLeadCard))
            {
                return RedirectToAction("Index", "EnterLead", new { secondPass = "FALSE" });
            }
            else
            {
                return RedirectToAction("Index", "EnterContract", new { board = Convert.ToInt32(Session["Board"]) } );
            }
        }
    }
}