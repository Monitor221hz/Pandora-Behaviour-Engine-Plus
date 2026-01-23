// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System.IO;

namespace Pandora.Paths.Validation;

public interface IGameDataValidator
{
	DirectoryInfo? Normalize(DirectoryInfo input);
	bool IsValid(DirectoryInfo input);
}
