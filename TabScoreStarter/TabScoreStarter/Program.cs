// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Windows.Forms;

namespace TabScoreStarter
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if TabScoreStarter is already running
            if (System.Diagnostics.Process.GetProcessesByName("TabScoreStarter").Length > 1)
            {
                MessageBox.Show("TabScoreStarter is already running", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(new TabScoreForm());
        }
    }
}
