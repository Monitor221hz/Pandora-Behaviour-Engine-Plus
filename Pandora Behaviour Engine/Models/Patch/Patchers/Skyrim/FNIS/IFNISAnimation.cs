using NLog.Targets;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System.Collections.Generic;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;
public interface IFNISAnimation
{
	string AnimationFilePath { get; }
	FNISAnimation.AnimFlags Flags { get; }
	string GraphEvent { get; }
	bool HasModifier { get; }
	FNISAnimation? NextAnimation { get; set; }
	FNISAnimation.AnimType TemplateType { get; }

	bool BuildPatch(FNISAnimationListBuildContext buildContext);
	string ToString();
}