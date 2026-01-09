// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Services;
using Pandora.Services;

namespace PandoraTests;

public class ArgumentsParsingTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly ILaunchOptionsParser _parser;
    private readonly DirectoryInfo _tempDir;
    private readonly DirectoryInfo _tempDirWithSpaces;

    public ArgumentsParsingTests(ITestOutputHelper output)
    {
        _output = output;

        var tempRoot = Path.GetTempPath();
        _tempDir = Directory.CreateDirectory(Path.Combine(tempRoot, $"pandora_test_Temp"));
        _tempDirWithSpaces = Directory.CreateDirectory(
            Path.Combine(tempRoot, $"pandora test Temp")
        );

        _output.WriteLine($"Test directories created:");
        _output.WriteLine($"  - Path: {_tempDir.FullName}");
        _output.WriteLine($"  - Path with spaces: {_tempDirWithSpaces.FullName}");

        _parser = new LaunchOptionsParser();
    }

    #region Test Data Sources

    public static TheoryData<string[]> StandardFormatsData =>
        new()
        {
            { new[] { "--tesv", "{0}" } },
            { new[] { "-tesv", "{0}" } },
            { new[] { "--tesv:{0}" } },
            { new[] { "-tesv:{0}" } },
            { new[] { "--tesv={0}" } },
            { new[] { "-tesv={0}" } },
        };
    public static TheoryData<string[]> StandardFormatsWithSpaceData =>
        new()
        {
            { new[] { "--tesv", "{0}" } },
            { new[] { "-tesv", "{0}" } },
            { new[] { "--tesv:", "{0}" } },
            { new[] { "-tesv:", "{0}" } },
            { new[] { "--tesv=", "{0}" } },
            { new[] { "-tesv=", "{0}" } },
        };

    public static TheoryData<string> CorrectCaseCombinedArgumentData =>
        new() { { "--tesv:\"{0}\"-o:\"{0}\"" }, { "-tesv:\"{0}\"--output:\"{0}\"" } };

    public static TheoryData<string> MixedCaseCombinedArgumentData =>
        new() { { "--TeSv:\"{0}\"-O:\"{0}\"" }, { "-tEsV:\"{0}\"--OuTpUt:\"{0}\"" } };

    public static TheoryData<string[]> MultipleArgumentsData =>
        new()
        {
            { new[] { "--tesv", "{0}", "-o", "{1}", "--auto_run" } },
            { new[] { "-tesv", "{0}", "--output", "{1}", "-ar" } },
            { new[] { "--tesv={0}", "-o:{1}", "--auto_close" } },
        };

    public static TheoryData<string[]> CaseVariantFormatsData =>
        new()
        {
            { new[] { "--TeSv", "{0}" } },
            { new[] { "-tEsV:{0}" } },
            { new[] { "--TESV={0}" } },
        };

    #endregion

    #region Tests

    [Theory(DisplayName = "Parse: Standard formats")]
    [MemberData(nameof(StandardFormatsData))]
    public void Parse_Should_CorrectlyHandleStandardFormats(params string[] argsTemplate)
    {
        RunParseTest(_tempDir.FullName, argsTemplate);
    }

    [Theory(DisplayName = "Parse: Path with spaces")]
    [MemberData(nameof(StandardFormatsWithSpaceData))]
    public void Parse_Should_HandlePathsWithSpaces_WhenQuoted(params string[] argsTemplate)
    {
        RunParseTest(_tempDirWithSpaces.FullName, argsTemplate);
    }

    [Theory(DisplayName = "Parse: Incorrect fused arguments (Correct Case)")]
    [MemberData(nameof(CorrectCaseCombinedArgumentData))]
    public void Parse_Should_Handle_CorrectCase_FusedArguments(string formatTemplate)
    {
        RunCombinedArgumentsTest(_tempDir.FullName, formatTemplate);
    }

    [Theory(DisplayName = "Parse: Incorrect fused arguments (Mixed Case)")]
    [MemberData(nameof(MixedCaseCombinedArgumentData))]
    public void Parse_Should_Handle_MixedCase_FusedArguments_BasedOnFlag(string formatTemplate)
    {
        RunCombinedArgumentsTest(_tempDir.FullName, formatTemplate);
    }

    [Theory(DisplayName = "Parse: Value without separator")]
    [InlineData("--tesv{0}")]
    [InlineData("-tesv{0}")]
    [InlineData("--TeSv{0}")]
    [InlineData("-tEsV{0}")]
    public void Parse_Should_HandleValueWithoutSeparator(string format)
    {
        RunParseTest(_tempDir.FullName, format);
    }

    [Theory(DisplayName = "Parse: Multiple arguments")]
    [MemberData(nameof(MultipleArgumentsData))]
    public void Parse_Should_HandleMultipleArguments(params string[] argsTemplate)
    {
        RunMultipleArgumentsTest(argsTemplate);
    }

    [Theory(DisplayName = "Parse: Mixed case arguments")]
    [MemberData(nameof(CaseVariantFormatsData))]
    public void Parse_Should_HandleMixedCaseArguments_BasedOnFlag(params string[] argsTemplate)
    {
        RunParseTest(_tempDir.FullName, argsTemplate);
    }

    #endregion

    #region Private Helpers

    private void RunParseTest(string path, params string[] argsTemplate)
    {
        var args = argsTemplate.Select(arg => string.Format(arg, path)).ToArray();

        _output.WriteLine(
            $"Testing: ` {string.Join(" ", args)} `"
        );
        var options = _parser.Parse(args);

        Assert.NotNull(options.SkyrimGameDirectory);
        Assert.Equal(path, options.SkyrimGameDirectory.FullName);
    }

    private void RunCombinedArgumentsTest(string path, string formatTemplate)
    {
        var args = new[] { string.Format(formatTemplate, path) };

        _output.WriteLine(
            $"Testing combined argument: `{args[0]}`"
        );
        var options = _parser.Parse(args);

        Assert.NotNull(options.SkyrimGameDirectory);
        Assert.False(options.SkyrimGameDirectory.Exists);
        Assert.Null(options.OutputDirectory);
    }

    private void RunMultipleArgumentsTest(params string[] argsTemplate)
    {
        var skyrimPath = _tempDir.FullName;
        var outputPath = _tempDirWithSpaces.FullName;
        var args = argsTemplate.Select(arg => string.Format(arg, skyrimPath, outputPath)).ToArray();

        _output.WriteLine(
            $"Testing multiple arguments: `{string.Join(" ", args)}`"
        );
        var options = _parser.Parse(args);

        Assert.NotNull(options.SkyrimGameDirectory);
        Assert.Equal(skyrimPath, options.SkyrimGameDirectory.FullName);
        Assert.NotNull(options.OutputDirectory);
        Assert.Equal(outputPath, options.OutputDirectory.FullName);

        if (new[] { "--auto_run", "-ar" }.Any(a => args.Contains(a)))
            Assert.True(options.AutoRun);

        if (new[] { "--auto_close", "-ac" }.Any(a => args.Contains(a)))
            Assert.True(options.AutoClose);
    }

    #endregion

    public void Dispose()
    {
        if (Directory.Exists(_tempDir.FullName))
        {
            Directory.Delete(_tempDir.FullName, true);
        }
        if (Directory.Exists(_tempDirWithSpaces.FullName))
        {
            Directory.Delete(_tempDirWithSpaces.FullName, true);
        }
        _output.WriteLine("--- Test environment clear ---");
    }
}
