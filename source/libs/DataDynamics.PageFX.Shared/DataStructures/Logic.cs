using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics
{
    public static class Logic
    {
    	public static int CountOf<T>(IEnumerable<T> list, System.Func<T,bool> p)
        {
        	return list.Count(p);
        }
    }
}