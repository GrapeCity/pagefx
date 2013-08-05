using System;
using System.Collections;
using System.Reflection;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Swf.Tags;

namespace DataDynamics.PageFX.FlashLand.Swf.Actions
{
	/// <summary>
	/// Factory to create <cref name="SwfAction"/> instances.
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

                    var attr = type.GetAttribute<SwfActionAttribute>(true);
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