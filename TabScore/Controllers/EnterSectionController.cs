using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterSectionController : Controller
    {
        public ActionResult Index()
        {
            List<SectionClass> sectionsList = new List<SectionClass>();
            sectionsList = Sections.GetSections(Session["DBConnectionString"].ToString());

            // Check if Section and Table No saved in cookie are valid - if so use them
            if (Request.Cookies["SectionLetter"] != null)
            {
                string cookieSectionLetter = Request.Cookies["SectionLetter"].Value.ToString();
                if (sectionsList.Exists(x => x.Letter == cookieSectionLetter)){
                    SectionClass sect = sectionsList.Find(x => x.Letter == cookieSectionLetter);
                    string cookieTableNo = Request.Cookies["TableNo"].Value.ToString();
                    if (Convert.ToInt32(cookieTableNo) <= sect.Tables)
                    {
                        if (Tables.LogonTable(Session["DBConnectionString"].ToString(), sect.ID.ToString(), cookieTableNo))
                        {
                            Session["SectionLetter"] = cookieSectionLetter;
                            Session["SectionID"] = sect.ID.ToString();
                            Session["NumTables"] = sect.Tables.ToString();
                            Session["MissingPair"] = sect.MissingPair.ToString();
                            Session["Table"] = cookieTableNo;
                            return RedirectToAction("Index", "ShowPlayerNos");
                        }
                    }
                }
            }
            // Check if only one section - if so use it
            if (sectionsList.Count == 1)
            {
                Session["SectionLetter"] = sectionsList[0].Letter;
                Session["SectionID"] = sectionsList[0].ID.ToString();
                Session["NumTables"] = sectionsList[0].Tables.ToString();
                Session["MissingPair"] = sectionsList[0].MissingPair.ToString();
                return RedirectToAction("Index", "EnterTableNo");
            }
            else
            // Get Section
            {
                ViewBag.Header = "";
                ViewData["CancelButton"] = "FALSE";
                return View(sectionsList);
            }
        }

        public ActionResult OKButtonClick(string s)
        {
            List<SectionClass> sectionsList = new List<SectionClass>();
            sectionsList = Sections.GetSections(Session["DBConnectionString"].ToString());

            Session["SectionLetter"] = s;
            SectionClass sect = sectionsList.Find(x => x.Letter == s);
            Session["SectionID"] = sect.ID.ToString();
            Session["NumTables"] = sect.Tables.ToString();
            Session["MissingPair"] = sect.MissingPair.ToString();
            return RedirectToAction("Index", "EnterTableNo");
        }

        public ActionResult ControlButtonClick()
        {
            Session["ControlReturnScreen"] = "EnterSection";
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}