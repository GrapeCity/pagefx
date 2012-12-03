using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    /// <summary>
    /// The Keyboard class is used to build an interface that can be controlled by a user with a standard keyboard.
    /// You can use the methods and properties of the Keyboard class without using a constructor.
    /// The properties of the Keyboard class are constants representing the keys that are
    /// most commonly used to control games.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Keyboard : Avm.Object
    {
        /// <summary>Constant associated with the key code value for the Escape key (27).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint ESCAPE;

        /// <summary>Constant associated with the key code value for the Right Arrow key (39).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint RIGHT;

        /// <summary>Constant associated with the key code value for the Left Arrow key (37).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint LEFT;

        /// <summary>Constant associated with the key code value for the number 7 key on the number pad (103).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_7;

        /// <summary>Constant associated with the key code value for the Tab key (9).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint TAB;

        /// <summary>Constant associated with the key code value for the addition key on the number pad (107).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_ADD;

        /// <summary>Constant associated with the key code value for the Spacebar (32).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint SPACE;

        /// <summary>Constant associated with the key code value for the Down Arrow key (40).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint DOWN;

        /// <summary>Constant associated with the key code value for the Up Arrow key (38).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint UP;

        /// <summary>Constant associated with the key code value for the F1 key (112).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F1;

        /// <summary>Constant associated with the key code value for the F2 key (113).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F2;

        /// <summary>Constant associated with the key code value for the F3 key (114).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F3;

        /// <summary>Constant associated with the key code value for the F4 key (115).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F4;

        /// <summary>Constant associated with the key code value for the F5 key (116).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F5;

        /// <summary>Constant associated with the key code value for the F6 key (117).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F6;

        /// <summary>Constant associated with the key code value for the F7 key (118).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F7;

        /// <summary>Constant associated with the key code value for the Delete key (46).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint DELETE;

        /// <summary>Constant associated with the key code value for the F9 key (120).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F9;

        /// <summary>Constant associated with the key code value for the Enter key (13).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint ENTER;

        /// <summary>Constant associated with the key code value for the Insert key (45).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint INSERT;

        /// <summary>Constant associated with the key code value for the division key on the number pad (111).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_DIVIDE;

        /// <summary>Constant associated with the key code value for the End key (35).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint END;

        /// <summary>Constant associated with the key code value for the Control key (17).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint CONTROL;

        /// <summary>Constant associated with the key code value for the number 1 key on the number pad (97).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_1;

        /// <summary>Constant associated with the key code value for the number 2 key on the number pad (98).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_2;

        /// <summary>Constant associated with the key code value for the F8 key (119).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F8;

        /// <summary>Constant associated with the key code value for the number 4 key on the number pad (100).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_4;

        /// <summary>Constant associated with the key code value for the number 5 key on the number pad (101).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_5;

        /// <summary>Constant associated with the key code value for the number 8 key on the number pad (104).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_8;

        /// <summary>Constant associated with the key code value for the number 9 key on the number pad (105).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_9;

        /// <summary>Constant associated with the key code value for the number 3 key on the number pad (99).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_3;

        /// <summary>Constant associated with the key code value for the Caps Lock key (20).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint CAPS_LOCK;

        /// <summary>Constant associated with the key code value for the number 6 key on the number pad (102).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_6;

        /// <summary>Constant associated with the key code value for the number 0 key on the number pad (96).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_0;

        /// <summary>Constant associated with the key code value for the Enter key on the number pad (108).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_ENTER;

        /// <summary>Constant associated with the key code value for the decimal key on the number pad (110).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_DECIMAL;

        /// <summary>Constant associated with the key code value for the Backspace key (8).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint BACKSPACE;

        /// <summary>Constant associated with the key code value for the Page Down key (34).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint PAGE_DOWN;

        /// <summary>Constant associated with the key code value for the Page Up key (33).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint PAGE_UP;

        /// <summary>Constant associated with the key code value for the F10 key (121).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F10;

        /// <summary>Constant associated with the key code value for the Home key (36).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint HOME;

        /// <summary>Constant associated with the key code value for the F12 key (123).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F12;

        /// <summary>Constant associated with the key code value for the F13 key (124).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F13;

        /// <summary>Constant associated with the key code value for the F14 key (125).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F14;

        /// <summary>Constant associated with the key code value for the F15 key (126).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F15;

        /// <summary>Constant associated with the key code value for the Shift key (16).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint SHIFT;

        /// <summary>Constant associated with the key code value for the subtraction key on the number pad (109).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_SUBTRACT;

        /// <summary>Constant associated with the key code value for the F11 key (122).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F11;

        /// <summary>Constant associated with the key code value for the multiplication key on the number pad (106).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_MULTIPLY;

        public extern static bool capsLock
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool numLock
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Keyboard();

        /// <summary>
        /// Specifies whether the last key pressed is accessible by other SWF files.
        /// By default, security restrictions prevent code from a SWF file in one domain
        /// from accessing a keystroke generated from a SWF file in another domain.
        /// </summary>
        /// <returns>
        /// The value true if the last key pressed can be accessed.
        /// If access is not permitted, this method returns false.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isAccessible();


    }
}
