// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.IOManagers;

public interface IMetaDataExporter<T> : IDataExporter<T>
{
	public void LoadMetaData();
	public void SaveMetaData(IEnumerable<T> collection);
}
