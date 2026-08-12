// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.IOManagers;

public interface IDataExporter<T>
{
	public DirectoryInfo GetExportDirectory();
	public bool Export(T obj);
	public T Import(FileInfo file);

	public bool ExportParallel(IEnumerable<T> objs)
	{
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
