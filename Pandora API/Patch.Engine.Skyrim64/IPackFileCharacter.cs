using HKX2E;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.API.Patch.Engine.Skyrim64;
public interface IPackFileCharacter
{
	string BehaviorFileName { get; }
	hkbCharacterData Data { get; set; }
	uint InitialAnimationCount { get; }
	uint NewAnimationCount { get; }
	string SkeletonFileName { get; }
	hkbCharacterStringData StringData { get; set; }
	public void AddUniqueAnimation(string name);
}