using Pandora.Core;
using System.Xml;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public interface IPackFileChange
	{
		public enum ChangeType
		{
			Remove,
			Insert,
			Replace,
			Append,
			
			
		}
		public bool Apply(PackFile packFile);

		public bool Revert(PackFile packFile); 

		public ChangeType Type { get; }	
		public XmlNodeType AssociatedType { get; }
		
		public string Path { get; }


	}





}
