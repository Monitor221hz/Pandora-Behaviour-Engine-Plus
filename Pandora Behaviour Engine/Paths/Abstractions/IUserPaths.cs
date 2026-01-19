using System;
using System.IO;

namespace Pandora.Paths.Abstractions;

public interface IUserPaths
{
	DirectoryInfo GameData { get; }
	DirectoryInfo Output { get; }

	IObservable<DirectoryInfo> GameDataChanged { get; }
	IObservable<DirectoryInfo> OutputChanged { get; }

	void SetGameData(DirectoryInfo dir);
	void SetOutput(DirectoryInfo dir);
}
