using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(103)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class ShaderJob : flash.events.EventDispatcher
    {
        public extern virtual flash.display.Shader shader
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

        public extern virtual object target
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

        public extern virtual int width
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

        public extern virtual int height
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double progress
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("complete")]
        public event flash.events.ShaderEventHandler complete
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderJob(flash.display.Shader shader, object target, int width, int height);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderJob(flash.display.Shader shader, object target, int width);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderJob(flash.display.Shader shader, object target);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderJob(flash.display.Shader shader);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderJob();

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void start(bool waitForCompletion);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void start();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void cancel();


    }
}
