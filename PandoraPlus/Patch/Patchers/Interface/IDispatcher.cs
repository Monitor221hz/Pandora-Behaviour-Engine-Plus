using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers
{
	// some overreaching assumptions about the type of data being handled had to be made here for the sake of abstraction.
	// does the role of this interface need to exist? I don't know, but I'd rather have it here just in case.

	public interface IDispatcher<T1>
	{
		public bool Insert(T1 t1);

		public bool Delete(T1 t1);
	}


	public interface IDispatcher<T1, T2>
	{
		public bool Insert(T1 t1, T2 t2); 

		public bool Delete(T1 t1, T2 t2);
	}

	public interface IDispatcher<T1, T2, T3>
	{
		public bool Insert(T1 t1, T2 t2, T3 t3);

		public bool Delete(T1 t1, T2 t2, T3 t3);	

	}





	//C# has no variadic templates so the types must be coded manually

}
