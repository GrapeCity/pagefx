using System;
using System.Runtime.CompilerServices;

namespace __AS3__.vec
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class Vector : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector();

        #region Custom Members
        public extern object this[int index]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }
        
        public extern virtual uint length
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }
        
        public extern virtual bool @fixed
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object shift();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector reverse();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int indexOf(object searchElement, int fromIndex);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int indexOf(object searchElement);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object pop();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector slice(int startIndex, int endIndex);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector slice(int startIndex);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector slice();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector concat(object item1);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector concat(object item1, object item2);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector concat(object item1, object item2, object item3);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector concat(object item1, object item2, object item3, object item4);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector concat(object item1, object item2, object item3, object item4, object item5);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector concat(object item1, object item2, object item3, object item4, object item5, object item6);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool some(Avm.Function callback, object thisObject);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool some(Avm.Function callback);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool every(Avm.Function callback, object thisObject);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool every(Avm.Function callback);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector map(Avm.Function callback, object thisObject);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector map(Avm.Function callback);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector sort(Avm.Function compareFunction);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void forEach(Avm.Function callback, object thisObject);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void forEach(Avm.Function callback);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int lastIndexOf(object searchElement, int fromIndex);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int lastIndexOf(object searchElement);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toLocaleString();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String join(Avm.String sep);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String join();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector filter(Avm.Function callback, object thisObject);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector filter(Avm.Function callback);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector splice(int startIndex, uint deleteCount);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector splice(int startIndex, uint deleteCount, object item1);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector splice(int startIndex, uint deleteCount, object item1, object item2);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector splice(int startIndex, uint deleteCount, object item1, object item2, object item3);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10);
        #endregion



    }
}
