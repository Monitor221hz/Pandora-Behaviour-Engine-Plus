// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Pandora.API.Patch.IOManagers;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using Pandora.Utils;

namespace Pandora.Models.Patch.IO.Skyrim64;

public abstract class BasePackFileExporter : IMetaDataExporter<PackFile>
{
	protected static readonly FileInfo PreviousOutputFile = PandoraPaths.PreviousOutputFile;
	protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public DirectoryInfo ExportDirectory { get; set; }

	protected BasePackFileExporter()
	{
		ExportDirectory = PandoraPaths.OutputPath;
	}

	public abstract bool Export(PackFile packFile);

	public virtual PackFile Import(FileInfo file) => throw new NotImplementedException();

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

	public virtual void SaveMetaData(IEnumerable<PackFile> packFiles)
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
}
