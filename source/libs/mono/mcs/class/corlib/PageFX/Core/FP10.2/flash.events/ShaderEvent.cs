using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(171)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class ShaderEvent : flash.events.Event
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String COMPLETE;

        public extern virtual flash.display.BitmapData bitmapData
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.utils.ByteArray byteArray
        {
            [PageFX.AbcInstanceTrait(4)]
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

        public extern virtual Avm.Vector<Avm.String> vector
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderEvent(Avm.String type, bool bubbles, bool cancelable, flash.display.BitmapData bitmap, flash.utils.ByteArray array, Avm.Vector<Avm.String> vector);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderEvent(Avm.String type, bool bubbles, bool cancelable, flash.display.BitmapData bitmap, flash.utils.ByteArray array);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderEvent(Avm.String type, bool bubbles, bool cancelable, flash.display.BitmapData bitmap);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
