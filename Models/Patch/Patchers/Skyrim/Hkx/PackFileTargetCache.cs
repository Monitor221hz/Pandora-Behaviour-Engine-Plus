using HKX2E;
using Pandora.API.Patch;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
/// <summary>
/// Reuses pack file change sets when possible and outputs targets for patching. Thread safe.
/// </summary>
public class PackFileTargetCache
{
	private Dictionary<PackFile, PackFileTarget> packFileChangeSetMap = new();
	private Dictionary<PackFile, HavokXmlSerializer> packFileSerializerMap = new();

	private ProjectManager projectManager;
	public PackFileTargetCache(IModInfo origin, ProjectManager manager)
	{
		Origin = origin;
		projectManager = manager;
	}

	public IEnumerable<PackFileTarget> Targets => packFileChangeSetMap.Values;

	public IModInfo Origin { get; private set; }

	public HavokXmlSerializer GetSerializer(PackFile packFile)
	{
		HavokXmlSerializer? serializer = null; 
		lock (packFileSerializerMap)
		{
			if (packFileSerializerMap.TryGetValue(packFile, out serializer))
			{
				return serializer;
			}
		}
		serializer = new HavokXmlSerializer();
		lock (packFileSerializerMap)
		{
			packFileSerializerMap.Add(packFile, serializer);
		}
		return serializer;
	}
	public PackFileChangeSet GetChangeSet(PackFile packFile)
	{
		lock (packFileChangeSetMap)
		{
			return packFileChangeSetMap[packFile].ChangeSet;
		}
	} 
	public bool TryGetChangeSet(PackFile packFile, [NotNullWhen(true)] out PackFileChangeSet? changeSet)
	{
		lock (packFileChangeSetMap)
		{
			if (packFileChangeSetMap.TryGetValue(packFile, out var target))
			{
				changeSet = target.ChangeSet;
				return true;
			}
		}
		changeSet = null; 
		return false; 
	}
	public void AddChangeSet(PackFile packFile, PackFileChangeSet changeSet)
	{
		lock (packFileChangeSetMap)
		{
			if (!packFileChangeSetMap.ContainsKey(packFile))
			{
				packFileChangeSetMap.Add(packFile, new PackFileTarget(packFile, changeSet));
			}
		}
		projectManager.TryActivatePackFile(packFile);
	}
	public void AddChange(PackFile packFile, IPackFileChange change)
	{
		lock(packFileChangeSetMap)
		{
			if (packFileChangeSetMap.TryGetValue(packFile, out var target))
			{
				lock(target.ChangeSet)
				{
					target.ChangeSet.AddChange(change);
				}
				return;
			}
		}
		projectManager.TryActivatePackFile(packFile); 
		var changeSet = new PackFileChangeSet(Origin);
		changeSet.AddChange(change); 
		AddChangeSet(packFile, changeSet); 
	}
	/// <summary>
	/// Add a named havok object as a change, provided no existing objects have been added already.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="hkObject"></param>
	/// <param name="packFile"></param>
	/// <param name="name"></param>
	public void AddNamedHkObjectAsChange<T>(T hkObject, PackFile packFile, string name) where T : IHavokObject
	{
		//bool existingReference = false; 
		//XElement element = GetSerializer(packFile).WriteRegisteredNamedObject<T>(hkObject, name, out existingReference);
		//if (!existingReference)
		//{
		//	//AddElementAsChange(packFile, element);
		//}
	}
	public void AddElementAsChange(PackFile packFile, XElement element)
	{
		lock (packFileChangeSetMap)
		{

			if (packFileChangeSetMap.TryGetValue(packFile, out var target))
			{
				lock (target.ChangeSet)
				{
					target.ChangeSet.AddElementAsChange(element);
				}
				return; 
			}
		}
		projectManager.TryActivatePackFile(packFile); 
		var changeSet = new PackFileChangeSet(Origin);
		changeSet.AddElementAsChange(element);
		AddChangeSet(packFile, changeSet);
	}
}
