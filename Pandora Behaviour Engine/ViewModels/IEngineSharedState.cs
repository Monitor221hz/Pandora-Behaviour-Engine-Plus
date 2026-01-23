namespace Pandora.ViewModels;

public interface IEngineSharedState
{
	bool IsEngineRunning { get; set; }
	bool IsPreloading { get; set; }
	string SearchTerm { get; set; }
	string OutputFolderUri { get; }
	string OutputDirectoryMessage { get; }
	bool IsOutputFolderCustomSet { get; }
}
