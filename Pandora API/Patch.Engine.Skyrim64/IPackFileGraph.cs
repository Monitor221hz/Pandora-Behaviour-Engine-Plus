using HKX2E;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.API.Patch.Engine.Skyrim64;
public interface IPackFileGraph
{
	public hkbBehaviorGraphData Data { get; }
	public hkbBehaviorGraphStringData StringData { get; }
	public hkbVariableValueSet VariableValueSet { get; }
}
