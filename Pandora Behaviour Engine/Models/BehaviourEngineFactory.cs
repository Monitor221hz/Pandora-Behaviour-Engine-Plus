// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

﻿using Pandora.API;

namespace Pandora.Models;

public class BehaviourEngineFactory()
{
	public static IBehaviourEngine Create()
	{
		return new BehaviourEngine();
	}
}
