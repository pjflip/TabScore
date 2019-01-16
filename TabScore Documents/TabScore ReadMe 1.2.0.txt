TabScore ReadMe file

TabScore provides wireless scoring for the card game bridge. It is a web 
application that uses IIS to serve web pages across a local wireless network.
It requires a server PC and some sort of table-top device with a browser on
each table (tablet, phone, etc). It uses a Bridgemate .bws standard Access 
database, so is a direct replacement for BridgeTab (or Bridgemate, BridgePad 
etc).  It should work with any bridge scoring program that can create a .bws 
database, but has been built with EBUScore and Jeff Smith's scoring programs 
in mind.

TabScore is designed for use on a PC with Windows 10 (which includes Internet
Information Services (IIS) 10), .NET Framework 4.6.1 and ASP.NET 4.7.

IMPORTANT: Please ensure you have completed all the steps in the 'TabScore 
Installation Guide' document before attempting to run TabScore.

TabScore is currently limited to 4 section (A, B, C and D in that order) and 
30 tables per section.

TabScore implements a limited range of display options which can be set by 
the scoring program.  See the document 'BridgeMate BCS Options in TabScore' 
for more details.

TabScoreStarter uses Bo Haglund's double dummy solver to analyse hand records.