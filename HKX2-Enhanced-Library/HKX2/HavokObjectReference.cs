namespace HKX2E;

public class HavokObjectReference : IHavokReference
{
	public HavokObjectReference(IHavokObject @object)
	{

		Object = @object;
	}
	public IHavokObject Object { get; set; }
	public void Update<T>(T value) where T : IHavokObject
	{
		Object = value; 
	}
}
