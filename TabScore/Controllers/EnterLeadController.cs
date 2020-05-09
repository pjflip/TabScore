// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterLeadController : Controller
    {
        public ActionResult Index(string validateWarning)
        {
            if (!Settings.EnterLeadCard)
            {
                return RedirectToAction("Index", "EnterTricksTaken");
            }

            Result result = Session["Result"] as Result;
            if (result.LeadCard == "")  // Lead not set, so use validateWarning value as passed to controller
            {
                ViewData["ValidateWarning"] = validateWarning;
            }
            else  // Lead already set, so must be an edit (ie no validation and no warning)
            {
                ViewData["ValidateWarning"] = "NoWarning";
            }
            ViewData["BackButton"] = "TRUE";
            return View(result);
        }

        public ActionResult OKButtonClick(string card, string validateWarning)
        {
            Result result = Session["Result"] as Result;
            if (validateWarning != "Validate" || !Settings.ValidateLeadCard || UtilityFunctions.ValidateLead(AppData.DBConnectionString, result.SectionID, result.BoardNumber, card, result.NSEW))
            {
                result.LeadCard = card;
                Session["Result"] = result;
                return RedirectToAction("Index", "EnterTricksTaken");
            }
            else
            {
                return RedirectToAction("Index", "EnterLead", new { validateWarning = "Warning" });
            }
        }

        public ActionResult BackButtonClick()
        {
            Result result = Session["Result"] as Result;
            return RedirectToAction("Index", "EnterContract", new { boardNumber = result.BoardNumber } );
        }
    }
}