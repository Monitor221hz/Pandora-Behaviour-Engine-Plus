// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Skyrim64;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Pandora.Models.Patch.Skyrim64.Format.FNIS;

struct FNISAnimPreset
{
	public FNISAnimPreset(FNISAnimType animationType, FNISAnimFlags flags)
	{
		AnimationType = animationType;
		Flags = flags;
	}

	public FNISAnimPreset(FNISAnimType animationType)
	{
		AnimationType = animationType;
		Flags = FNISAnimFlags.None;
	}

	public FNISAnimType AnimationType { get; set; }
	public FNISAnimFlags Flags { get; set; }
}

public class FNISAnimationFactory
{
	private static readonly char[] LineWhitespace = [' ', '\t'];
	private static readonly Dictionary<string, FNISAnimPreset> AnimTypePrefixes = new(
		StringComparer.OrdinalIgnoreCase
	)
	{
		{ "b", new(FNISAnimType.Basic) },
		{ "s", new(FNISAnimType.Sequenced) },
		{ "so", new(FNISAnimType.SequencedOptimized) },
		{ "fu", new(FNISAnimType.Furniture) },
		{ "fuo", new(FNISAnimType.FurnitureOptimized) },
		{ "+", new(FNISAnimType.SequencedContinued) },
		{ "ofa", new(FNISAnimType.OffsetArm) },
		{ "o", new(FNISAnimType.Basic, FNISAnimFlags.AnimObjects) },
		{ "pa", new(FNISAnimType.Paired) },
		{ "km", new(FNISAnimType.Killmove) },
		{ "aa", new(FNISAnimType.Alternate) },
		{ "ch", new(FNISAnimType.Chair) },
	};
	private static readonly Dictionary<string, FNISAnimFlags> AnimFlagValues = new()
	{
		{ "a", FNISAnimFlags.Acyclic },
		{ "o", FNISAnimFlags.AnimObjects },
		{ "ac", FNISAnimFlags.AnimatedCamera },
		{ "ac1", FNISAnimFlags.AnimatedCameraSet },
		{ "ac0", FNISAnimFlags.AnimatedCameraReset },
		{ "bsa", FNISAnimFlags.BSA },
		{ "h", FNISAnimFlags.Headtracking },
		{ "k", FNISAnimFlags.Known },
		{ "md", FNISAnimFlags.MotionDriven },
		{ "st", FNISAnimFlags.Sticky },
		{ "Tn", FNISAnimFlags.TransitionNext },
	};
	private readonly Stack<IFNISAnimation> _headAnimationStack = new();
	private AlternateAnimation? _pendingAA;

	private BasicAnimation Create(
		FNISAnimType templateType,
		FNISAnimFlags flags,
		string graphEvent,
		string animationFilePath,
		List<string> animationObjectNames
	)
	{
		switch (templateType)
		{
			case FNISAnimType.OffsetArm:
				return new OffsetArmAnimation(
					templateType,
					flags,
					graphEvent,
					animationFilePath,
					animationObjectNames
				);
			case FNISAnimType.Furniture:
				return new FurnitureAnimation(
					templateType,
					flags,
					graphEvent,
					animationFilePath,
					animationObjectNames
				);
			case FNISAnimType.FurnitureOptimized:
				return new FurnitureAnimation(
					templateType,
					flags,
					graphEvent,
					animationFilePath,
					animationObjectNames
				);
			case FNISAnimType.SequencedContinued:
				if (!_headAnimationStack.TryPop(out var prevAnimation))
				{
					return new BasicAnimation(
						templateType,
						flags,
						graphEvent,
						animationFilePath,
						animationObjectNames
					);
				}
				if (
					prevAnimation.Flags.HasFlag(FNISAnimFlags.Acyclic)
					&& !prevAnimation.Flags.HasFlag(FNISAnimFlags.SequenceFinish)
				)
				{
					prevAnimation.Flags |= FNISAnimFlags.SequenceStart;
				}
				else if (flags.HasFlag(FNISAnimFlags.Acyclic))
				{
					flags |= FNISAnimFlags.SequenceFinish;
				}
				BasicAnimation animation = Create(
					prevAnimation.TemplateType,
					flags,
					graphEvent,
					animationFilePath,
					animationObjectNames
				);
				prevAnimation.NextAnimation = animation;
				return animation;
			default:
				return new BasicAnimation(
					templateType,
					flags,
					graphEvent,
					animationFilePath,
					animationObjectNames
				);
		}
	}

	public bool CreateFromLine(
		string animRoot,
		string line,
		[NotNullWhen(true)] out BasicAnimation? animation,
		out AlternateAnimation? altAnimation
	)
	{
		string[] args = line.Split(
			LineWhitespace,
			StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
		);

		// Alternate ---------------------------------------------------------------------------------------------------
		altAnimation = null;

		// Syntax: AAprefix <prefix: str>
		if (args[0].Equals("AAprefix", StringComparison.OrdinalIgnoreCase))
		{
			if (_pendingAA != null)
			{
				altAnimation = _pendingAA; // <- Need: To support 1 list multi AA definition.
			}

			_pendingAA = new AlternateAnimation(line[9..].Trim(), animRoot);
			animation = null;
			return false;
		}
		// Syntax: AAset <group: str> <slots: int>
		if (_pendingAA != null && args[0].Equals("AAset", StringComparison.OrdinalIgnoreCase))
		{
			if (args.Length >= 3 && int.TryParse(args[2], out int slots))
			{
				_pendingAA.Sets.Add(new AASet(args[1], slots));
			}
			animation = null;
			return false;
		}
		// When other syntax come, then finalize the pending AA and clear it.
		if (_pendingAA != null)
		{
			altAnimation = _pendingAA;
			_pendingAA = null;
			animation = null;
			return false;
		}
		// Alternate End -----------------------------------------------------------------------------------------------

		if (!AnimTypePrefixes.TryGetValue(args[0], out FNISAnimPreset animPreset))
		{
			animation = null;
			return false;
		}
		int optionsOffset = 0;
		FNISAnimFlags flags = animPreset.Flags;
		if (args[1].Length > 0 && args[1][0] == '-')
		{
			string[] flagValues = args[1].Substring(1).Split(',');
			foreach (string flagValue in flagValues)
			{
				if (AnimFlagValues.TryGetValue(flagValue, out FNISAnimFlags flag))
				{
					flags |= flag;
				}
			}
			optionsOffset = 1;
		}
		List<string> animObjectNames = [];
		if (args.Length > optionsOffset + 3)
		{
			for (int i = 4; i < args.Length; i++)
			{
				animObjectNames.Add(args[i]);
			}
		}
		string animationPath = Path.Combine(animRoot, args[2 + optionsOffset]);
		animation = Create(
			animPreset.AnimationType,
			flags,
			args[1 + optionsOffset],
			animationPath,
			animObjectNames
		);
		_headAnimationStack.Push(animation);
		//switch (animPreset.AnimationType)
		//{
		//	case FNISAnimType.OffsetArm:
		//		animation = new OffsetArmAnimation(animPreset.AnimationType, flags, args[1 + optionsOffset], animationPath, animObjectNames);
		//		headAnimationStack.Push(animation);
		//		break;

		//	case FNISAnimType.Furniture:
		//		animation = new FurnitureAnimation(animPreset.AnimationType, flags, args[1 + optionsOffset], animationPath, animObjectNames);
		//		headAnimationStack.Push(animation);
		//		break;
		//	case FNISAnimType.SequencedContinued:
		//		animation = new BasicAnimation(animPreset.AnimationType, flags, args[1 + optionsOffset], animationPath, animObjectNames);
		//		var head = headAnimationStack.Peek();
		//		head.NextAnimation = animation;
		//		headAnimationStack.Push(animation);
		//		break;
		//	default:
		//		animation = new BasicAnimation(animPreset.AnimationType, flags, args[1 + optionsOffset], animationPath, animObjectNames);
		//		headAnimationStack.Push(animation);
		//		break;
		//}
		return true;
	}

	/// <summary>
	/// Returns the pending alternate animation if it exists and clears it.
	///
	/// In the 1-line push pattern, if List.txt ends with `AASet`, it is never assigned to altAnimation.
	/// This fn prevents that issue.
	/// </summary>
	public AlternateAnimation? FinalizePendingAA()
	{
		var altAnimation = _pendingAA;
		_pendingAA = null;
		return altAnimation;
	}
}