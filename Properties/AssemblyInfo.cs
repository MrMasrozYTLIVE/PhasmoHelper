using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(PhasmoHelper.BuildInfo.Name)]
[assembly: AssemblyDescription(PhasmoHelper.BuildInfo.Description)]
[assembly: AssemblyCompany(PhasmoHelper.BuildInfo.Company)]
[assembly: AssemblyProduct(PhasmoHelper.BuildInfo.Name)]
[assembly: AssemblyCopyright("Copyright © " + PhasmoHelper.BuildInfo.Author + " 2021")]
[assembly: AssemblyTrademark(PhasmoHelper.BuildInfo.Company)]
[assembly: AssemblyVersion(PhasmoHelper.BuildInfo.Version)]
[assembly: AssemblyFileVersion(PhasmoHelper.BuildInfo.Version)]
[assembly: MelonInfo(typeof(PhasmoHelper.Main), PhasmoHelper.BuildInfo.Name, PhasmoHelper.BuildInfo.Version, PhasmoHelper.BuildInfo.Author, PhasmoHelper.BuildInfo.DownloadLink)]
[assembly: MelonGame("Kinetic Games", "Phasmophobia")]