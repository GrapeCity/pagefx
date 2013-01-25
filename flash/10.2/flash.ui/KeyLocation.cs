using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    /// <summary>
    /// The KeyLocation class contains constants that indicate the location of a key pressed on the keyboard.
    /// The KeyLocation constants are used in the KeyboardEvent.keyLocation  property.
    /// </summary>
    [PageFX.AbcInstance(251)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class KeyLocation : Avm.Object
    {
        /// <summary>
        /// Indicates the key activation is not distinguished as the left or right version of the key,
        /// and did not originate on the numeric keypad (or did not originate with a virtual
        /// key corresponding to the numeric keypad). Example: The Q key on a PC 101 Key US keyboard.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint STANDARD;

        /// <summary>
        /// Indicates the key activated is in the left key location (there is more than one possible location for this
        /// key).
        /// Example: The left Shift key on a PC 101 Key US keyboard.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint LEFT;

        /// <summary>
        /// Indicates the key activated is in the right key location (there is more than one possible location for this
        /// key).
        /// Example: The right Shift key on a PC 101 Key US keyboard.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint RIGHT;

        /// <summary>
        /// Indicates the key activation originated on the numeric keypad or with a virtual key corresponding
        /// to the numeric keypad. Example: The 1 key on a PC 101 Key US keyboard located on the numeric pad.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUM_PAD;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint D_PAD;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyLocation();
    }
}
