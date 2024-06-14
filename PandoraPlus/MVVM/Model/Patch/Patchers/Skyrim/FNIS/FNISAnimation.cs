using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;
public class FNISAnimation
{
	public static readonly Regex AnimLineRegex = new Regex(@"^([^('|\s)]+)\s*(-\S+)*\s*(\S+)\s+(\S+.hkx)(?:[^\S\r\n]+(\S+))*", RegexOptions.Compiled);
	public enum AnimType
	{
		Basic, 
		Sequenced, 
		SequencedOptimized,
		Furniture,
		FurnitureOptimized,
		SequencedContinued,
		OffsetArm,
		Paired,
		Killmove,
		Alternate,
		Chair
	}
	[Flags]
	public enum AnimFlags
	{
		None,
		Acyclic,
		AnimObjects,
		AnimatedCamera,
		AnimatedCameraSet,
		AnimatedCameraReset,
		BSA,
		Headtracking,
		Known,
		MotionDriven,
		Sticky,
		TransitionNext
	}

	private static readonly Dictionary<string, AnimType> animTypePrefixes = new () 
	{ 
		{ "b", AnimType.Basic }, 
		{ "s", AnimType.Sequenced }, 
		{ "so", AnimType.SequencedOptimized },
		{ "fu", AnimType.Furniture },
		{ "fuo", AnimType.FurnitureOptimized },
		{ "+", AnimType.SequencedContinued },
		{ "ofa", AnimType.OffsetArm },
		{ "pa", AnimType.Paired },
		{ "km", AnimType.Killmove },
		{ "aa", AnimType.Alternate },
		{ "ch", AnimType.Chair }
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
	public AnimType Type { get; private set; } = AnimType.Basic;
	public AnimFlags Flags { get; private set; } = AnimFlags.None;

	private List<string> animObjectNames = new();
	public FNISAnimation NextAnimation { get; private set; }

	/// <summary>
	/// Assumes that match has the groups specified in the animLine regex.
	/// </summary>
	/// <param name="match"></param>
    public FNISAnimation(Match match)
    {
		AnimType animType;
		if (animTypePrefixes.TryGetValue(match.Groups[1].Value, out animType))
		{
			Type = animType;
		}
		else
		{
			throw new ArgumentException("Match did not have animTypePrefix");
		}

		if (match.Groups[2].Success)
		{
			var optionValues = match.Groups[2].Value.Split(',');
			AnimFlags animFlags; 
			foreach(var optionValue in optionValues)
			{
				if (animFlagValues.TryGetValue(optionValue, out animFlags))
				{
					Flags |= animFlags;
				}
			}
		}
		if (Flags.HasFlag(AnimFlags.AnimObjects) && match.Groups[5].Success)
		{
			foreach(Capture capture in match.Groups[5].Captures)
			{
				animObjectNames.Add(capture.Value);
			}
		}
	}
	


}
