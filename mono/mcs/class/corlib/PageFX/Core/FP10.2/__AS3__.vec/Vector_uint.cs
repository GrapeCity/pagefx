using System;
using System.Runtime.CompilerServices;

namespace __AS3__.vec
{
    [PageFX.AbcInstance(13)]
    [PageFX.ABC]
    [PageFX.QName("Vector$uint", "__AS3__.vec", "internal")]
    [PageFX.FP("10.2")]
    public class Vector_uint : Avm.Object
    {
        public extern virtual uint length
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool @fixed
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector_uint(int length, bool @fixed);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector_uint(int length);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector_uint();

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.QName("toLocaleString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toLocaleString();

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("join", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String join(Avm.String sep);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("join", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String join();

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("every", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool every(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("every", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool every(Avm.Function callback);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("forEach", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void forEach(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("forEach", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void forEach(Avm.Function callback);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("map", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_uint map(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("map", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint map(Avm.Function callback);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint push();

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18, uint item19);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18, uint item19, uint item20);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.QName("some", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool some(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.QName("some", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool some(Avm.Function callback);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint unshift();

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18, uint item19);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18, uint item19, uint item20);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_uint concat();

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18, uint item19);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint concat(uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18, uint item19, uint item20);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_uint filter(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint filter(Avm.Function callback);

        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.QName("pop", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint pop();

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("reverse", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_uint reverse();

        [PageFX.AbcInstanceTrait(26)]
        [PageFX.ABC]
        [PageFX.QName("shift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint shift();

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_uint slice(int startIndex, int endIndex);

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint slice(int startIndex);

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint slice();

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_uint sort(Avm.Function compareFunction);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18, uint item19);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_uint splice(int startIndex, uint deleteCount, uint item1, uint item2, uint item3, uint item4, uint item5, uint item6, uint item7, uint item8, uint item9, uint item10, uint item11, uint item12, uint item13, uint item14, uint item15, uint item16, uint item17, uint item18, uint item19, uint item20);

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int indexOf(uint searchElement, int fromIndex);

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int indexOf(uint searchElement);

        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int lastIndexOf(uint searchElement, int fromIndex);

        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int lastIndexOf(uint searchElement);

        #region Custom Members
        public static implicit operator Vector(Vector_uint v)
        {
            throw new NotImplementedException();
        }
        
        public extern uint this[int index]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }
        #endregion



    }
}
