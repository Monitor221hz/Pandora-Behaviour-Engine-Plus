using System;
using System.Collections.Generic;
using System.IO;
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
	private static readonly Dictionary<string, FNISAnimPreset> animTypePrefixes = new(StringComparer.OrdinalIgnoreCase)
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
	private static readonly Dictionary<string, AnimFlags> animFlagValues = new()
	{
		{ "a", AnimFlags.Acyclic },
		{ "o", AnimFlags.AnimObjects },
		{ "ac", AnimFlags.AnimatedCamera },
		{ "ac1", AnimFlags.AnimatedCameraSet },
		{ "ac0", AnimFlags.AnimatedCameraReset },
		{ "bsa", AnimFlags.BSA },
		{ "h", AnimFlags.Headtracking },
		{ "k", AnimFlags.Known },
		{ "md", AnimFlags.MotionDriven },
		{ "st", AnimFlags.Sticky },
		{ "Tn", AnimFlags.TransitionNext }

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
	public static IFNISAnimation CreateFromLine(string animRoot, string line)
	{
		string[] args = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		FNISAnimPreset animPreset;
		if (!animTypePrefixes.TryGetValue(args[0], out animPreset))
		{
			throw new ArgumentException("Line did not have animTypePrefix");
		}
		int optionsOffset = 0;
		AnimFlags flags = animPreset.Flags;
		if (args[1].Length > 0 && args[1][0] == '-')
		{
			string[] flagValues = args[1].Substring(1).Split(',');
			AnimFlags flag; 
			foreach (string flagValue in flagValues)
			{
				if (animFlagValues.TryGetValue(flagValue, out flag))
				{
					flags |= flag;
				}
			}
			optionsOffset = 1; 
		}
		List<string> animObjectNames = new(); 
		if (args.Length > optionsOffset + 3)
		{
			for (int i = 4; i < args.Length; i++)
			{
				animObjectNames.Add(args[i]);
			}
		}
		string animationPath = Path.Combine(animRoot, args[2 + optionsOffset]);
		switch (animPreset.AnimationType)
		{
			case AnimType.OffsetArm:
				return new OffsetArmAnimation(animPreset.AnimationType, flags, args[1 + optionsOffset], animationPath, animObjectNames);
			default:
				return new FNISAnimation(animPreset.AnimationType, flags, args[1 + optionsOffset], animationPath, animObjectNames); 
		}
		
	}
}
