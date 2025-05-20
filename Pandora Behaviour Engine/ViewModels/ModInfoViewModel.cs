using Pandora.API.Patch;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.IO;

namespace Pandora.ViewModels;

public partial class ModInfoViewModel : ViewModelBase, IEquatable<ModInfoViewModel>
{
    public IModInfo ModInfo { get; }

    public ModInfoViewModel(IModInfo modInfo)
    {
        ModInfo = modInfo;
        _active = modInfo.Active;
        _priority = modInfo.Priority;

        this.WhenAnyValue(x => x.Active)
            .Subscribe(val => ModInfo.Active = val);

        this.WhenAnyValue(x => x.Priority)
            .Subscribe(val => ModInfo.Priority = val);
    }

    public string Name => ModInfo.Name;
    public string Author => ModInfo.Author;
    public string URL => ModInfo.URL;
    public string Code => ModInfo.Code;
    public Version Version => ModInfo.Version;
    public DirectoryInfo Folder => ModInfo.Folder;
    public IModInfo.ModFormat Format => ModInfo.Format;

    [Reactive] private bool _active;
    [Reactive] private uint _priority;

    public bool Equals(ModInfoViewModel? other)
    {
        return other != null && this.ModInfo.Equals(other.ModInfo);
    }

    public override int GetHashCode()
    {
        return ModInfo.GetHashCode();
    }
}
