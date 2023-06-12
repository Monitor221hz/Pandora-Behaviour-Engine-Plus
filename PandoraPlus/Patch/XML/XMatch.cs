using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PandoraPlus.Patch.XML
{
    public class XSnippet
    {
        public XComment startComment;
        public XComment separatingComments;
        public XComment endComments;

        public XSnippet(string start, string separator, string end)
        {
            startComment = new XComment(start);
            separatingComments= new XComment(separator);
            endComments= new XComment(end);

        }
        

    }

}
