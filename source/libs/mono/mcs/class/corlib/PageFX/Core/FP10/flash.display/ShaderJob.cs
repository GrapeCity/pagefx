using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class ShaderJob : flash.events.EventDispatcher
    {
        public extern virtual Shader shader
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int width
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int height
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual object target
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double progress
        {
            [PageFX.ABC]
            [PageFX.FP10]
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
        public extern ShaderJob(Shader arg0, object arg1, int arg2, int arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderJob(Shader arg0, object arg1, int arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderJob(Shader arg0, object arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderJob(Shader arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderJob();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void start();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void cancel();
    }
}
