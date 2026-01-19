using Pandora.Paths.Abstractions;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Pandora.Paths;

public sealed class UserPaths : IUserPaths
{
	private readonly IApplicationPaths _appPaths;

	private readonly BehaviorSubject<DirectoryInfo> _gameData;
	private readonly BehaviorSubject<DirectoryInfo> _output;

	public UserPaths(IApplicationPaths appPaths)
	{
		_appPaths = appPaths;

		var initial = _appPaths.AssemblyDirectory;

		_gameData = new BehaviorSubject<DirectoryInfo>(initial);
		_output = new BehaviorSubject<DirectoryInfo>(initial);
	}

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
}
