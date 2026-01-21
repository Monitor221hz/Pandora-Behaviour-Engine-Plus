using Pandora.Paths.Abstractions;
using Pandora.Services.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Pandora.Services;

public sealed partial class EngineSharedState : ReactiveObject, IEngineSharedState, IDisposable
{
	private readonly CompositeDisposable _disposables = [];

	[Reactive]
	private bool _isEngineRunning;

	[Reactive]
	private bool _isPreloading;

	[Reactive]
	private string _searchTerm = string.Empty;

	[ObservableAsProperty]
	private bool _isOutputFolderCustomSet;

	[ObservableAsProperty]
	private string _outputFolderUri = string.Empty;

	[ObservableAsProperty]
	private string _outputDirectoryMessage = string.Empty;

	public EngineSharedState(
		IUserPaths paths)
	{
		_outputFolderUriHelper = paths.OutputChanged
			.Select(d => d.FullName)
			.ToProperty(this, x => x.OutputFolderUri)
			.DisposeWith(_disposables);

		_isOutputFolderCustomSetHelper = paths.OutputChanged
			.CombineLatest(
				paths.GameDataChanged,
				(outDir, gameDir) => !outDir.FullName.Equals(gameDir.FullName))
			.ToProperty(this, x => x.IsOutputFolderCustomSet)
			.DisposeWith(_disposables);

		_outputDirectoryMessageHelper = paths.OutputChanged
			.CombineLatest(
				paths.GameDataChanged,
				(outDir, gameDir) =>
					outDir.FullName == gameDir.FullName
						? $"Custom output dir not set. Files output to:"
						: string.Empty)
			.ToProperty(this, x => x.OutputDirectoryMessage)
			.DisposeWith(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
		GC.SuppressFinalize(this);
	}
}
