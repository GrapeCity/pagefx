using System;
using System.Collections.Generic;

class Disposable<T> : IDisposable
{
	public void Dispose ()
	{
	}
}

class Test
{
	static Func<T[]> For<T> (List<T> list)
	{
		T [] t = new T [2];
		return () => {
			for (int i = 0; i < t.Length; ++i) {
				t [i] = list [i];
			}
			
			return t;
		};
	}
	
	static Func<T> Throw<T> (T t)
	{
		T l = t;
		return () => {
			throw new ApplicationException (l.ToString ());
		};
	}
	
	static Func<T> Do<T> (T t)
	{
		T l = t;
		return () => {
			T t2;
			do {
				t2 = l;
			} while (default (T) != null);
			
			return t2;
		};
	}
	
	static Func<T> Lock<T> (T t)
	{
		T l = t;
		return () => {
			lock (l.GetType ())
			{
				l = default (T);
				return l;
			}
		};
	}
	
	static Func<T> Catch<T> (T t)
	{
		T l = t;
		return () => {
			try {
				throw new ApplicationException (l.ToString ());
			} catch {
				return l;
			}
		};
	}
	
	static Func<T> Finally<T> (T t)
	{
		T l = t;
		return () => {
			try {
				l = Lock (l)();
			} finally {
				l = default (T);
			}
			
			return l;
		};
	}
	
	static Func<T> Using<T> (T t)
	{
		T l = t;
		using (var d = new Disposable<T> ())
		{
			return () => {
				return l;
			};
		}
	}
	
	static Func<T> Switch<T> (T t)
	{
		T l = t;
		int? i = 0;
		return () => {
			switch (i) {
				default: return l;
			}
		};
	}
	
	static Func<List<T>> ForForeach<T> (T[] t)
	{
		return () => {
			foreach (T e in t)
				return new List<T> () { e };
			
			throw new ApplicationException ();
		};
	}
	
	public static int Main ()
	{
		if (For (new List<int> { 5, 10 })() [1] != 10)
			return 1;
		
		var t = Throw (5);
		try {
			t ();
			return 2;
		} catch (ApplicationException)
		{
		}
		
		var t3 = Do ("rr");
		if (t3 () != "rr")
			return 3;

		var t4 = Lock ('f');
		if (t4 () != '\0')
			return 4;
		
		var t5 = Catch (3);
		if (t5 () != 3)
			return 5;

		var t6 = Finally (5);
		if (t6 () != 0)
			return 6;
		
		var t7 = Using (1.1);
		if (t7 () != 1.1)
			return 7;
		
		var t8 = Switch (55);
		if (t8 () != 55)
			return 8;
		
		var t9 = ForForeach (new [] { 4, 1 });
		if (t9 ()[0] != 4)
			return 9;
		
		Console.WriteLine ("OK");
		return 0;
	}
}