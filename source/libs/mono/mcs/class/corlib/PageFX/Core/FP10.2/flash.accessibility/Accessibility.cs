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
    [PageFX.AbcInstance(95)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Accessibility : Avm.Object
    {
        /// <summary>
        /// Indicates whether a screen reader is currently active and the player is
        /// communicating with it. Use this method when you want your application to behave
        /// differently in the presence of a screen reader.
        /// Note: If you call this method within 1 or 2 seconds of the first
        /// appearance of the Flash® window in which your document is playing, you might get a return
        /// value of false even if there is an active accessibility
        /// client. This happens because of an asynchronous communication mechanism between Flash
        /// and accessibility clients. You can work around this limitation by ensuring a delay of 1 to 2
        /// seconds after loading your document before calling this method.
        /// To determine whether the player is running in an environment that supports screen readers, use the
        /// Capabilities.hasAccessibility property.
        /// </summary>
        public extern static bool active
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Accessibility();

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void sendEvent(flash.display.DisplayObject source, uint childID, uint eventType, bool nonHTML);

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void sendEvent(flash.display.DisplayObject source, uint childID, uint eventType);

        /// <summary>
        /// Tells Flash Player to apply any accessibility changes made by using the DisplayObject.accessibilityProperties property.
        /// You need to call this method for your changes to take effect.
        /// If you modify the accessibility properties for multiple objects, only one call to the
        /// Accessibility.updateProperties() method is necessary; multiple calls can result in
        /// reduced performance and erroneous screen reader output.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void updateProperties();
    }
}
