// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Microsoft.Extensions.DependencyInjection;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Pandora.Logging.NLogger.Abstractions;
using Pandora.Logging.NLogger.UI;
using System;

namespace Pandora.Logging.NLogger;

public class NLogTargetsFactory(IServiceProvider serviceProvider) : INLogTargetsFactory
{
	public Target CreateFileTarget(string name, string filePath)
    {
        var fileTarget = new FileTarget(name)
        {
			FileName = "${var:EngineLogDir}/Engine.log",
			DeleteOldFileOnStartup = true,
			Layout = "${level:uppercase=true} : ${message}${exception:format=tostring}"
		};
        
        return WrapInAsync(fileTarget, name);
    }

    public Target CreateUiTarget(string name)
    {
        var rawTarget = serviceProvider.GetRequiredService<ObservableNLogTarget>();
        
        rawTarget.Layout = "${message}${exception:format=tostring}";
        
        return WrapInAsync(rawTarget, name);
    }

    private static AsyncTargetWrapper WrapInAsync(Target target, string name)
    {
        return new AsyncTargetWrapper(target)
        {
            Name = $"{name}_Async",
            QueueLimit = 5000,
            OverflowAction = AsyncTargetWrapperOverflowAction.Discard
        };
    }
}