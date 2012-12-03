using System;
using System.Runtime.CompilerServices;

namespace __AS3__.vec
{
    [PageFX.AbcInstance(12)]
    [PageFX.ABC]
    [PageFX.QName("Vector$int", "__AS3__.vec", "internal")]
    [PageFX.FP("10.2")]
    public class Vector_int : Avm.Object
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
        public extern Vector_int(int length, bool @fixed);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector_int(int length);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector_int();

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
        public extern virtual __AS3__.vec.Vector_int map(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("map", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int map(Avm.Function callback);

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
        public extern uint push(int item1);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18, int item19);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18, int item19, int item20);

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
        public extern uint unshift(int item1);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18, int item19);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18, int item19, int item20);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_int concat();

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18, int item19);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int concat(int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18, int item19, int item20);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_int filter(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int filter(Avm.Function callback);

        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.QName("pop", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int pop();

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("reverse", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_int reverse();

        [PageFX.AbcInstanceTrait(26)]
        [PageFX.ABC]
        [PageFX.QName("shift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int shift();

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_int slice(int startIndex, int endIndex);

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int slice(int startIndex);

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int slice();

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_int sort(Avm.Function compareFunction);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18, int item19);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_int splice(int startIndex, uint deleteCount, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9, int item10, int item11, int item12, int item13, int item14, int item15, int item16, int item17, int item18, int item19, int item20);

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int indexOf(int searchElement, int fromIndex);

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int indexOf(int searchElement);

        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int lastIndexOf(int searchElement, int fromIndex);

        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int lastIndexOf(int searchElement);

        #region Custom Members
        public static implicit operator Vector(Vector_int v)
        {
            throw new NotImplementedException();
        }
        
        public extern int this[int index]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }
        #endregion



    }
}
