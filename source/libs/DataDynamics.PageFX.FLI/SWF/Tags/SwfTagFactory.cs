using System;
using System.Collections;
using System.Reflection;

namespace DataDynamics.PageFX.FLI.SWF
{
	/// <summary>
	/// Factory to create <cref name="SwfTag"/> instances.
	/// </summary>
	public static class SwfTagFactory
	{
        private static Hashtable _ctors;

		/// <summary>
		/// Try to create concrete <see cref="SwfTag"/> subclass using given tag code.
		/// </summary>
		/// <param name="code">tag code</param>
		/// <returns></returns>
		public static SwfTag Create(SwfTagCode code)
		{
            //TODO: Can be optimized using simple switch.

		    ConstructorInfo ctor;
            if (_ctors == null)
            {
                _ctors = new Hashtable();

                var asm = typeof(SwfTagFactory).Assembly;

                foreach (var type in asm.GetTypes())
                {
                    if (type.IsAbstract) continue;

                    if (type.IsDefined(typeof(TODOAttribute), true))
                        continue;

                    var attr = ReflectionHelper.GetAttribute<SwfTagAttribute>(type, true);
                    if (attr != null)
                    {
                        ctor = type.GetConstructor(Type.EmptyTypes);
                        if (ctor == null)
                            throw new InvalidOperationException("SwfTag has no default ctor");
                        _ctors[attr.Code] = ctor;
                    }
                }
            }

		    ctor = (ConstructorInfo)_ctors[code];
            if (ctor != null)
                return (SwfTag)ctor.Invoke(null);

			return null;
		}
	}
}