using System;
using System.Runtime.CompilerServices;

namespace __AS3__.vec
{
    [PageFX.AbcInstance(11)]
    [PageFX.ABC]
    [PageFX.QName("Vector$object", "__AS3__.vec", "internal")]
    [PageFX.FP("10.2")]
    public class Vector_object : Avm.Object
    {
        public extern virtual uint length
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool @fixed
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector_object(int length, bool @fixed);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector_object(int length);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector_object();

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("toLocaleString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toLocaleString();

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("join", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String join(Avm.String sep);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("join", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String join();

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("every", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool every(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("every", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool every(Avm.Function callback);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("forEach", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void forEach(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("forEach", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void forEach(Avm.Function callback);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.QName("map", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_object map(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.QName("map", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object map(Avm.Function callback);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint push();

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18, object item19);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18, object item19, object item20);

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.QName("some", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool some(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.QName("some", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool some(Avm.Function callback);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint unshift();

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18, object item19);

        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18, object item19, object item20);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_object concat();

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18, object item19);

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object concat(object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18, object item19, object item20);

        [PageFX.AbcInstanceTrait(26)]
        [PageFX.ABC]
        [PageFX.QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_object filter(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(26)]
        [PageFX.ABC]
        [PageFX.QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object filter(Avm.Function callback);

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("pop", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object pop();

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.QName("reverse", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_object reverse();

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.QName("shift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object shift();

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_object slice(int startIndex, int endIndex);

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object slice(int startIndex);

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object slice();

        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_object sort(Avm.Function compareFunction);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18, object item19);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern __AS3__.vec.Vector_object splice(int startIndex, uint deleteCount, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15, object item16, object item17, object item18, object item19, object item20);

        [PageFX.AbcInstanceTrait(33)]
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int indexOf(object searchElement, int fromIndex);

        [PageFX.AbcInstanceTrait(33)]
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int indexOf(object searchElement);

        [PageFX.AbcInstanceTrait(34)]
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int lastIndexOf(object searchElement, int fromIndex);

        [PageFX.AbcInstanceTrait(34)]
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int lastIndexOf(object searchElement);

        #region Custom Members
        public static implicit operator Vector(Vector_object v)
        {
            throw new NotImplementedException();
        }
        
        public extern object this[int index]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }
        #endregion



    }
}
