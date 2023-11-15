// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Windows.Forms;

namespace TabScore2Starter
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if TabScore2Starter is already running
            if (System.Diagnostics.Process.GetProcessesByName("TabScore2Starter").Length > 1)
            {
                MessageBox.Show("TabScore2Starter is already running", "TabScore2Starter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(new TabScoreForm());
        }
    }
}
