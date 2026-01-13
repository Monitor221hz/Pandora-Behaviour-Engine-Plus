using System.IO;
using System.Threading.Tasks;

namespace Pandora.Services.Interfaces;

public interface IDiskDialogService
{
    Task<DirectoryInfo?> OpenFolderAsync(string title, DirectoryInfo? initialDirectory = null);
    Task<FileInfo?> OpenFileAsync(string title, params string[] patterns);
}
