// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.Paths.Contexts;
using System.Reactive.Subjects;

namespace PandoraTests.Fakes;

public sealed class FakeUserPathContext : IUserPathContext
{
    private readonly BehaviorSubject<DirectoryInfo> _gameData;
    private readonly BehaviorSubject<DirectoryInfo> _output;

    public FakeUserPathContext(DirectoryInfo gameData, DirectoryInfo output)
    {
        _gameData = new BehaviorSubject<DirectoryInfo>(gameData);
        _output = new BehaviorSubject<DirectoryInfo>(output);

        gameData.Create();
        output.Create();
    }

    public DirectoryInfo GameData => _gameData.Value;
    public DirectoryInfo Output => _output.Value;

    public IObservable<DirectoryInfo> GameDataChanged => _gameData;
    public IObservable<DirectoryInfo> OutputChanged => _output;

    public void SetGameData(DirectoryInfo dir)
    {
        dir.Create();
        _gameData.OnNext(dir);
    }

    public void SetOutput(DirectoryInfo dir)
    {
        dir.Create();
        _output.OnNext(dir);
    }
}
