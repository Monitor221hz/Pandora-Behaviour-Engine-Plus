using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Pandora.Patch.Patchers.Skyrim.FNIS.FNISAnimation;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;
struct FNISAnimPreset
{
	public FNISAnimPreset(AnimType animationType, AnimFlags flags)
	{
		AnimationType = animationType;
		Flags = flags;
	}
	public FNISAnimPreset(AnimType animationType)
	{
		AnimationType = animationType; 
		Flags = AnimFlags.None;
	}
	public AnimType AnimationType { get; set; }
	public AnimFlags Flags { get; set; }

}

public class FNISAnimationFactory
{
	private static readonly Dictionary<string, FNISAnimPreset> animTypePrefixes = new()
	{
		{ "b", new(AnimType.Basic) },
		{ "s", new(AnimType.Sequenced) },
		{ "so", new(AnimType.SequencedOptimized) },
		{ "fu", new (AnimType.Furniture) },
		{ "fuo", new(AnimType.FurnitureOptimized) },
		{ "+", new (AnimType.SequencedContinued) },
		{ "ofa", new (AnimType.OffsetArm) },
		{ "o", new (AnimType.Basic, AnimFlags.AnimObjects) },
		{ "pa", new (AnimType.Paired) },
		{ "km", new (AnimType.Killmove) },
		{ "aa", new (AnimType.Alternate) },
		{ "ch", new (AnimType.Chair) }
	};
	public static IFNISAnimation CreateFromMatch(Match match)
	{
		FNISAnimPreset animPreset;
		if (!animTypePrefixes.TryGetValue(match.Groups[1].Value, out animPreset))
		{
			throw new ArgumentException("Match did not have animTypePrefix");
		}
		switch (animPreset.AnimationType)
		{
			case AnimType.OffsetArm:
				return new OffsetArmAnimation(match);
			default:
				return new FNISAnimation(match); 
		}

		throw new NotImplementedException();
	}
}
