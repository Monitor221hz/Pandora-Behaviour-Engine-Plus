using System;
using System.Collections.Generic;

namespace HKX2E;

internal interface IHavokReference
{
	public void Update<T>(T value) where T : IHavokObject;
}