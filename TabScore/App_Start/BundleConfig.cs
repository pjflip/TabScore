// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Optimization;

namespace TabScore
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/coreJS").Include("~/Scripts/bootstrap.js", "~/Scripts/jquery-{version}.js"));
            bundles.Add(new StyleBundle("~/Content/coreCSS").Include("~/Content/bootstrap.css", "~/Content/font-awesome.css"));
 
            bundles.Add(new ScriptBundle("~/bundles/enterContractJS").Include("~/Scripts/EnterContract.js"));
            bundles.Add(new ScriptBundle("~/bundles/enterLeadJS").Include("~/Scripts/EnterLead.js"));
            bundles.Add(new ScriptBundle("~/bundles/enterPlayerIdJS").Include("~/Scripts/EnterPlayerID.js"));
            bundles.Add(new ScriptBundle("~/bundles/individualRankingListJS").Include("~/Scripts/MainLayout.js"));
            bundles.Add(new ScriptBundle("~/bundles/mainLayoutJS").Include("~/Scripts/MainLayout.js"));
            bundles.Add(new ScriptBundle("~/bundles/oneWinnerRankingListJS").Include("~/Scripts/OneWinnerRankingList.js"));
            bundles.Add(new ScriptBundle("~/bundles/showHandRecordJS").Include("~/Scripts/ShowHandRecord.js"));
            bundles.Add(new ScriptBundle("~/bundles/totalTricksJS").Include("~/Scripts/TotalTricks.js"));
            bundles.Add(new ScriptBundle("~/bundles/tricksPlusMinusJS").Include("~/Scripts/TricksPlusMinus.js"));
            bundles.Add(new ScriptBundle("~/bundles/twoWinnersRankingListJS").Include("~/Scripts/TwoWinnersRankingList.js"));
        }
    }
}