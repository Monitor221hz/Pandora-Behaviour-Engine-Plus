using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace XmlCake.Linq.Expressions;

public class XMatchCollection : IEnumerable<XMatch>
{
	private List<XMatch> groupCollection = new List<XMatch>();

	public XMatchCollection() { }

	public XMatchCollection(List<XMatch> groups) => groupCollection = groups;

	public bool Success => groupCollection.Count > 0 && groupCollection[0].Success;

	public int Count => groupCollection.Count;

	public List<XMatch> List => groupCollection;

	public IEnumerator<XMatch> GetEnumerator()
	{
		return new XMatchCollectionEnumerator(groupCollection); 
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
}

public class XMatchCollectionEnumerator : IEnumerator<XMatch>
{
	private List<XMatch> groupCollection;

	private int index { get; set;  } = -1; 

	internal XMatchCollectionEnumerator(List<XMatch> groups) => groupCollection=groups;

	public XMatch Current => groupCollection[index];

	object IEnumerator.Current => this.Current; 

	public void Dispose()
	{
		return; 
	}

	public bool MoveNext()
	{
		index++;
		return index < groupCollection.Count;
	}

	public void Reset()
	{
		index = -1; 
	}
}

