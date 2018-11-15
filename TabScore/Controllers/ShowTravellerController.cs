﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowTravellerController : Controller
    {
        public ActionResult Index()
        {
            HandRecordClass hr = HandRecord.GetHandRecord(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Board"].ToString());
            if (hr.NorthSpades == "###")
            {
                ViewData["HandRecord"] = "FALSE";
            }
            else
            {
                ViewData["HandRecord"] = "TRUE";
            }

            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - NS {Session["PairNS"]} v EW {Session["PairEW"]}";
            ViewData["CancelButton"] = "TRUE";
            ViewData["Board"] = Session["Board"];
            ViewData["PairNS"] = Session["PairNS"];

            List<TravellerResultClass> resList = Traveller.GetResults(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Board"].ToString());
            resList.Sort((x, y) => y.Score.CompareTo(x.Score));
            int numEqual = 0;
            int numBelow = 0;
            int thisScore = Convert.ToInt32(Session["Score"]);
            foreach (TravellerResultClass tr in resList)
            {
                if (tr.Score == thisScore) numEqual++;
                if (tr.Score < thisScore) numBelow++;
            }
            int percentageNS;
            if (resList.Count == 1)
            {
                percentageNS = 50;
            }
            else
            {
                percentageNS = Convert.ToInt32(100.0 * (numBelow + 0.5 * (numEqual - 1)) / (resList.Count - 1));
            }
            ViewData["PercentageNS"] = percentageNS.ToString();
            ViewData["PercentageEW"] = (100 - percentageNS).ToString();
            return View(resList);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult CancelButtonClick()
        {
            return RedirectToAction("Index", "ConfirmResult");
        }


        public ActionResult HandRecordButtonClick()
        {
            return RedirectToAction("Index", "ShowHandRecord");
        }

        public ActionResult ControlButtonClick()
        {
            Session["ControlReturnScreen"] = "ShowTraveller";
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}