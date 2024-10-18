﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Pandora.Core;

namespace Pandora.Core;

public class NemesisModInfo : IModInfo
{
	public override int GetHashCode()
	{
		return Code.GetHashCode();
	}
	public bool Equals(IModInfo? other)
	{
		return other == null ? false :
			Code == other.Code &&
		    Version == other.Version;
	}
	public bool Active { get; set; } = false;

	public DirectoryInfo Folder { get; private set; }

	public Dictionary<string, string> StringProperties { get; private set; } = new Dictionary<string, string>();

	public string Name { get; private set; }

	public string Author { get; private set; } 
	public string URL { get; private set; }

	public string Code { get; private set; }

	public Version Version { get; } 

	public IModInfo.ModFormat Format { get; } = IModInfo.ModFormat.Nemesis;

	public uint Priority { get; set; } = 0;

	//internal string Auto { get; set; } = "Default";
	//internal string RequiredFile { get; set; } = "Default";
	//internal string FileToCopy { get; set; } = "Default";
	//internal bool Hidden { get; set; } = false;
	public bool Valid { get; private set; } = false;

	public NemesisModInfo()
	{
		Valid = false;
		Folder = new DirectoryInfo(Directory.GetCurrentDirectory());
	}
	public NemesisModInfo(DirectoryInfo folder, string name, string author, string url, Version version, bool active,  Dictionary<string, string> properties)
	{
		Folder = folder;
		Name = name;
		Author = author;
		URL = url;
		Version = version;
		StringProperties = properties;
		Code = Folder.Name;
		Valid = true;
		Active = active;
	}
	public static NemesisModInfo ParseMetadata(FileInfo file)
	{
		Dictionary<string, string> properties = new Dictionary<string, string>();

		if (!file.Exists)
		{
			return new NemesisModInfo();
		}
		using (StreamReader reader = new StreamReader(file.FullName))
		{
			string s;
			string[] args;

			while ((s = reader.ReadLine()!) != null)
			{
				args = s.Split("=");
				if (args.Length > 1)
				{
					properties.Add(args[0].ToLower().Trim(), args[1].Trim());
				}
			}
		}
		string? name;
		string? author;
		string? url;
		string? hidden;
		string? versionString;
		properties.TryGetValue("name", out name);
		properties.TryGetValue("author", out author);
		properties.TryGetValue("site", out url);
		properties.TryGetValue("hidden", out hidden);
		properties.TryGetValue("version", out versionString);

		Version version = Version.TryParse(versionString, out var value) ? value : new(1,0,0);

		bool active;
		bool.TryParse(hidden, out active);


		return (name != null && author != null && url != null) ? new NemesisModInfo(file.Directory!, name, author, url,version, active, properties) : new NemesisModInfo();
		//add metadata later
	}
}

