﻿using System;
using System.Runtime.InteropServices;

namespace TabScoreStarter
{
    public unsafe static class DoubleDummySolver
    {
        public struct ddTableDealPBN
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public char[] cards;
        }

        public struct ddTableResults
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public int[] resTable;
        }

        [DllImport("dds.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void CalcDDtablePBN(ddTableDealPBN tableDealPBN, IntPtr tablep);

        public unsafe static int[] CalcTable(string cardsString)
        {
            ddTableDealPBN tdp = new ddTableDealPBN();
            tdp.cards = new char[80];
            for (int i = 0; i < cardsString.Length; i++)
            {
                tdp.cards[i] = Convert.ToChar(cardsString.Substring(i, 1));
            }

            ddTableResults tr = new ddTableResults();
            IntPtr tablep = Marshal.AllocHGlobal(Marshal.SizeOf(tr));
            Marshal.StructureToPtr(tr, tablep, false);

            CalcDDtablePBN(tdp, tablep);

            tr = (ddTableResults)Marshal.PtrToStructure(tablep, typeof(ddTableResults));
            Marshal.FreeHGlobal(tablep);

            return tr.resTable;
        }
    }
}
