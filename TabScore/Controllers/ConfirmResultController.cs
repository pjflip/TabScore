using TabScore.Models;
using System.Web.Mvc;
using System;

namespace TabScore.Controllers
{
    public class ConfirmResultController : Controller
    {
        public ActionResult Index()
        {
            if (Session["ContractLevel"].ToString() == "PASS")
            {
                ViewData["DisplayContract"] = "PASSed Out";
            }
            else
            {
                ResultClass res = new ResultClass
                {
                    Board = Session["Board"].ToString(),
                    PairNS = Session["PairNS"].ToString(),
                    PairEW = Session["PairEW"].ToString(),
                    ContractLevel = Session["ContractLevel"].ToString(),
                    ContractSuit = Session["ContractSuit"].ToString(),
                    ContractX = Session["ContractX"].ToString(),
                    NSEW = Session["NSEW"].ToString(),
                    TricksTakenNumber = Convert.ToInt32(Session["TricksTakenNumber"]),
                    LeadCard = Session["LeadCard"].ToString()
                };
                ViewData["DisplayContract"] = res.DisplayContract(2);

                int score = res.Score();
                if (score > 0)
                {
                    ViewData["Direction"] = "NS";
                    ViewData["Score"] = score.ToString();
                }
                else
                {
                    ViewData["Direction"] = "EW";
                    ViewData["Score"] = Convert.ToString(-score);
                }
            }

            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - {Vulnerability.SetPairString("NS", Session["Board"].ToString(), Session["PairNS"].ToString())} v {Vulnerability.SetPairString("EW", Session["Board"].ToString(), Session["PairEW"].ToString())}";
            ViewData["BackButton"] = "TRUE";
            return View();
        }

        public ActionResult OKButtonClick()
        {
            ResultClass res = new ResultClass
            {
                SectionID = Session["SectionID"].ToString(),
                Table = Session["Table"].ToString(),
                Round = Session["Round"].ToString(),
                Board = Session["Board"].ToString(),
                PairNS = Session["PairNS"].ToString(),
                PairEW = Session["PairEW"].ToString(),
                ContractLevel = Session["ContractLevel"].ToString(),
                ContractSuit = Session["ContractSuit"].ToString(),
                ContractX = Session["ContractX"].ToString(),
                NSEW = Session["NSEW"].ToString(),
                TricksTakenNumber = Convert.ToInt32(Session["TricksTakenNumber"]),
                LeadCard = Session["LeadCard"].ToString()
            };

            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "ErrorScreen");
            }

            res.UpdateDB(DBConnectionString);
            Session["Score"] = res.Score().ToString();

            return RedirectToAction("Index", "ShowTraveller");
        }

        public ActionResult BackButtonClick()
        {
            if (Session["ContractLevel"].ToString() == "PASS")
            {
                return RedirectToAction("Index", "EnterContract", new { board = Session["Board"].ToString() });
            }
            else
            {
                return RedirectToAction("Index", "EnterTricksTaken");
            }
        }
    }
}
