
using Pandora.API.Patch;
using Pandora.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Pandora.ViewModels;
public class ModInfoViewModel : INotifyPropertyChanged, IEquatable<ModInfoViewModel>
{
	public IModInfo ModInfo { get; }

	public event PropertyChangedEventHandler? PropertyChanged;
	private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public bool Equals(ModInfoViewModel? other)
	{
		return other != null && this.ModInfo.Equals(other.ModInfo);
	}
	public override int GetHashCode()
	{
		return ModInfo.GetHashCode();
	}

	public ModInfoViewModel(IModInfo modInfo)
	{
		ModInfo = modInfo;
	}

	public string Name => ModInfo.Name;

	public string Author => ModInfo.Author;

	public string URL => ModInfo.URL;

	public string Code => ModInfo.Code;

	public Version Version => ModInfo.Version;

	public DirectoryInfo Folder => ModInfo.Folder;

	public IModInfo.ModFormat Format => ModInfo.Format;

	public bool Active
	{
		get => ModInfo.Active;
		set
		{
			ModInfo.Active = value;
			RaisePropertyChanged(nameof(Active));
		}
	}

	public uint Priority
	{
		get => ModInfo.Priority;
		set
		{
			ModInfo.Priority = value;
			RaisePropertyChanged(nameof(Priority));
		}
	}

	
}
