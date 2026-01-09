using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;

namespace PandoraTests.Utils;

public class TestCapturingExporter : IMetaDataExporter<IPackFile>
{
    private readonly IMetaDataExporter<IPackFile> _realExporter;

    public List<IPackFile> CapturedPackFiles { get; private set; } = new List<IPackFile>();

    public TestCapturingExporter(IMetaDataExporter<IPackFile> realExporter)
    {
        _realExporter = realExporter;
    }
    
    public bool ExportParallel(IEnumerable<IPackFile> packFiles)
    {
        CapturedPackFiles = packFiles.ToList();
        return _realExporter.ExportParallel(packFiles);
    }

    public void LoadMetaData() => _realExporter.LoadMetaData();
    public void SaveMetaData(IEnumerable<IPackFile> packFiles) => _realExporter.SaveMetaData(packFiles);

    public DirectoryInfo GetExportDirectory() => _realExporter.GetExportDirectory();
    public bool Export(IPackFile obj) => _realExporter.Export(obj);
    public IPackFile Import(FileInfo file) => _realExporter.Import(file);

    public bool ExportParallel(IEnumerable<IPackFile> objs, bool useExportMethodFallback = false)
    {
        if (!useExportMethodFallback)
            return ExportParallel(objs);

        bool success = true;
        Parallel.ForEach(
            objs,
            obj =>
            {
                if (!Export(obj))
                {
                    success = false;
                }
            }
        );
        return success;
    }
}