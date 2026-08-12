// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using HKX2E;
using Pandora.API.Patch.Skyrim64;
using Pandora.Skyrim.AnimSetData;
using Pandora.Skyrim.Hkx.Packfile;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Pandora.Skyrim.Format.FNIS;

public class PairedAnimation : BasicAnimation
{
	private static readonly hkbVariableBindingSetBinding SyncedBinding = new()
	{
		bindingType = 0,
		bitIndex = -1,
		variableIndex = 34, //bIsSynced
		memberPath = "bIsActive0",
	};

	public PairedAnimation(Match match)
		: base(FNISAnimType.Paired, match) { }

	public PairedAnimation(
		FNISAnimType templateType,
		FNISAnimFlags flags,
		string graphEvent,
		string animationFilePath,
		List<string> animationObjectNames
	)
		: base(templateType, flags, graphEvent, animationFilePath, animationObjectNames) { }

	public override void BuildAnimation(
		DirectoryInfo templateFolder,
		IFNISAnimationListBuildContext context
	)
	{
		base.BuildAnimation(templateFolder, context);
		var animSetData = context.TargetProject.AnimSetData;
		var inputDirectory = context.TargetProject.ProjectFile.InputHandle.DirectoryName;
		if (animSetData == null || inputDirectory == null)
		{
			return;
		}
		var folderPath = Path.Join(
			"meshes",
			Path.GetRelativePath(templateFolder.FullName, inputDirectory).ToLowerInvariant(),
			"animations",
			context.ModInfo.Name
		);
		var animationName = Path.GetFileNameWithoutExtension(AnimationFilePath);
		foreach (var animset in animSetData.AnimSets)
		{
			animset.AddAnimInfo(new SetCachedAnimInfo().Encode(folderPath, animationName));
		}
	}

	public override bool BuildBehavior(IFNISAnimationListBuildContext buildContext)
	{
		var project = buildContext.TargetProject;
		var projectManager = buildContext.ProjectManager;
		var modInfo = buildContext.ModInfo;

		if (
			!base.BuildBehavior(buildContext)
			|| !project.TryLookupPackFile("1hm_behavior", out var targetPackFile)
			|| targetPackFile is not PackFileGraph graph
		) //only supports humanoids as FNIS does
		{
			return false;
		}

		hkbClipGenerator victimClipGenerator = new()
		{
			name = "PairedAnimation",
			animationName = AnimationFilePath,
		};

		hkbStateMachine victimBehaviorStateMachine = graph.GetPushedObjectAs<hkbStateMachine>(
			"#2458"
		);

		BSSynchronizedClipGenerator victimSyncClipGenerator = new()
		{
			name = GraphEvent,
			pClipGenerator = victimClipGenerator,
			SyncAnimPrefix = "2_",
			bSyncClipIgnoreMarkPlacement = false,
			fGetToMarkTime = 0.0f,
			fMarkErrorThreshold = 0.1f,
			bLeadCharacter = false,
			bReorientSupportChar = false,
			bApplyMotionFromRoot = false,
			sAnimationBindingIndex = -1,
		};
		hkbVariableBindingSet bindingSet = new() { bindings = [SyncedBinding] };
		string uniqueStateInfoName = $"{modInfo.Code}_{GraphEvent}_State";
		int stateId = Hash;
		BSIsActiveModifier isActiveModifier = new BSIsActiveModifier() { enable = true };

		hkbModifierGenerator modifierGenerator = new hkbModifierGenerator()
		{
			modifier = isActiveModifier,
			generator = victimSyncClipGenerator,
		};

		hkbStateMachineStateInfo furnitureGroupStateInfo = new()
		{
			name = uniqueStateInfoName,
			generator = modifierGenerator,
			stateId = stateId,
			// variableBindingSet = bindingSet,
			//transitions = graph.GetPushedObjectAs<hkbStateMachineTransitionInfoArray>("#4005")
		};
		furnitureGroupStateInfo.SetDefault();
		return true;
	}
}