// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using NLog.Targets;

namespace Pandora.Logging.NLogger.Abstractions;

public interface INLogConfigurator
{
	void Configure(Target fileTarget, Target uiTarget);
}
