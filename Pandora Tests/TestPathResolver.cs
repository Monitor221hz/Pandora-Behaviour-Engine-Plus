// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Services;
using System.Diagnostics;

namespace PandoraTests
{
    public class TestPathResolver : IPathResolver
    {
        private const string ACTIVE_MODS_FILENAME = "ActiveMods.json";
        private const string PATH_CONFIG_FILENAME = "Paths.json";
        private const string PREVIOUS_OUTPUT_FILENAME = "PreviousOutput.txt";
        private const string PANDORA_ENGINE_FOLDERNAME = "Pandora_Engine";

        private DirectoryInfo _assemblyDirectory = new(
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)
                ?? throw new NullReferenceException("Main process not found.")
        );

        public static readonly DirectoryInfo _templateDirectory = new(
            Path.Combine(Environment.CurrentDirectory, "Pandora_Engine", "Skyrim", "Template")
        );
        public static readonly DirectoryInfo _outputDirectory = new(
            Path.Combine(Environment.CurrentDirectory, "Output")
        );
        public static readonly DirectoryInfo _currentDirectory = new(Environment.CurrentDirectory);
        public static readonly DirectoryInfo _dataDirectory = new(
            Path.Combine(Environment.CurrentDirectory, "Data")
        );

        public static readonly DirectoryInfo _outputMeshDirectory = new DirectoryInfo(
            Path.Combine(Resources.OutputDirectory.FullName, "meshes")
        );

        static TestPathResolver()
        {
            _outputDirectory.Create();
            _templateDirectory.Create();
            _dataDirectory.Create();
            _outputMeshDirectory.Create();
        }

        public FileInfo GetActiveModsFile()
        {
            return new FileInfo(
                Path.Combine(Resources.CurrentDirectory.FullName, PREVIOUS_OUTPUT_FILENAME)
            );
        }

        public DirectoryInfo GetAssemblyFolder()
        {
            return _assemblyDirectory;
        }

        public DirectoryInfo GetCurrentFolder()
        {
            return _currentDirectory;
        }

        public DirectoryInfo GetGameDataFolder()
        {
            return _dataDirectory;
        }

        public DirectoryInfo GetOutputFolder()
        {
            return _outputDirectory;
        }

        public DirectoryInfo GetOutputMeshFolder()
        {
            return _outputMeshDirectory;
        }

        public DirectoryInfo GetPandoraEngineFolder()
        {
            return new DirectoryInfo(
                Path.Combine(GetGameDataFolder().FullName, PANDORA_ENGINE_FOLDERNAME)
            );
        }

        public FileInfo GetPreviousOutputFile()
        {
            return new FileInfo(Path.Combine(_outputDirectory.FullName, PREVIOUS_OUTPUT_FILENAME));
        }

        public DirectoryInfo GetTemplateFolder()
        {
            return Resources.TemplateDirectory;
        }

        public void SavePathsConfiguration()
        {
            throw new NotImplementedException();
        }

        public void SetGameDataFolder(DirectoryInfo gameDataFolder)
        {
            throw new NotImplementedException();
        }

        public void SetOutputFolder(DirectoryInfo outputFolder)
        {
            throw new NotImplementedException();
        }
    }
}
