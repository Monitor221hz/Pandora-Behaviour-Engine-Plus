using Pandora.Paths.Abstractions;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Pandora.Paths;

public sealed class UserPaths(IApplicationPaths appPaths) : IUserPaths, IDisposable
{
	private readonly BehaviorSubject<DirectoryInfo> _gameData = new(appPaths.AssemblyDirectory);
	private readonly BehaviorSubject<DirectoryInfo> _output = new(appPaths.AssemblyDirectory);

	public DirectoryInfo GameData => _gameData.Value;
	public DirectoryInfo Output => _output.Value;

	public IObservable<DirectoryInfo> GameDataChanged =>
		_gameData.AsObservable();

	public IObservable<DirectoryInfo> OutputChanged =>
		_output.AsObservable();

	public void SetGameData(DirectoryInfo dir) =>
		_gameData.OnNext(dir);

	public void SetOutput(DirectoryInfo dir) =>
		_output.OnNext(dir);

	public void Dispose()
	{
		_gameData.Dispose();
		_output.Dispose();
	}
}
