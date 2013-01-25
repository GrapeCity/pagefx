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
    [PageFX.AbcInstance(367)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Keyboard : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_UPARROW;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_DOWNARROW;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_LEFTARROW;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_RIGHTARROW;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F1;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F2;

        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F3;

        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F4;

        [PageFX.AbcClassTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F5;

        [PageFX.AbcClassTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F6;

        [PageFX.AbcClassTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F7;

        [PageFX.AbcClassTrait(11)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F8;

        [PageFX.AbcClassTrait(12)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F9;

        [PageFX.AbcClassTrait(13)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F10;

        [PageFX.AbcClassTrait(14)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F11;

        [PageFX.AbcClassTrait(15)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F12;

        [PageFX.AbcClassTrait(16)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F13;

        [PageFX.AbcClassTrait(17)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F14;

        [PageFX.AbcClassTrait(18)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F15;

        [PageFX.AbcClassTrait(19)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F16;

        [PageFX.AbcClassTrait(20)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F17;

        [PageFX.AbcClassTrait(21)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F18;

        [PageFX.AbcClassTrait(22)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F19;

        [PageFX.AbcClassTrait(23)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F20;

        [PageFX.AbcClassTrait(24)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F21;

        [PageFX.AbcClassTrait(25)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F22;

        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F23;

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F24;

        [PageFX.AbcClassTrait(28)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F25;

        [PageFX.AbcClassTrait(29)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F26;

        [PageFX.AbcClassTrait(30)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F27;

        [PageFX.AbcClassTrait(31)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F28;

        [PageFX.AbcClassTrait(32)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F29;

        [PageFX.AbcClassTrait(33)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F30;

        [PageFX.AbcClassTrait(34)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F31;

        [PageFX.AbcClassTrait(35)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F32;

        [PageFX.AbcClassTrait(36)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F33;

        [PageFX.AbcClassTrait(37)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F34;

        [PageFX.AbcClassTrait(38)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_F35;

        [PageFX.AbcClassTrait(39)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_INSERT;

        [PageFX.AbcClassTrait(40)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_DELETE;

        [PageFX.AbcClassTrait(41)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_HOME;

        [PageFX.AbcClassTrait(42)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_BEGIN;

        [PageFX.AbcClassTrait(43)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_END;

        [PageFX.AbcClassTrait(44)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_PAGEUP;

        [PageFX.AbcClassTrait(45)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_PAGEDOWN;

        [PageFX.AbcClassTrait(46)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_PRINTSCREEN;

        [PageFX.AbcClassTrait(47)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_SCROLLLOCK;

        [PageFX.AbcClassTrait(48)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_PAUSE;

        [PageFX.AbcClassTrait(49)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_SYSREQ;

        [PageFX.AbcClassTrait(50)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_BREAK;

        [PageFX.AbcClassTrait(51)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_RESET;

        [PageFX.AbcClassTrait(52)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_STOP;

        [PageFX.AbcClassTrait(53)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_MENU;

        [PageFX.AbcClassTrait(54)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_USER;

        [PageFX.AbcClassTrait(55)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_SYSTEM;

        [PageFX.AbcClassTrait(56)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_PRINT;

        [PageFX.AbcClassTrait(57)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_CLEARLINE;

        [PageFX.AbcClassTrait(58)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_CLEARDISPLAY;

        [PageFX.AbcClassTrait(59)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_INSERTLINE;

        [PageFX.AbcClassTrait(60)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_DELETELINE;

        [PageFX.AbcClassTrait(61)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_INSERTCHAR;

        [PageFX.AbcClassTrait(62)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_DELETECHAR;

        [PageFX.AbcClassTrait(63)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_PREV;

        [PageFX.AbcClassTrait(64)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_NEXT;

        [PageFX.AbcClassTrait(65)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_SELECT;

        [PageFX.AbcClassTrait(66)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_EXECUTE;

        [PageFX.AbcClassTrait(67)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_UNDO;

        [PageFX.AbcClassTrait(68)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_REDO;

        [PageFX.AbcClassTrait(69)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_FIND;

        [PageFX.AbcClassTrait(70)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_HELP;

        [PageFX.AbcClassTrait(71)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEYNAME_MODESWITCH;

        [PageFX.AbcClassTrait(72)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_UPARROW;

        [PageFX.AbcClassTrait(73)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_DOWNARROW;

        [PageFX.AbcClassTrait(74)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_LEFTARROW;

        [PageFX.AbcClassTrait(75)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_RIGHTARROW;

        [PageFX.AbcClassTrait(76)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F1;

        [PageFX.AbcClassTrait(77)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F2;

        [PageFX.AbcClassTrait(78)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F3;

        [PageFX.AbcClassTrait(79)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F4;

        [PageFX.AbcClassTrait(80)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F5;

        [PageFX.AbcClassTrait(81)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F6;

        [PageFX.AbcClassTrait(82)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F7;

        [PageFX.AbcClassTrait(83)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F8;

        [PageFX.AbcClassTrait(84)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F9;

        [PageFX.AbcClassTrait(85)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F10;

        [PageFX.AbcClassTrait(86)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F11;

        [PageFX.AbcClassTrait(87)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F12;

        [PageFX.AbcClassTrait(88)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F13;

        [PageFX.AbcClassTrait(89)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F14;

        [PageFX.AbcClassTrait(90)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F15;

        [PageFX.AbcClassTrait(91)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F16;

        [PageFX.AbcClassTrait(92)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F17;

        [PageFX.AbcClassTrait(93)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F18;

        [PageFX.AbcClassTrait(94)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F19;

        [PageFX.AbcClassTrait(95)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F20;

        [PageFX.AbcClassTrait(96)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F21;

        [PageFX.AbcClassTrait(97)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F22;

        [PageFX.AbcClassTrait(98)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F23;

        [PageFX.AbcClassTrait(99)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F24;

        [PageFX.AbcClassTrait(100)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F25;

        [PageFX.AbcClassTrait(101)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F26;

        [PageFX.AbcClassTrait(102)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F27;

        [PageFX.AbcClassTrait(103)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F28;

        [PageFX.AbcClassTrait(104)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F29;

        [PageFX.AbcClassTrait(105)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F30;

        [PageFX.AbcClassTrait(106)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F31;

        [PageFX.AbcClassTrait(107)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F32;

        [PageFX.AbcClassTrait(108)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F33;

        [PageFX.AbcClassTrait(109)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F34;

        [PageFX.AbcClassTrait(110)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_F35;

        [PageFX.AbcClassTrait(111)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_INSERT;

        [PageFX.AbcClassTrait(112)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_DELETE;

        [PageFX.AbcClassTrait(113)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_HOME;

        [PageFX.AbcClassTrait(114)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_BEGIN;

        [PageFX.AbcClassTrait(115)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_END;

        [PageFX.AbcClassTrait(116)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_PAGEUP;

        [PageFX.AbcClassTrait(117)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_PAGEDOWN;

        [PageFX.AbcClassTrait(118)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_PRINTSCREEN;

        [PageFX.AbcClassTrait(119)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_SCROLLLOCK;

        [PageFX.AbcClassTrait(120)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_PAUSE;

        [PageFX.AbcClassTrait(121)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_SYSREQ;

        [PageFX.AbcClassTrait(122)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_BREAK;

        [PageFX.AbcClassTrait(123)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_RESET;

        [PageFX.AbcClassTrait(124)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_STOP;

        [PageFX.AbcClassTrait(125)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_MENU;

        [PageFX.AbcClassTrait(126)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_USER;

        [PageFX.AbcClassTrait(127)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_SYSTEM;

        [PageFX.AbcClassTrait(128)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_PRINT;

        [PageFX.AbcClassTrait(129)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_CLEARLINE;

        [PageFX.AbcClassTrait(130)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_CLEARDISPLAY;

        [PageFX.AbcClassTrait(131)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_INSERTLINE;

        [PageFX.AbcClassTrait(132)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_DELETELINE;

        [PageFX.AbcClassTrait(133)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_INSERTCHAR;

        [PageFX.AbcClassTrait(134)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_DELETECHAR;

        [PageFX.AbcClassTrait(135)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_PREV;

        [PageFX.AbcClassTrait(136)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_NEXT;

        [PageFX.AbcClassTrait(137)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_SELECT;

        [PageFX.AbcClassTrait(138)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_EXECUTE;

        [PageFX.AbcClassTrait(139)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_UNDO;

        [PageFX.AbcClassTrait(140)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_REDO;

        [PageFX.AbcClassTrait(141)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_FIND;

        [PageFX.AbcClassTrait(142)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_HELP;

        [PageFX.AbcClassTrait(143)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String STRING_MODESWITCH;

        [PageFX.AbcClassTrait(144)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.Array CharCodeStrings;

        /// <summary>Constant associated with the key code value for the 0 key (48).</summary>
        [PageFX.AbcClassTrait(145)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMBER_0;

        /// <summary>Constant associated with the key code value for the 1 key (49).</summary>
        [PageFX.AbcClassTrait(146)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMBER_1;

        /// <summary>Constant associated with the key code value for the 2 key (50).</summary>
        [PageFX.AbcClassTrait(147)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMBER_2;

        /// <summary>Constant associated with the key code value for the 3 key (51).</summary>
        [PageFX.AbcClassTrait(148)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMBER_3;

        /// <summary>Constant associated with the key code value for the 4 key (52).</summary>
        [PageFX.AbcClassTrait(149)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMBER_4;

        /// <summary>Constant associated with the key code value for the 5 key (53).</summary>
        [PageFX.AbcClassTrait(150)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMBER_5;

        /// <summary>Constant associated with the key code value for the 6 key (54).</summary>
        [PageFX.AbcClassTrait(151)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMBER_6;

        /// <summary>Constant associated with the key code value for the 7 key (55).</summary>
        [PageFX.AbcClassTrait(152)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMBER_7;

        /// <summary>Constant associated with the key code value for the 8 key (56).</summary>
        [PageFX.AbcClassTrait(153)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMBER_8;

        /// <summary>Constant associated with the key code value for the 9 key (57).</summary>
        [PageFX.AbcClassTrait(154)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMBER_9;

        /// <summary>Constant associated with the key code value for the A key (65).</summary>
        [PageFX.AbcClassTrait(155)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint A;

        /// <summary>Constant associated with the key code value for the B key (66).</summary>
        [PageFX.AbcClassTrait(156)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint B;

        /// <summary>Constant associated with the key code value for the C key (67).</summary>
        [PageFX.AbcClassTrait(157)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint C;

        /// <summary>Constant associated with the key code value for the D key (68).</summary>
        [PageFX.AbcClassTrait(158)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint D;

        /// <summary>Constant associated with the key code value for the E key (69).</summary>
        [PageFX.AbcClassTrait(159)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint E;

        /// <summary>Constant associated with the key code value for the F key (70).</summary>
        [PageFX.AbcClassTrait(160)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint F;

        /// <summary>Constant associated with the key code value for the G key (71).</summary>
        [PageFX.AbcClassTrait(161)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint G;

        /// <summary>Constant associated with the key code value for the H key (72).</summary>
        [PageFX.AbcClassTrait(162)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint H;

        /// <summary>Constant associated with the key code value for the I key (73).</summary>
        [PageFX.AbcClassTrait(163)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint I;

        /// <summary>Constant associated with the key code value for the J key (74).</summary>
        [PageFX.AbcClassTrait(164)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint J;

        /// <summary>Constant associated with the key code value for the K key (75).</summary>
        [PageFX.AbcClassTrait(165)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint K;

        /// <summary>Constant associated with the key code value for the L key (76).</summary>
        [PageFX.AbcClassTrait(166)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint L;

        /// <summary>Constant associated with the key code value for the M key (77).</summary>
        [PageFX.AbcClassTrait(167)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint M;

        /// <summary>Constant associated with the key code value for the N key (78).</summary>
        [PageFX.AbcClassTrait(168)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint N;

        /// <summary>Constant associated with the key code value for the O key (79).</summary>
        [PageFX.AbcClassTrait(169)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint O;

        /// <summary>Constant associated with the key code value for the P key (80).</summary>
        [PageFX.AbcClassTrait(170)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint P;

        /// <summary>Constant associated with the key code value for the Q key (81).</summary>
        [PageFX.AbcClassTrait(171)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint Q;

        /// <summary>Constant associated with the key code value for the R key (82).</summary>
        [PageFX.AbcClassTrait(172)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint R;

        /// <summary>Constant associated with the key code value for the S key (83).</summary>
        [PageFX.AbcClassTrait(173)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint S;

        /// <summary>Constant associated with the key code value for the T key (84).</summary>
        [PageFX.AbcClassTrait(174)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint T;

        /// <summary>Constant associated with the key code value for the U key (85).</summary>
        [PageFX.AbcClassTrait(175)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint U;

        /// <summary>Constant associated with the key code value for the V key (86).</summary>
        [PageFX.AbcClassTrait(176)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint V;

        /// <summary>Constant associated with the key code value for the W key (87).</summary>
        [PageFX.AbcClassTrait(177)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint W;

        /// <summary>Constant associated with the key code value for the X key (88).</summary>
        [PageFX.AbcClassTrait(178)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint X;

        /// <summary>Constant associated with the key code value for the Y key (89).</summary>
        [PageFX.AbcClassTrait(179)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint Y;

        /// <summary>Constant associated with the key code value for the Z key (90).</summary>
        [PageFX.AbcClassTrait(180)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint Z;

        /// <summary>Constant associated with the key code value for the ; key (186).</summary>
        [PageFX.AbcClassTrait(181)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint SEMICOLON;

        /// <summary>Constant associated with the key code value for the = key (187).</summary>
        [PageFX.AbcClassTrait(182)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint EQUAL;

        /// <summary>Constant associated with the key code value for the , key (188).</summary>
        [PageFX.AbcClassTrait(183)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint COMMA;

        /// <summary>Constant associated with the key code value for the - key (189).</summary>
        [PageFX.AbcClassTrait(184)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint MINUS;

        /// <summary>Constant associated with the key code value for the . key (190).</summary>
        [PageFX.AbcClassTrait(185)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint PERIOD;

        /// <summary>Constant associated with the key code value for the / key (191).</summary>
        [PageFX.AbcClassTrait(186)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint SLASH;

        /// <summary>Constant associated with the key code value for the ` key (192).</summary>
        [PageFX.AbcClassTrait(187)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint BACKQUOTE;

        /// <summary>Constant associated with the key code value for the [ key (219).</summary>
        [PageFX.AbcClassTrait(188)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint LEFTBRACKET;

        /// <summary>Constant associated with the key code value for the \ key (220).</summary>
        [PageFX.AbcClassTrait(189)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint BACKSLASH;

        /// <summary>Constant associated with the key code value for the ] key (221).</summary>
        [PageFX.AbcClassTrait(190)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint RIGHTBRACKET;

        /// <summary>Constant associated with the key code value for the &apos; key (222).</summary>
        [PageFX.AbcClassTrait(191)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint QUOTE;

        /// <summary>Constant associated with the key code value for the Alternate (Option) key (18).</summary>
        [PageFX.AbcClassTrait(192)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint ALTERNATE;

        /// <summary>Constant associated with the key code value for the Backspace key (8).</summary>
        [PageFX.AbcClassTrait(193)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint BACKSPACE;

        /// <summary>Constant associated with the key code value for the Caps Lock key (20).</summary>
        [PageFX.AbcClassTrait(194)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint CAPS_LOCK;

        /// <summary>Constant associated with the Mac command key (17).  This constant is currently only used for setting menu key equivalents.</summary>
        [PageFX.AbcClassTrait(195)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint COMMAND;

        /// <summary>Constant associated with the key code value for the Control key (17).</summary>
        [PageFX.AbcClassTrait(196)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint CONTROL;

        /// <summary>Constant associated with the key code value for the Delete key (46).</summary>
        [PageFX.AbcClassTrait(197)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint DELETE;

        /// <summary>Constant associated with the key code value for the Down Arrow key (40).</summary>
        [PageFX.AbcClassTrait(198)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint DOWN;

        /// <summary>Constant associated with the key code value for the End key (35).</summary>
        [PageFX.AbcClassTrait(199)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint END;

        /// <summary>Constant associated with the key code value for the Enter key (13).</summary>
        [PageFX.AbcClassTrait(200)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint ENTER;

        /// <summary>Constant associated with the key code value for the Escape key (27).</summary>
        [PageFX.AbcClassTrait(201)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint ESCAPE;

        /// <summary>Constant associated with the key code value for the F1 key (112).</summary>
        [PageFX.AbcClassTrait(202)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F1;

        /// <summary>Constant associated with the key code value for the F2 key (113).</summary>
        [PageFX.AbcClassTrait(203)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F2;

        /// <summary>Constant associated with the key code value for the F3 key (114).</summary>
        [PageFX.AbcClassTrait(204)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F3;

        /// <summary>Constant associated with the key code value for the F4 key (115).</summary>
        [PageFX.AbcClassTrait(205)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F4;

        /// <summary>Constant associated with the key code value for the F5 key (116).</summary>
        [PageFX.AbcClassTrait(206)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F5;

        /// <summary>Constant associated with the key code value for the F6 key (117).</summary>
        [PageFX.AbcClassTrait(207)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F6;

        /// <summary>Constant associated with the key code value for the F7 key (118).</summary>
        [PageFX.AbcClassTrait(208)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F7;

        /// <summary>Constant associated with the key code value for the F8 key (119).</summary>
        [PageFX.AbcClassTrait(209)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F8;

        /// <summary>Constant associated with the key code value for the F9 key (120).</summary>
        [PageFX.AbcClassTrait(210)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F9;

        /// <summary>Constant associated with the key code value for the F10 key (121).</summary>
        [PageFX.AbcClassTrait(211)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F10;

        /// <summary>Constant associated with the key code value for the F11 key (122).</summary>
        [PageFX.AbcClassTrait(212)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F11;

        /// <summary>Constant associated with the key code value for the F12 key (123).</summary>
        [PageFX.AbcClassTrait(213)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F12;

        /// <summary>Constant associated with the key code value for the F13 key (124).</summary>
        [PageFX.AbcClassTrait(214)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F13;

        /// <summary>Constant associated with the key code value for the F14 key (125).</summary>
        [PageFX.AbcClassTrait(215)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F14;

        /// <summary>Constant associated with the key code value for the F15 key (126).</summary>
        [PageFX.AbcClassTrait(216)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint F15;

        /// <summary>Constant associated with the key code value for the Home key (36).</summary>
        [PageFX.AbcClassTrait(217)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint HOME;

        /// <summary>Constant associated with the key code value for the Insert key (45).</summary>
        [PageFX.AbcClassTrait(218)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint INSERT;

        /// <summary>Constant associated with the key code value for the Left Arrow key (37).</summary>
        [PageFX.AbcClassTrait(219)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint LEFT;

        /// <summary>
        /// Constant associated with the pseudo-key code for the the number pad (21).  Use to
        /// set numpad modifier on key equivalents
        /// </summary>
        [PageFX.AbcClassTrait(220)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NUMPAD;

        /// <summary>Constant associated with the key code value for the number 0 key on the number pad (96).</summary>
        [PageFX.AbcClassTrait(221)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_0;

        /// <summary>Constant associated with the key code value for the number 1 key on the number pad (97).</summary>
        [PageFX.AbcClassTrait(222)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_1;

        /// <summary>Constant associated with the key code value for the number 2 key on the number pad (98).</summary>
        [PageFX.AbcClassTrait(223)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_2;

        /// <summary>Constant associated with the key code value for the number 3 key on the number pad (99).</summary>
        [PageFX.AbcClassTrait(224)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_3;

        /// <summary>Constant associated with the key code value for the number 4 key on the number pad (100).</summary>
        [PageFX.AbcClassTrait(225)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_4;

        /// <summary>Constant associated with the key code value for the number 5 key on the number pad (101).</summary>
        [PageFX.AbcClassTrait(226)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_5;

        /// <summary>Constant associated with the key code value for the number 6 key on the number pad (102).</summary>
        [PageFX.AbcClassTrait(227)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_6;

        /// <summary>Constant associated with the key code value for the number 7 key on the number pad (103).</summary>
        [PageFX.AbcClassTrait(228)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_7;

        /// <summary>Constant associated with the key code value for the number 8 key on the number pad (104).</summary>
        [PageFX.AbcClassTrait(229)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_8;

        /// <summary>Constant associated with the key code value for the number 9 key on the number pad (105).</summary>
        [PageFX.AbcClassTrait(230)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_9;

        /// <summary>Constant associated with the key code value for the addition key on the number pad (107).</summary>
        [PageFX.AbcClassTrait(231)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_ADD;

        /// <summary>Constant associated with the key code value for the decimal key on the number pad (110).</summary>
        [PageFX.AbcClassTrait(232)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_DECIMAL;

        /// <summary>Constant associated with the key code value for the division key on the number pad (111).</summary>
        [PageFX.AbcClassTrait(233)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_DIVIDE;

        /// <summary>Constant associated with the key code value for the Enter key on the number pad (108).</summary>
        [PageFX.AbcClassTrait(234)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_ENTER;

        /// <summary>Constant associated with the key code value for the multiplication key on the number pad (106).</summary>
        [PageFX.AbcClassTrait(235)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_MULTIPLY;

        /// <summary>Constant associated with the key code value for the subtraction key on the number pad (109).</summary>
        [PageFX.AbcClassTrait(236)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMPAD_SUBTRACT;

        /// <summary>Constant associated with the key code value for the Page Down key (34).</summary>
        [PageFX.AbcClassTrait(237)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint PAGE_DOWN;

        /// <summary>Constant associated with the key code value for the Page Up key (33).</summary>
        [PageFX.AbcClassTrait(238)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint PAGE_UP;

        /// <summary>Constant associated with the key code value for the Right Arrow key (39).</summary>
        [PageFX.AbcClassTrait(239)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint RIGHT;

        /// <summary>Constant associated with the key code value for the Shift key (16).</summary>
        [PageFX.AbcClassTrait(240)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint SHIFT;

        /// <summary>Constant associated with the key code value for the Spacebar (32).</summary>
        [PageFX.AbcClassTrait(241)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint SPACE;

        /// <summary>Constant associated with the key code value for the Tab key (9).</summary>
        [PageFX.AbcClassTrait(242)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint TAB;

        /// <summary>Constant associated with the key code value for the Up Arrow key (38).</summary>
        [PageFX.AbcClassTrait(243)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint UP;

        [PageFX.AbcClassTrait(249)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint RED;

        [PageFX.AbcClassTrait(250)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint GREEN;

        [PageFX.AbcClassTrait(251)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint YELLOW;

        [PageFX.AbcClassTrait(252)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint BLUE;

        [PageFX.AbcClassTrait(253)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint CHANNEL_UP;

        [PageFX.AbcClassTrait(254)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint CHANNEL_DOWN;

        [PageFX.AbcClassTrait(255)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint RECORD;

        [PageFX.AbcClassTrait(256)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint PLAY;

        [PageFX.AbcClassTrait(257)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint PAUSE;

        [PageFX.AbcClassTrait(258)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint STOP;

        [PageFX.AbcClassTrait(259)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint FAST_FORWARD;

        [PageFX.AbcClassTrait(260)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint REWIND;

        [PageFX.AbcClassTrait(261)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint SKIP_FORWARD;

        [PageFX.AbcClassTrait(262)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint SKIP_BACKWARD;

        [PageFX.AbcClassTrait(263)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint NEXT;

        [PageFX.AbcClassTrait(264)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint PREVIOUS;

        [PageFX.AbcClassTrait(265)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint LIVE;

        [PageFX.AbcClassTrait(266)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint LAST;

        [PageFX.AbcClassTrait(267)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint MENU;

        [PageFX.AbcClassTrait(268)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint INFO;

        [PageFX.AbcClassTrait(269)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint GUIDE;

        [PageFX.AbcClassTrait(270)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint EXIT;

        [PageFX.AbcClassTrait(271)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint BACK;

        [PageFX.AbcClassTrait(272)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint AUDIO;

        [PageFX.AbcClassTrait(273)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint SUBTITLE;

        [PageFX.AbcClassTrait(274)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint DVR;

        [PageFX.AbcClassTrait(275)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint VOD;

        [PageFX.AbcClassTrait(276)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint INPUT;

        [PageFX.AbcClassTrait(277)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint SETUP;

        [PageFX.AbcClassTrait(278)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint HELP;

        [PageFX.AbcClassTrait(279)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint MASTER_SHELL;

        [PageFX.AbcClassTrait(280)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint SEARCH;

        /// <summary>Specifies whether the Caps Lock key is activated (true) or not (false).</summary>
        public extern static bool capsLock
        {
            [PageFX.AbcClassTrait(244)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>Specifies whether the Num Lock key is activated (true) or not (false).</summary>
        public extern static bool numLock
        {
            [PageFX.AbcClassTrait(245)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool hasVirtualKeyboard
        {
            [PageFX.AbcClassTrait(247)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static Avm.String physicalKeyboardType
        {
            [PageFX.AbcClassTrait(248)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
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
        [PageFX.AbcClassTrait(246)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isAccessible();


    }
}
