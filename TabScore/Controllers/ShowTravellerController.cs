using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowTravellerController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (!Settings.GetSetting<bool>(DBConnectionString, SettingName.ShowResults))
            {
                return RedirectToAction("Index", "ShowBoards");
            }
            if (Settings.GetSetting<bool>(DBConnectionString, SettingName.ShowHandRecord))
            {
                HandRecordClass hr = HandRecord.GetHandRecord(DBConnectionString, Session["SectionID"].ToString(), Session["Board"].ToString());
                if (hr.NorthSpades == "###")
                {
                    ViewData["HandRecord"] = "FALSE";
                    if (Session["SectionID"].ToString() != "1")    // Try default Section 1 hand records
                    {
                        hr = HandRecord.GetHandRecord(DBConnectionString, "1", Session["Board"].ToString());
                        if (hr.NorthSpades != "###")
                        {
                            ViewData["HandRecord"] = "TRUE";
                        }
                    }
                }
                else
                {
                    ViewData["HandRecord"] = "TRUE";
                }
            }
            else
            {
                ViewData["HandRecord"] = "FALSE";
            }

            List<TravellerResultClass> resList = Traveller.GetResults(DBConnectionString, Session["SectionID"].ToString(), Session["Board"].ToString());
            if (resList == null) return RedirectToAction("Index", "ErrorScreen");    // Always at least this table's result unless an error has occurred
            resList.Sort((x, y) => y.Score.CompareTo(x.Score));
            if (Settings.GetSetting<bool>(DBConnectionString, SettingName.ShowPercentage))
            {
                int percentageNS;
                if (resList.Count == 1)
                {
                    percentageNS = 50;
                }
                else
                {
                    int currentScore = Convert.ToInt32(Session["Score"]);
                    int currentMP = 2 * resList.FindAll((x) => x.Score < currentScore).Count + resList.FindAll((x) => x.Score == currentScore).Count - 1;
                    percentageNS = Convert.ToInt32(50.0 * currentMP / (resList.Count - 1));
                }
                ViewData["PercentageNS"] = percentageNS.ToString();
                ViewData["PercentageEW"] = (100 - percentageNS).ToString();
            }
            else
            {
                ViewData["PercentageNS"] = "###";   // Don't show percentage
            }

            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - {Vulnerability.SetPairString("NS", Session["Board"].ToString(), Session["PairNS"].ToString())} v {Vulnerability.SetPairString("EW", Session["Board"].ToString(), Session["PairEW"].ToString())}";
            ViewData["BackButton"] = "TRUE";
            return View(resList);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult BackButtonClick()
        {
            return RedirectToAction("Index", "ConfirmResult");
        }


        public ActionResult HandRecordButtonClick()
        {
            return RedirectToAction("Index", "ShowHandRecord");
        }
    }
}
