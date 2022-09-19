TabScore ReadMe file

TabScore is free and open source software that provides wireless scoring for 
the card game bridge. It is a web application that uses IIS to serve web pages 
across a local wireless network.  It requires a server PC and some sort of 
table-top device with a browser on each table (tablet, phone, etc). It uses 
a Bridgemate .bws standard Access database, so is a direct replacement for 
BridgeTab (or Bridgemate, BridgePad, etc).  Although TabScore has been built 
with EBUScore in mind, it should work with any bridge scoring program that can 
create a .bws database.

TabScore is designed for use on a PC with Windows 10 (which includes Internet
Information Services (IIS) 10), .NET Framework 4.7.2 and ASP.NET 4.7.  It has
been installed and run successfully (but not fully tested) on Windows 7 and 
Windows 11.

IMPORTANT: Please ensure you have completed all the installation steps in the 
'TabScore User Guide' document before attempting to run TabScore.

To upgrade TabScore from a previous installation, please read the section in
the User Guide on Upgrading TabScore.

TabScore is currently limited to 4 sections (A, B, C and D in that order) and 
30 tables per section.  It can be used for pairs, teams, Swiss events, and  
individual events (provided the scoring program supports them).

TabScore can operate in 2 modes: Traditional Mode with one table-top device 
per table (like Bridgemate); or Personal Mode where the table-top devices
move with players.  Personal mode allows players to use their own tablets or 
phones, avoiding the need for bridge clubs to invest in expensive hardware.

TabScore implements a range of display options which can be set by the
scoring program, or from TabScoreStarter.  See the User Guide for more
details.

The TabScore webapp can be configured to display in different languages.
Currently, English, Spanish and German are supported.

TabScoreStarter uses Bo Haglund's Double Dummy Solver (DDS) to analyse 
hand records.  DDS requires the Microsoft Visual C++ Redistributable (x86) 
2015 (or later) to be installed on the PC.

See the NOTICE and LICENSE files.