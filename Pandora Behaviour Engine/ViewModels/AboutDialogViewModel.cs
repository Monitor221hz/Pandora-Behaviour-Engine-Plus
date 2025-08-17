using Pandora.Utils;
using System.Reflection;

namespace Pandora.ViewModels;

public class AboutDialogViewModel : ViewModelBase
{
	public static string SubHeader => $"Version: {AppInfo.Version}";
	public static string Header => AppInfo.Name;
	public static string Content => "Behaviour engine tool for patching Skyrim Nemesis/FNIS behaviour mods, with full creature support.";

    public AboutDialogViewModel() { }

}
