using System.Diagnostics.CodeAnalysis;

namespace HKX2E;
public interface IHavokObjectNameMap
{
	bool TryGetName(IHavokObject obj, [NotNullWhen(true)] out string? name);
}