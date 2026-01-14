using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Pandora.DTOs;

public partial class EngineSessionState : ReactiveObject, IEngineSessionState
{
	[Reactive]
	private bool _isEngineRunning;

	[Reactive]
	private bool _isPreloading;

	[Reactive]
	private string _searchTerm = string.Empty;

	[Reactive]
	private string _outputFolderUri = string.Empty;
	[Reactive]
	private string _outputDirectoryMessage = string.Empty;

	[Reactive]
	private bool _isOutputFolderCustomSet;
}
