using System;
using System.Runtime.CompilerServices;
using __AS3__.vec;
using T = $PARAM;

namespace $NS
{
    [Vector("$PARAM")]
    public class $TYPE : Avm.Object
    {
        #region ctors
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE(uint length, bool @fixed);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE(uint length);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE();
        #endregion

        public static implicit operator Vector($TYPE v)
        {
            throw new NotImplementedException();
        }
        
        public extern T this[int index]
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

        ////[QName("shift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual T shift();

        //[QName("reverse", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual $TYPE reverse();

        //[QName("pop", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual T pop();

        #region unshift
        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item);

        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2);
    
        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3);
       
        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4);
      
        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5);
        
        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5, T item6);
       
        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5, T item6, T item7);
     
        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8);
        
        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9);
  
        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10);
        #endregion

        #region indexOf, lastIndexOf
        //[QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int indexOf(T searchElement, int fromIndex);

        //[QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int indexOf(T searchElement);

        //[QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int lastIndexOf(T searchElement, int fromIndex);

        //[QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int lastIndexOf(T searchElement);
        #endregion
        
        #region slice
        //[QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual $TYPE slice(int startIndex, int endIndex);

        //[QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE slice(int startIndex);
        #endregion

        #region concat
        //[QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE concat(T item1);

        //[QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE concat(T item1, T item2);

        //[QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE concat(T item1, T item2, T item3);

        //[QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE concat(T item1, T item2, T item3, T item4);

        //[QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE concat(T item1, T item2, T item3, T item4, T item5);

        //[QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE concat(T item1, T item2, T item3, T item4, T item5, T item6);

        //[QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE concat(T item1, T item2, T item3, T item4, T item5, T item6, T item7);

        //[QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE concat(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8);

        //[QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE concat(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9);

        //[QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE concat(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10);
        #endregion

        #region some
        
        //[QName("some", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool some(Avm.Function callback, object thisObject);

        
        //[QName("some", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool some(Avm.Function callback);
        #endregion

        #region push
        //[QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10, T item11);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10, T item11, T item12);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10, T item11, T item12, T item13);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10, T item11, T item12, T item13, T item14);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10, T item11, T item12, T item13, T item14, T item15);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10, T item11, T item12, T item13, T item14, T item15, T item16);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10, T item11, T item12, T item13, T item14, T item15, T item16, T item17);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10, T item11, T item12, T item13, T item14, T item15, T item16, T item17, T item18);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10, T item11, T item12, T item13, T item14, T item15, T item16, T item17, T item18, T item19);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10, T item11, T item12, T item13, T item14, T item15, T item16, T item17, T item18, T item19, T item20);
        #endregion

        #region every, map, sort, forEach, filter
        //[QName("every", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool every(Avm.Function callback, object thisObject);

        //[QName("every", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool every(Avm.Function callback);

        //[QName("map", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual $TYPE map(Avm.Function callback, object thisObject);

        //[QName("map", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE map(Avm.Function callback);

        //[QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual $TYPE sort(Avm.Function compareFunction);

        //[QName("forEach", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void forEach(Avm.Function callback, object thisObject);

        //[QName("forEach", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void forEach(Avm.Function callback);

        //[QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual $TYPE filter(Avm.Function callback, object thisObject);

        //[QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE filter(Avm.Function callback);
        #endregion

        #region splice
        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual $TYPE splice(int startIndex, uint deleteCount);

        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE splice(int startIndex, uint deleteCount, T item1);

        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE splice(int startIndex, uint deleteCount, T item1, T item2);

        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE splice(int startIndex, uint deleteCount, T item1, T item2, T item3);

        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4);

        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5);

        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5, T item6);

        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5, T item6, T item7);

        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8);

        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9);
       
        //[QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern $TYPE splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9, T item10);
        #endregion

        //[QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        //[QName("toLocaleString", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toLocaleString();

        //[QName("join", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String join(Avm.String sep);

        //[QName("join", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String join();
    }
}