using System.Reflection;

namespace Pandora.ViewModels;

public class AboutDialogViewModel : ViewModelBase
{
    public static string Version => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? "Unknown";
    public static string Header => Assembly.GetExecutingAssembly().GetName().Name ?? "Unknown";
    public static string SubHeader => $"Version: {Version}";
    public static string Content => "Behaviour engine tool for patching Skyrim Nemesis/FNIS behaviour mods, with full creature support.";

    public AboutDialogViewModel() { }

}
