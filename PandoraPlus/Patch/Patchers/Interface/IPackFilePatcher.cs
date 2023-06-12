using Pandora.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Patch
{

    /// <summary>
    /// Interface for hkx patchers. Action keys decide which xml closing comments correspond to their relative patch functions.
    /// </summary>
    public interface IPackFilePatcher
    {
        Dictionary<string, Func<XObject, XParticle, bool>> ActionKeys { get; set; }
        string KeyAttributeName { get; set; }
        string CountAttributeName { get; set; }
        public bool ValidateFile(PackFile  packFile);


    }
}
