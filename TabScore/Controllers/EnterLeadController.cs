using TabScore.Models;
using System.Web.Mvc;
using System;

namespace TabScore.Controllers
{
    public class EnterLeadController : Controller
    {
        public ActionResult Index(string secondPass)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (!Settings.GetSetting<bool>(DBConnectionString, SettingName.EnterLeadCard))
            {
                Session["LeadCard"] = "SKIP";
                return RedirectToAction("Index", "EnterTricksTaken");
            }

            Result res = new Result()
            {
                ContractLevel = Session["ContractLevel"].ToString(),
                ContractSuit = Session["ContractSuit"].ToString(),
                ContractX = Session["ContractX"].ToString(),
                NSEW = Session["NSEW"].ToString()
            };
            ViewData["DisplayContract"] = res.DisplayContract();

            if (Session["LeadCard"].ToString() == "")
            {
                ViewData["Suit"] = "";
                ViewData["Card"] = "";
            }
            else
            {
                ViewData["Suit"] = Session["LeadCard"].ToString().Substring(0, 1);
                ViewData["Card"] = Session["LeadCard"].ToString().Substring(1, 1);
            }

            ViewData["BackButton"] = "TRUE";
            ViewData["SecondPass"] = secondPass;
            return View();
        }

        public ActionResult OKButtonClick(string card, string secondPass)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (!Settings.GetSetting<bool>(DBConnectionString, SettingName.ValidateLeadCard) || secondPass == "TRUE")
            {
                Session["LeadCard"] = card;
                return RedirectToAction("Index", "EnterTricksTaken");
            }
            else
            {
                if (Lead.Validate(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Board"]), card, Session["NSEW"].ToString()))
                {
                    Session["LeadCard"] = card;
                    return RedirectToAction("Index", "EnterTricksTaken");
                }
                else
                {
                    return RedirectToAction("Index", "EnterLead", new { secondPass = "TRUE" });
                }
            }
        }

        public ActionResult BackButtonClick()
        {
            return RedirectToAction("Index", "EnterContract", new { board = Session["Board"].ToString() } );
        }
    }
}