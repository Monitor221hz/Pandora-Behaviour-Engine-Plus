namespace Pandora.DTOs;

public interface IEngineSessionState
{
	bool IsEngineRunning { get; set; }
	bool IsPreloading { get; set; }
	string OutputFolderUri { get; set; }
	string OutputDirectoryMessage { get; set; }
	string SearchTerm { get; set; }
	bool IsOutputFolderCustomSet { get; set; }
}
