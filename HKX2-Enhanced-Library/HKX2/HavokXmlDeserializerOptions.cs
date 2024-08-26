using System;

namespace HKX2E
{
	[Flags]
	public enum HavokXmlDeserializerOptions
	{
		None, 
		IgnoreNonFatalErrors, 
		IgnoreMissingPointers,
	}
}
