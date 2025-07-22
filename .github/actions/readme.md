# Composite actions

These are reusable workflow fragments.

- See [GitHub docs](https://docs.github.com/actions/tutorials/creating-a-composite-action)

## dotnet publish hack note

| Command                                                                                                             | Result / Error                                                                               |
| ------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- |
| `dotnet publish -c Release --self-contained /p:PublishSingleFile=true`                                              | ❌ Some DLLs, such as `libSkiaSharp.dll`, remain and cannot be launched(cause unknown).      |
| `dotnet publish -c Release --self-contained /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true` | ❌ The dll is statically linked to the exe, but the GUI no longer starts up (cause unknown). |
| `dotnet publish -c Release --self-contained /p:PublishSingleFile=true /p:IncludeAllContentForSelfExtract=true`      | ✅ It can be started, but even the XML template dir is integrated into the exe.              |

### Problem

`/p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true` causes some APIs that retrieve the current directory to point to the tmp dir.

- Pandora Behaviour Engine.csproj

```xml
<ItemGroup>
  <None Update="Nemesis_Engine\**\*.txt">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory> <!-- added  -->
    <ExcludeFromSingleFile>true</ExcludeFromSingleFile> <!-- added  -->
  </None>
</ItemGroup>
```

Using `ExcludeFromSingleFile` allows us to integrate only the DLL into the exe and perform a copy.

However, as mentioned above, `Directory.GetCurrentDirectory()` points to the tmp dir, so even if we start it, the template dir cannot be read, and as of v3.2.0-beta, it crashes without even displaying a log.

Using `Environment.CurrentDirectory` allows us to obtain the correct directory for the exe file, so use this to locate the template directory.

By paying attention to this point, we can create a single exe file.
