using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.API.Patch.Engine.Skyrim64;
public interface IProject
{
	public string Identifier { get; }

	public IPackFile GetProjectFile();
	public IPackFile GetCharacterPackFile();
	public IPackFile GetSkeletonFile();
	public IPackFile GetBehaviorFile();
	public IPackFile GetSiblingProject();

	public bool TryLookupPackfile(string name, out IPackFile packfile);

	public bool ContainsPackFile(string name);


}