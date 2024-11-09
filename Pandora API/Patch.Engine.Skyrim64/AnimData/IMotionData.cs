using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.API.Patch.Engine.Skyrim64.AnimData;

public interface IMotionData
{
	void AddDummyClipMotionData(string id);
	List<IClipMotionDataBlock> GetBlocks();
	int GetLineCount();
	string ToString();
	bool TryGetBlock(int id, [NotNullWhen(true)] out IClipMotionDataBlock? block);
}
