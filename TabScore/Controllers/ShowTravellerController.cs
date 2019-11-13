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
                HandRecord hr = new HandRecord(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Board"]));
                if (hr.NorthSpades == "###")
                {
                    ViewData["HandRecord"] = "FALSE";
                    if (Convert.ToInt32(Session["SectionID"]) != 1)    // Try default Section 1 hand records
                    {
                        hr = new HandRecord(DBConnectionString, 1, Convert.ToInt32(Session["SectionID"]));
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

            List<Result> resList = Traveller.GetResults(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Board"]), Session["IndividualEvent"].ToString()=="YES");
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
