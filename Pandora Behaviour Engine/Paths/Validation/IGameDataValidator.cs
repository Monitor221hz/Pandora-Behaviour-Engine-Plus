using System.IO;

namespace Pandora.Paths.Validation;

public interface IGameDataValidator
{
	DirectoryInfo? Normalize(DirectoryInfo input);
	bool IsValid(DirectoryInfo input);
}
