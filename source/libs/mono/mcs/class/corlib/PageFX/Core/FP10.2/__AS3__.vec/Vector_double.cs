using System;
using System.Runtime.CompilerServices;

namespace __AS3__.vec
{
    [PageFX.AbcInstance(14)]
    [PageFX.ABC]
    [PageFX.QName("Vector$double", "__AS3__.vec", "internal")]
    [PageFX.FP("10.2")]
    public class Vector_double : Avm.Object
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
        public extern Vector_double(int length, bool @fixed);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector_double(int length);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector_double();

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
        public extern virtual __AS3__.vec.Vector_double map(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("map", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double map(Avm.Function callback);

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
        public extern uint push(double item1);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18, double item19);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18, double item19, double item20);

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
        public extern uint unshift(double item1);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18, double item19);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18, double item19, double item20);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_double concat();

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18, double item19);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double concat(double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18, double item19, double item20);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_double filter(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double filter(Avm.Function callback);

        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.QName("pop", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double pop();

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("reverse", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_double reverse();

        [PageFX.AbcInstanceTrait(26)]
        [PageFX.ABC]
        [PageFX.QName("shift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double shift();

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_double slice(int startIndex, int endIndex);

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double slice(int startIndex);

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double slice();

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_double sort(Avm.Function compareFunction);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18, double item19);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_double splice(int startIndex, uint deleteCount, double item1, double item2, double item3, double item4, double item5, double item6, double item7, double item8, double item9, double item10, double item11, double item12, double item13, double item14, double item15, double item16, double item17, double item18, double item19, double item20);

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int indexOf(double searchElement, int fromIndex);

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int indexOf(double searchElement);

        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int lastIndexOf(double searchElement, int fromIndex);

        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int lastIndexOf(double searchElement);

        #region Custom Members
        public static implicit operator Vector(Vector_double v)
        {
            throw new NotImplementedException();
        }
        
        public extern double this[int index]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }
        #endregion



    }
}
