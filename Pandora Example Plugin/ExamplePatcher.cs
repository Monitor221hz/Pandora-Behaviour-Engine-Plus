using Pandora.API.Patch;
using System.Diagnostics;

namespace ExamplePlugin;

public class ExamplePatcher : IPatcher
{
	public IPatcher.PatcherFlags Flags { get;  } = IPatcher.PatcherFlags.None;

	public string GetFailureMessages()
	{
		return "\nFailed. Uh oh!";
	}

	public string GetPostRunMessages()
	{
		return "\nDone!";
	}
	public Version GetVersion()
	{
		return new Version(0, 6, 9);
	}

	public string GetVersionString()
	{
		return $"{GetVersion()}-sigma";
	}

	public async Task PreloadAsync()
	{
		await Task.Run(() => { Debug.WriteLine("Testing preload.");  }); 
	}

	public void Run()
	{
		return;
	}

	public async Task<bool> RunAsync()
	{
		return await Task.Run(() => { return true; });
	}

	public void SetOutputPath(DirectoryInfo directoryInfo)
	{
		return;
	}

	public void SetTarget(List<IModInfo> mods)
	{
		return;
	}

	public void Update()
	{
		return;
	}

	public async Task<bool> UpdateAsync()
	{
		return await Task.Run(() => { return true; });
	}
}
