namespace Pandora.API.Services;

public interface IDiskDialogService
{
    Task<DirectoryInfo?> OpenFolderAsync(string title);
    Task<FileInfo?> OpenFileAsync(string title, params string[] patterns);
}
