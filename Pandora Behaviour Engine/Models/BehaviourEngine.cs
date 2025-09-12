// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.Plugins;
using Pandora.Utils;
using Pandora.Utils.Platform.Windows;
using Pandora.Utils.Skyrim;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Testably.Abstractions;

namespace Pandora.Models;

public class BehaviourEngine
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private static readonly PluginLoader pluginLoader = new();

	/// <summary>
	/// Gets the real file system path to the folder containing the running executable.
	/// 
	/// Unlike typical assembly path methods, this returns the actual disk path even when 
	/// running inside virtualized environments like Mod Organizer 2 (MO2), bypassing the VFS.
	/// </summary>
	public static readonly DirectoryInfo AssemblyDirectory = new(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)!);

	public static readonly DirectoryInfo CurrentDirectory = new(Environment.CurrentDirectory!);
	public static readonly DirectoryInfo SkyrimGameDirectory;
	public static readonly List<IEngineConfigurationPlugin> EngineConfigurations = [];

	public bool IsExternalOutput = false;

	public IEngineConfiguration Configuration { get; private set; } = new SkyrimConfiguration();
	public static DirectoryInfo OutputPath { get; private set; } = new(Environment.CurrentDirectory);

	static BehaviourEngine()
	{
		PluginManager.LoadAllPlugins(AssemblyDirectory);
		var runtimeEnvironment = new PandoraRuntimeEnvironment();
		var registry = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new WindowsRegistry() : null;
		var fileSystem = new RealFileSystem();
		var skyrimPathResolver = new SkyrimPathResolver(runtimeEnvironment, registry, fileSystem);

		IDirectoryInfo abstractSkyrimDir = skyrimPathResolver.Resolve();

		SkyrimGameDirectory = new DirectoryInfo(abstractSkyrimDir.FullName);
	}

	public BehaviourEngine()
	{

	}
	public BehaviourEngine SetOutputPath(DirectoryInfo outputPath)
	{
		OutputPath = outputPath!;
		IsExternalOutput = CurrentDirectory != OutputPath;
		Configuration.Patcher.SetOutputPath(outputPath);
		return this;
	}

	public BehaviourEngine SetConfiguration(IEngineConfiguration? configuration)
	{
		Configuration = configuration ?? new SkyrimConfiguration();
		return this;
	}

	public async Task<bool> LaunchAsync(List<IModInfo> mods)
	{
		logger.Info($"Launching with configuration {Configuration.Name}");
		logger.Info($"Launching with patcher version {Configuration.Patcher.GetVersionString()}");
		Configuration.Patcher.SetTarget(mods);

		if (!OutputPath.Exists) 
			OutputPath.Create();

		if (!await Configuration.Patcher.UpdateAsync()) 
			return false;

		return await Configuration.Patcher.RunAsync();
	}


	public async Task PreloadAsync() => 
		await Configuration.Patcher.PreloadAsync();

	public string GetMessages(bool success) => 
		success ? Configuration.Patcher.GetPostRunMessages() : Configuration.Patcher.GetFailureMessages();
}
