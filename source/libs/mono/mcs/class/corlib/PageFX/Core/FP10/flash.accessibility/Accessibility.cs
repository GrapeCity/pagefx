using System;
using System.Runtime.CompilerServices;

namespace flash.accessibility
{
    /// <summary>
    /// The Accessibility class manages communication with screen readers. Screen readers are a
    /// type of assistive technology for visually impaired users that provides an audio version of
    /// screen content. The methods of the Accessibility class are static—that is, you don&apos;t
    /// have to create an instance of the class to use its methods.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Accessibility : Avm.Object
    {
        public extern static bool active
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Accessibility();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void sendEvent(flash.display.DisplayObject arg0, uint arg1, uint arg2, bool arg3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void sendEvent(flash.display.DisplayObject arg0, uint arg1, uint arg2);

        /// <summary>
        /// Tells Flash Player to apply any accessibility changes made by using the DisplayObject.accessibilityProperties property.
        /// You need to call this method for your changes to take effect.
        /// If you modify the accessibility properties for multiple objects, only one call to the
        /// Accessibility.updateProperties() method is necessary; multiple calls can result in
        /// reduced performance and erroneous screen reader output.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void updateProperties();


    }
}
