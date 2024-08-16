using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.IOManagers;
public interface IDataExporter<T>
{
	public DirectoryInfo ExportDirectory { get; set; }
	public bool Export(T obj);
	public T Import(FileInfo file);

	public bool ExportParallel(IEnumerable<T> objs)
	{
		bool success = true;
		Parallel.ForEach(objs, obj => { if (!Export(obj)) { success = false; } });
		return success;
	}
}
public interface IMetaDataExporter<T> : IDataExporter<T>
{
	public void LoadMetaData();
	public void SaveMetaData(IEnumerable<T> collection);
}

