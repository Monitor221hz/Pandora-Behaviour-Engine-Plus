using System.Collections.Generic;

namespace Pandora.API.Patch.IOManagers;

public interface IMetaDataExporter<T> : IDataExporter<T>
{
	public void LoadMetaData();
	public void SaveMetaData(IEnumerable<T> collection);
}

