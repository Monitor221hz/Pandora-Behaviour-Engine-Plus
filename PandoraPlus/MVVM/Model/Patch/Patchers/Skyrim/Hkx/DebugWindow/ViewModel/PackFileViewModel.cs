using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.Hkx.DebugWindow.ViewModel
{
	public class PackFileViewModel
	{
		ObservableCollection<PackFile> DisplayedPackFiles { get; set;  } = new ObservableCollection<PackFile>();
	}
}
