// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.Services.Interfaces;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.IO.Skyrim64;

public abstract class BasePackFileExporter : IMetaDataExporter<IPackFile>
{
	protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private readonly IPathResolver _pathResolver;
	protected FileInfo PreviousOutputFile => _pathResolver.GetPreviousOutputFile();

	protected BasePackFileExporter(IPathResolver pathResolver)
	{
		_pathResolver = pathResolver;
	}

	public abstract bool Export(IPackFile packFile);

	public virtual IPackFile Import(FileInfo file) => throw new NotImplementedException();

	public virtual void LoadMetaData()
	{
		if (!PreviousOutputFile.Exists)
		{
			Logger.Warn($"Previous output file not found");
			return;
		}

		try
		{
			using (FileStream readStream = PreviousOutputFile.OpenRead())
			{
				using (StreamReader reader = new(readStream))
				{
					string? expectedLine;
					while ((expectedLine = reader.ReadLine()) != null)
					{
						var fileInfo = new FileInfo(expectedLine);

						if (!fileInfo.Exists)
							continue;

						fileInfo.Delete();
					}
				}
			}
		}
		catch (IOException ex)
		{
			Logger.Error(ex, $"I/O error while reading metadata file: {PreviousOutputFile.Name}");
		}
		catch (Exception ex)
		{
			Logger.Fatal(
				ex,
				$"Unexpected error while processing metadata file: {PreviousOutputFile.Name}"
			);
			throw;
		}
	}

	public virtual void SaveMetaData(IEnumerable<IPackFile> packFiles)
	{
		PreviousOutputFile.Directory?.Create();

		using (
			FileStream writeStream = PreviousOutputFile.Open(
				FileMode.Create,
				FileAccess.Write,
				FileShare.None
			)
		)
		{
			using (StreamWriter writer = new(writeStream))
			{
				foreach (PackFile packFile in packFiles)
				{
					if (!packFile.ExportSuccess)
						continue;

					writer.WriteLine(packFile.OutputHandle.FullName);
				}
			}
		}
	}

	public DirectoryInfo GetExportDirectory()
	{
		return _pathResolver.GetOutputFolder();
	}
}
