// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTricksTakenController : Controller
    {
        public ActionResult Index()
        {
            ViewData["BackButton"] = "TRUE";
            if (Settings.EnterResultsMethod == 1)
            {
                return View("TotalTricks", Session["Result"] as Result);
            }
            else
            {
                return View("TricksPlusMinus", Session["Result"] as Result);
            }
        }

        public ActionResult OKButtonClick(string numTricks)
        {
            Result result = Session["Result"] as Result;
            result.TricksTakenNumber = Convert.ToInt32(numTricks);
            result.CalculateScore();
            Session["Result"] = result;
            return RedirectToAction("Index", "ConfirmResult");
        }

        public ActionResult BackButtonClick()
        {
            if (Settings.EnterLeadCard)
            {
                return RedirectToAction("Index", "EnterLead", new { validateWarning = "NoWarning" });
            }
            else
            {
                return RedirectToAction("Index", "EnterContract", new { boardNumber = (Session["Result"] as Result).BoardNumber });
            }
        }
    }
}