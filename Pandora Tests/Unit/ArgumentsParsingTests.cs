// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.Utils;

namespace PandoraTests.Unit;

public class ArgumentsParsingTests : IDisposable
{
    private readonly ITestOutputHelper _output;
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
        RunParseTest(_tempDir.FullName, caseInsensitive: false, argsTemplate);
        RunParseTest(_tempDir.FullName, caseInsensitive: true, argsTemplate);
    }

    [Theory(DisplayName = "Parse: Path with spaces")]
    [MemberData(nameof(StandardFormatsWithSpaceData))]
    public void Parse_Should_HandlePathsWithSpaces_WhenQuoted(params string[] argsTemplate)
    {
        RunParseTest(_tempDirWithSpaces.FullName, caseInsensitive: false, argsTemplate);
        RunParseTest(_tempDirWithSpaces.FullName, caseInsensitive: true, argsTemplate);
    }

    [Theory(DisplayName = "Parse: Incorrect fused arguments (Correct Case)")]
    [MemberData(nameof(CorrectCaseCombinedArgumentData))]
    public void Parse_Should_Handle_CorrectCase_FusedArguments(string formatTemplate)
    {
        RunCombinedArgumentsTest(_tempDir.FullName, caseInsensitive: false, formatTemplate);
        RunCombinedArgumentsTest(_tempDir.FullName, caseInsensitive: true, formatTemplate);
    }

    [Theory(DisplayName = "Parse: Incorrect fused arguments (Mixed Case)")]
    [MemberData(nameof(MixedCaseCombinedArgumentData))]
    public void Parse_Should_Handle_MixedCase_FusedArguments_BasedOnFlag(string formatTemplate)
    {
        RunCombinedArgumentsTest(_tempDir.FullName, caseInsensitive: true, formatTemplate);

        var path = _tempDir.FullName;
        var args = new[] { string.Format(formatTemplate, path) };
        _output.WriteLine($"Testing (expect failure, caseSensitive=true): `{args[0]}`");
        var options = LaunchOptions.Parse(args, caseInsensitive: false);

        Assert.Null(options.SkyrimGameDirectory);
    }

    [Theory(DisplayName = "Parse: Value without separator")]
    [InlineData("--tesv{0}", false)]
    [InlineData("-tesv{0}", false)]
    [InlineData("--TeSv{0}", true)]
    [InlineData("-tEsV{0}", true)]
    public void Parse_Should_HandleValueWithoutSeparator(string format, bool isCaseInsensitive)
    {
        RunParseTest(_tempDir.FullName, isCaseInsensitive, format);
    }

    [Theory(DisplayName = "Parse: Multiple arguments")]
    [MemberData(nameof(MultipleArgumentsData))]
    public void Parse_Should_HandleMultipleArguments(params string[] argsTemplate)
    {
        RunMultipleArgumentsTest(caseInsensitive: false, argsTemplate);
        RunMultipleArgumentsTest(caseInsensitive: true, argsTemplate);
    }

    [Theory(DisplayName = "Parse: Mixed case arguments")]
    [MemberData(nameof(CaseVariantFormatsData))]
    public void Parse_Should_HandleMixedCaseArguments_BasedOnFlag(params string[] argsTemplate)
    {
        RunParseTest(_tempDir.FullName, caseInsensitive: true, argsTemplate);

        var path = _tempDir.FullName;
        var args = argsTemplate.Select(arg => string.Format(arg, path)).ToArray();

        _output.WriteLine(
            $"Testing (expect failure, caseSensitive=true): `{string.Join(" ", args)}`"
        );
        var options = LaunchOptions.Parse(args, caseInsensitive: false);

        Assert.Null(options.SkyrimGameDirectory);
    }

    #endregion

    #region Private Helpers

    private void RunParseTest(string path, bool caseInsensitive, params string[] argsTemplate)
    {
        var args = argsTemplate.Select(arg => string.Format(arg, path)).ToArray();

        _output.WriteLine(
            $"Testing (caseInsensitive={caseInsensitive}): ` {string.Join(" ", args)} `"
        );
        var options = LaunchOptions.Parse(args, caseInsensitive);

        Assert.NotNull(options.SkyrimGameDirectory);
        Assert.Equal(path, options.SkyrimGameDirectory.FullName);
    }

    private void RunCombinedArgumentsTest(string path, bool caseInsensitive, string formatTemplate)
    {
        var args = new[] { string.Format(formatTemplate, path) };

        _output.WriteLine(
            $"Testing combined argument (caseInsensitive={caseInsensitive}): `{args[0]}`"
        );
        var options = LaunchOptions.Parse(args, caseInsensitive);

        Assert.NotNull(options.SkyrimGameDirectory);
        Assert.False(options.SkyrimGameDirectory.Exists);
        Assert.Null(options.OutputDirectory);
    }

    private void RunMultipleArgumentsTest(bool caseInsensitive, params string[] argsTemplate)
    {
        var skyrimPath = _tempDir.FullName;
        var outputPath = _tempDirWithSpaces.FullName;
        var args = argsTemplate.Select(arg => string.Format(arg, skyrimPath, outputPath)).ToArray();

        _output.WriteLine(
            $"Testing multiple arguments (caseInsensitive={caseInsensitive}): `{string.Join(" ", args)}`"
        );
        var options = LaunchOptions.Parse(args, caseInsensitive);

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
        LaunchOptions.Current = null;

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
