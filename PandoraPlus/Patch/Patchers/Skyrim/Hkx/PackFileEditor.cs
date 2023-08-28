using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlCake.Linq;
using XmlCake.String;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public partial class PackFileEditor
	{
		//these tuples are gross, pretend this implementation doesn't exist

		private List<(string path, XElement element)> ReplaceEdits { get; set; } = new List<(string path, XElement element)>();

		private List<(string path, XElement element)> InsertEdits { get; set; } = new List<(string path, XElement element)>();

		private List<string> RemoveEdits { get; set; } = new List<string>(); 

		private List<(string path, string oldValue, string newValue)> TextReplaceEdits { get; set; } = new List<(string path, string oldValue, string newValue)>();

		private List<(string path, string insertValue)> TextInsertEdits { get; set; } = new List<(string path, string insertValue)>();

		private List<(string path, string removeValue)> TextRemoveEdits { get; set; } = new List<(string path, string removeValue)>(); 






		public void QueueReplaceElement(string path, XElement element) => ReplaceEdits.Add((path, element));

		public void QueueInsertElement(string path, XElement element) => InsertEdits.Add((path, element));

		public void QueueRemoveElement(string path) => RemoveEdits.Add(path);

		public void QueueReplaceText(string path, string oldValue, string newValue) => TextReplaceEdits.Add((path, oldValue, newValue));

		public void QueueInsertText(string path, string insertvalue) => TextInsertEdits.Add((path, insertvalue));

		public void QueueRemoveText(string path, string removeValue) => TextRemoveEdits.Add((path, removeValue));



		private void ApplyReplaceEdits(PackFile packFile)
		{
			foreach (var edit in ReplaceEdits)
			{
				ReplaceElement(packFile, edit.path, edit.element);
			}
		}

		private void ApplyInsertEdits(PackFile packFile)
		{
			foreach (var edit in InsertEdits)
			{
				InsertElement(packFile, edit.path, edit.element);
			}
		}
		private void ApplyTextReplaceEdits(PackFile packFile)
		{
			foreach (var edit in TextReplaceEdits)
			{
				ReplaceText(packFile, edit.path, edit.oldValue, edit.newValue);
			}

		}

		private void ApplyTextInsertEdits(PackFile packFile)
		{
			foreach (var edit in TextInsertEdits)
			{
			}
		}
		public void ApplyEdits(PackFile packFile)
		{
			ApplyReplaceEdits(packFile);

			ApplyInsertEdits(packFile); 

			ApplyTextReplaceEdits(packFile);

			ApplyTextInsertEdits(packFile);

		}
		

	}
}
