using System.Diagnostics.CodeAnalysis;

namespace HKX2E;
public interface INameHavokObjectMap
{
	T GetObjectAs<T>(string name) where T : class, IHavokObject;
	bool TryGetObject(string name, [NotNullWhen(true)] out IHavokObject? obj);
	bool TryGetObjectAs<T>(string name, [NotNullWhen(true)] out T? obj) where T : class, IHavokObject;
}