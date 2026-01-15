using Pandora.Paths.Contexts;
using Pandora.Services.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Pandora.Services;

public partial class EngineSharedState : ReactiveObject, IEngineSharedState, IDisposable
{
	private readonly CompositeDisposable _disposables = new();

	[Reactive]
	private bool _isEngineRunning;

	[Reactive]
	private bool _isPreloading;

	[Reactive]
	private string _searchTerm;

	[ObservableAsProperty]
	private bool _isOutputFolderCustomSet;

	[ObservableAsProperty]
	private string _outputFolderUri;

	[ObservableAsProperty]
	private string _outputDirectoryMessage;

	public EngineSharedState(
		IEnginePathContext paths)
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
	}
}
