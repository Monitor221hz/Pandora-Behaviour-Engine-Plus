using System.Collections.Generic;
using System.Xml.Linq;

namespace XmlCake.Linq.Expressions; 




public interface XEdit 
{

    public static List<XStep> XPattern { get; set;} = new List<XStep>(); 


    public bool IsCompatible(List<XNode> source)
    {
        for (int i = 0; i < source.Count; i++)
        {
            XNode node = source[i];
            if (!XPattern[i].IsMatch(node)) return false; 
        }
        return true; 
    }
    public void AssignSource(List<XNode> source);
    public void Perform(XMap map);
}