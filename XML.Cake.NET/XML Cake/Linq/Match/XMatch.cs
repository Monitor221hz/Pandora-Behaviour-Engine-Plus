using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlCake.Linq.Expressions; 

public class XMatch : IEnumerable<XNode>
{
	public List<XNode> nodes { get; private set; } = new List<XNode>();

	public XMatch() { }
	public XMatch(List<XNode> nodes) => this.nodes = nodes;

	public bool Success => nodes.Count > 0;

	public XNode this[int index]
	{
		get => nodes[index]; 
		set => nodes[index] = value;
	}

	public int Count => nodes.Count;	


	public IEnumerator<XNode> GetEnumerator()
	{
		return new XMatchEnumerator(nodes);
	}

	public void Filter(Func<XNode, bool> filter)
	{
		nodes = nodes.Where(x => filter(x)).ToList();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
}

public class XMatchEnumerator : IEnumerator<XNode>
{
	private List<XNode> nodeCollection;

	private int index { get; set;  } = -1;

	internal XMatchEnumerator(List<XNode> groups) => nodeCollection = groups;

	public XNode Current => nodeCollection[index];

	object IEnumerator.Current => this.Current;

	public void Dispose()
	{
		return;
	}

	public bool MoveNext()
	{
		index++;
		return index < nodeCollection.Count;
	}

	public void Reset()
	{
		index =-1 ;
	}
}
