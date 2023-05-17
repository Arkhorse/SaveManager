using MelonLoader;
using System.Reflection;
using BuildInfo = SaveManager.BuildInfo;

[assembly: AssemblyTitle(BuildInfo.Name)]
[assembly: AssemblyDescription(BuildInfo.Description)]
[assembly: AssemblyCompany(BuildInfo.Company)]
[assembly: AssemblyProduct(BuildInfo.Product)]
[assembly: AssemblyCopyright(BuildInfo.Copyright)]
[assembly: AssemblyTrademark(BuildInfo.Trademark)]
[assembly: AssemblyCulture(BuildInfo.Culture)]

[assembly: AssemblyVersion(BuildInfo.Version)]
[assembly: AssemblyFileVersion(BuildInfo.Version)]
[assembly: MelonInfo(typeof(SaveManager.SaveManager), BuildInfo.Name, BuildInfo.Version, BuildInfo.Author, BuildInfo.DownloadLink)]
[assembly: MelonGame("Hinterland", "TheLongDark")]
