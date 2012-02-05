using System;
using System.Collections;
using System.Reflection;

namespace DataDynamics.PageFX.FLI.SWF
{
	/// <summary>
	/// Factory to create <cref name="SwfAction"> instances.
	/// </summary>
	public static class SwfActionFactory
	{
        private static Hashtable _ctors;

		/// <summary>
		/// Try to create concrete <see cref="SwfAction"/> subclass using given action code.
		/// </summary>
		/// <param name="code">action code</param>
		/// <returns></returns>
		public static SwfAction Create(SwfActionCode code)
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

                    var attr = ReflectionHelper.GetAttribute<SwfActionAttribute>(type, true);
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
                return (SwfAction)ctor.Invoke(null);

			return null;
		}
	}
}