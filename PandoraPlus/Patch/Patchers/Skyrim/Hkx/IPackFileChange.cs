using System.Xml;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public interface IPackFileChange
	{
		public enum ChangeType
		{
			Insert,
			Replace,
			Remove
		}
		public bool Apply(PackFile packFile);

		public bool Revert(PackFile packFile); 

		public ChangeType Type { get; }	
		public XmlNodeType AssociatedType { get; }
		
		public string Path { get; }

		public string ModName { get; }
	}





}
