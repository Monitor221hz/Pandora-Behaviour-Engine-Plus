using Microsoft.Win32;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Core.Engine.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Pandora.Models.Patch.Engine;
using Pandora.Models.Patch.Engine.Plugins;
using System.Diagnostics;
namespace Pandora.Core
{
    public class BehaviourEngine
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly PluginLoader pluginLoader = new PluginLoader();

        public static readonly DirectoryInfo AssemblyDirectory = new FileInfo(System.Reflection.Assembly.GetEntryAssembly()!.Location).Directory!;

        public static readonly List<IEngineConfigurationPlugin> EngineConfigurations = new List<IEngineConfigurationPlugin>();

        public readonly static DirectoryInfo? SkyrimGameDirectory;
        private static DirectoryInfo ResolveSymbolicLink(DirectoryInfo directory)
        {
            if (directory.Exists)
            {
                // Windows-specific symbolic link detection
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var attributes = directory.Attributes;
                    if (attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        logger.Info($"Detected symbolic link: {directory.FullName}");
                        return directory;
                    }
                }
                // Linux-specific symbolic link detection
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    var linkTarget = directory.ResolveLinkTarget(true);
                    if (linkTarget != null && Directory.Exists(linkTarget.FullName))
                    {
                        logger.Info($"Detected symbolic link: {directory.FullName} pointing to {linkTarget.FullName}");
                        return new DirectoryInfo(linkTarget.FullName);
                    }
                }
            }
            return directory;
        }


        private static void AddConfigurations(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IEngineConfigurationPlugin).IsAssignableFrom(type))
                {
                    IEngineConfigurationPlugin? result = Activator.CreateInstance(type) as IEngineConfigurationPlugin;
                    if (result != null)
                    {
                        EngineConfigurations.Add(result);
                    }
                }
            }
        }
        private static void LoadPlugins()
        {

            var pluginsDirectory = AssemblyDirectory.CreateSubdirectory("Plugins");
            Assembly assembly;
            foreach (DirectoryInfo pluginDirectory in pluginsDirectory.EnumerateDirectories())
            {
#if DEBUG
                // only for debug. DO NOT introduce json field plugin loading to release builds 
                IMetaPluginLoader metaPluginLoader = new JsonPluginLoader();

                if (!metaPluginLoader.TryLoadMetadata(pluginDirectory, out var pluginInfo))
                {
                    continue;
                }
                assembly = metaPluginLoader.LoadPlugin(pluginDirectory, pluginInfo);
#else
				assembly = pluginLoader.LoadPlugin(pluginDirectory);
#endif
                AddConfigurations(assembly);
            }
        }

        public class ModDetection
        {
            private static string? GetActiveMO2Profile(string mo2Path)
            {
                string iniPath = Path.Combine(mo2Path, "ModOrganizer.ini");
                if (File.Exists(iniPath))
                {
                    foreach (var line in File.ReadLines(iniPath))
                    {
                        if (line.StartsWith("selected_profile="))
                        {
                            return line.Substring("selected_profile=".Length).Trim();
                        }
                    }
                }

                logger.Error("Active MO2 profile not found in ModOrganizer.ini.");
                return null;
            }


            private static List<string> GetModsFromActiveProfile(string profilePath)
            {
                string modListPath = Path.Combine(profilePath, "modlist.txt");
                var enabledMods = new List<string>();

                if (File.Exists(modListPath))
                {
                    foreach (var line in File.ReadLines(modListPath))
                    {
                        if (line.StartsWith("+") && !line.StartsWith("#"))
                        {
                            enabledMods.Add(line.Substring(1).Trim());
                        }
                    }

                    logger.Info($"Loaded {enabledMods.Count} active mods from profile: {profilePath}");
                }
                else
                {
                    logger.Error($"modlist.txt not found in profile: {profilePath}");
                }

                return enabledMods;
            }




            private static List<string> GetActiveMods(string modsPath)
            {
                var mods = new List<string>();

                foreach (var file in Directory.GetFiles(modsPath, "*.esp", SearchOption.TopDirectoryOnly))
                {
                    mods.Add(Path.GetFileName(file));
                }

                foreach (var file in Directory.GetFiles(modsPath, "*.esm", SearchOption.TopDirectoryOnly))
                {
                    mods.Add(Path.GetFileName(file));
                }

                logger.Info($"Found {mods.Count} mods: {string.Join(", ", mods)}");
                return mods;
            }

        }

        private static string? GetActiveMO2Profile(string mo2Path)
        {
            string iniPath = Path.Combine(mo2Path, "ModOrganizer.ini");
            if (File.Exists(iniPath))
            {
                foreach (var line in File.ReadLines(iniPath))
                {
                    if (line.StartsWith("selected_profile="))
                    {
                        return line.Substring("selected_profile=".Length).Trim();
                    }
                }
            }

            logger.Error("Active MO2 profile not found in ModOrganizer.ini.");
            return null;
        }

        private static List<string> GetModsFromActiveProfile(string profilePath)
        {
            string modListPath = Path.Combine(profilePath, "modlist.txt");
            var enabledMods = new List<string>();

            if (File.Exists(modListPath))
            {
                foreach (var line in File.ReadLines(modListPath))
                {
                    if (line.StartsWith("+") && !line.StartsWith("#"))
                    {
                        enabledMods.Add(line.Substring(1).Trim());
                    }
                }

                logger.Info($"Loaded {enabledMods.Count} active mods from profile: {profilePath}");
            }
            else
            {
                logger.Error($"modlist.txt not found in profile: {profilePath}");
            }

            return enabledMods;
        }


        public static List<string> DetectMods()
        {
            try
            {
                string? mo2BasePath = GetModOrganizerBasePath();
                if (mo2BasePath != null)
                {
                    string? activeProfile = GetActiveMO2Profile(mo2BasePath);
                    if (activeProfile != null)
                    {
                        string profilePath = Path.Combine(mo2BasePath, "profiles", activeProfile);
                        return GetModsFromActiveProfile(profilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn($"MO2 detection failed: {ex.Message}");
            }

            logger.Error("No mods detected.");
            return new List<string>();
        }

        private static string? GetModOrganizerBasePath()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string mo2BasePath = Path.Combine(localAppData, "ModOrganizer");

            if (Directory.Exists(mo2BasePath))
            {
                logger.Info($"MO2 Base Path: {mo2BasePath}");
                return mo2BasePath;
            }

            logger.Error("MO2 base path not found.");
            return null;
        }

        private static string? ResolveSkyrimPath()
        {
            // Try to get path from registry (Windows)
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string[] registryKeys = new[]
                {
            @"SOFTWARE\Wow6432Node\Bethesda Softworks\Skyrim Special Edition", // Steam
            @"SOFTWARE\WOW6432Node\GOG.com\Games\1711230643" // GOG
        };

                foreach (var keyPath in registryKeys)
                {
                    using (var key = Registry.LocalMachine.OpenSubKey(keyPath, false))
                    {
                        if (key != null)
                        {
                            string? path = key.GetValue("Installed Path") as string;
                            if (!string.IsNullOrEmpty(path))
                            {
                                logger.Info($"Detected Skyrim path from registry: {path}");
                                return Path.Combine(path, "Data");
                            }
                        }
                    }
                }
            }

            // Try to resolve standard paths (Linux/Mac)
            var possiblePaths = new[]
            {
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local/share/Skyrim Special Edition/Data"),
        "/usr/local/share/skyrimse/Data",
        "/opt/SkyrimSpecialEdition/Data"
    };

            foreach (var path in possiblePaths)
            {
                if (Directory.Exists(path))
                {
                    logger.Info($"Detected Skyrim path from standard locations: {path}");
                    return path;
                }
            }

            logger.Error("Unable to resolve Skyrim path.");
            return null;
        }


        static BehaviourEngine()
        {
            LoadPlugins();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var subKey = "SOFTWARE\\Wow6432Node\\Bethesda Softworks\\Skyrim Special Edition";
                using (var key = Registry.LocalMachine.OpenSubKey(subKey, false))
                {
                    string? defaultPath = key?.GetValue("Installed Path") as string;
                    if (defaultPath != null)
                    {
                        var dataPathBethesda = Path.Join(defaultPath, "Data");
                        SkyrimGameDirectory = ResolveSymbolicLink(new DirectoryInfo(dataPathBethesda));
                        return;
                    }
                }

                var subKeyGOG = "SOFTWARE\\WOW6432Node\\GOG.com\\Games\\1711230643";
                using (var key = Registry.LocalMachine.OpenSubKey(subKeyGOG, false))
                {
                    string? defaultPath = key?.GetValue("Installed Path") as string;
                    if (defaultPath != null)
                    {
                        var dataPathGOG = Path.Join(defaultPath, "Data");
                        SkyrimGameDirectory = ResolveSymbolicLink(new DirectoryInfo(dataPathGOG));
                        return;
                    }
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var userHomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var possibleLinuxPaths = new[]
                {
                    Path.Combine(userHomePath, ".local/share/Skyrim Special Edition/Data"),
                    "/usr/local/share/skyrimse/Data",
                    "/opt/SkyrimSpecialEdition/Data"
                };


                foreach (var pathTemplate in possibleLinuxPaths)
                {
                    var path = string.Format(pathTemplate, Environment.UserName);
                    var dataDir = new DirectoryInfo(path);
                    if (dataDir.Exists)
                    {
                        SkyrimGameDirectory = ResolveSymbolicLink(dataDir);
                        return;
                    }
                }
            }

            var args = Environment.GetCommandLineArgs();
            var inputArg = args.Where(s => s.StartsWith("-tesv:", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (!string.IsNullOrEmpty(inputArg))
            {
                var path = inputArg.Substring(6).Trim();
                if (!string.IsNullOrEmpty(path))
                {
                    var dataPathCmd = Path.Join(path, "Data");
                    SkyrimGameDirectory = ResolveSymbolicLink(new DirectoryInfo(dataPathCmd));
                }
            }
        }

        public class BehaviourEngineInternal
        {
            public static DirectoryInfo? SkyrimGameDirectory { get; private set; }

            static BehaviourEngineInternal()
            {
                InitializeSkyrimGameDirectory();
            }

            private static void InitializeSkyrimGameDirectory()
            {
                var args = Environment.GetCommandLineArgs();
                var inputArg = args.FirstOrDefault(s => s.StartsWith("-tesv:", StringComparison.OrdinalIgnoreCase));

                if (inputArg != null)
                {
                    // Extract the path from the argument
                    var pathArr = inputArg.AsSpan().Slice(6);
                    var path = pathArr.Trim().ToString();
                }
            }
        }

        public IEngineConfiguration Configuration { get; private set; } = new SkyrimConfiguration();
public bool IsExternalOutput = false;
private DirectoryInfo CurrentDirectory { get; } = new DirectoryInfo(Directory.GetCurrentDirectory());
public DirectoryInfo OutputPath { get; private set; } = new DirectoryInfo(Directory.GetCurrentDirectory());
public void SetOutputPath(DirectoryInfo outputPath)
{
    OutputPath = outputPath!;
    IsExternalOutput = CurrentDirectory != OutputPath;
    Configuration.Patcher.SetOutputPath(outputPath);
}

public BehaviourEngine()
{

}
public BehaviourEngine(IEngineConfiguration configuration)
{
    Configuration = configuration;
}
public void Launch(List<IModInfo> mods)
{
    logger.Info($"Launching with configuration {Configuration.Name}");
    logger.Info($"Launching with patcher {Configuration.Patcher.GetVersionString()}");
    Configuration.Patcher.SetTarget(mods);
    Configuration.Patcher.Update();
    Configuration.Patcher.Run();
}

public async Task<bool> LaunchAsync(List<IModInfo> mods)
{
    logger.Info($"Launching with configuration {Configuration.Name}");
    logger.Info($"Launching with patcher version {Configuration.Patcher.GetVersionString()}");
    Configuration.Patcher.SetTarget(mods);

    if (!OutputPath.Exists) OutputPath.Create();


    if (!await Configuration.Patcher.UpdateAsync()) { return false; }

    return await Configuration.Patcher.RunAsync();
}

public async Task PreloadAsync()
{
    await Configuration.Patcher.PreloadAsync();
}

public string GetMessages(bool success)
{
    return success ? Configuration.Patcher.GetPostRunMessages() : Configuration.Patcher.GetFailureMessages();
}
	}
}