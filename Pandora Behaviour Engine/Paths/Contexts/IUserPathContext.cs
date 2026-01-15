using System;
using System.IO;

namespace Pandora.Paths.Contexts;

public interface IUserPathContext
{
	DirectoryInfo GameData { get; }
	DirectoryInfo Output { get; }

	IObservable<DirectoryInfo> GameDataChanged { get; }
	IObservable<DirectoryInfo> OutputChanged { get; }

	void SetGameData(DirectoryInfo dir);
	void SetOutput(DirectoryInfo dir);
}
