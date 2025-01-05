using Pandora.Core.Patchers.Skyrim;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HKX2E;
using Pandora.API.Patch.Engine.Skyrim64;
namespace Pandora.Patch.Patchers.Skyrim.Hkx;



public class PackFileCharacter : PackFile, IEquatable<PackFileCharacter>, IPackFileCharacter
{
	public PackFileCharacter(FileInfo file, Project? project) : base(file, project)
	{
		Load();
		InitialAnimationCount = (uint)StringData.animationNames.Count;
	}
	public PackFileCharacter(FileInfo file) : this(file, null) { }
	public hkbCharacterData Data { get; set; }
	public hkbCharacterStringData StringData { get; set; }
	public uint InitialAnimationCount { get; private set; } = 0;
	public uint NewAnimationCount => (uint)StringData.animationNames.Count - InitialAnimationCount;
	public IList<string> AnimationNames => StringData.animationNames;
	public string BehaviorFileName => StringData.behaviorFilename;
	public string SkeletonFileName => StringData.rigName;

	private HashSet<string> uniqueBaseAnimations = new HashSet<string>(StringComparer.OrdinalIgnoreCase); 
	private HashSet<string> uniqueAnimations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	private object uniqueAnimationLock = new();

	private void CacheUniqueBaseAnimations()
	{
		uniqueBaseAnimations = StringData.animationNames.ToHashSet(); 
	}

	[MemberNotNull(nameof(StringData), nameof(Data))]
	public override void Load()
	{
		Data = (hkbCharacterData)Container.namedVariants.First()!.variant!;
		StringData = (hkbCharacterStringData)Data.stringData!;

	}
	public override void PushXmlAsObjects()
	{
		base.PushXmlAsObjects();
	}
	public override void PopPriorityXmlAsObjects()
	{
		PopObjectAsXml(Data);
		PopObjectAsXml(StringData);
	}
	public override void ApplyPriorityChanges(PackFileDispatcher dispatcher)
	{
		base.ApplyPriorityChanges(dispatcher);
		dispatcher.ApplyChangesForNode(Data, this);
		dispatcher.ApplyChangesForNode(StringData, this);
	}
	public override void PushPriorityObjects()
	{
		base.PushPriorityObjects();
		PushXmlAsObject(Data);
		PushXmlAsObject(StringData);
	}
	public bool Equals(PackFileCharacter? other)
	{
		return base.Equals(other);
	}
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	public bool AddUniqueAnimation(string name)
	{
		lock (uniqueBaseAnimations)
		{
			if (uniqueBaseAnimations.Count == 0)
			{
				CacheUniqueBaseAnimations(); 
			}
			if (uniqueBaseAnimations.Contains(name))
			{
				return false; 
			}
		}
		if (ParentProject!.Sibling != null)
		{
			var sibling = ParentProject!.Sibling.CharacterPackFile;
			lock (uniqueAnimationLock) lock (sibling.uniqueAnimationLock)
				{
					if (!sibling.uniqueAnimations.Add(name) || !uniqueAnimations.Add(name))
					{
						return true;
					}
					sibling.StringData.animationNames.Add(name);
					StringData.animationNames.Add(name);
					return true;
				}
		}
		lock (uniqueAnimationLock)
		{
			if (uniqueAnimations.Add(name))
			{
				StringData.animationNames.Add(name);
				return true;
			}
			return true;
		}

	}
}
