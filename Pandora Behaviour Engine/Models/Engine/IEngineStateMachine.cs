// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pandora.Models.Engine;

public interface IEngineStateMachine
{
	EngineState Current { get; }
	IObservable<EngineState> Changes { get; }
	void Transition(EngineState next);
}
