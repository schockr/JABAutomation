using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation.AutoIt
{
    /// <summary>
    /// Representations of keys able to be pressed that are not text keys for sending to the app or browser.
    /// </summary>
    public static class Keys
    {
        /// <summary>
        /// Represents the Enter keystroke.
        /// </summary>
        public static readonly string ENTER = "{ENTER}";

        /// <summary>
        /// Represents the Exclamation point keystroke.
        /// </summary>
        public static readonly string EXCLAMATION = "{!}";

        /// <summary>
        /// Represents the Hashtag (pound) key.
        /// </summary>
        public static readonly string HASHTAG = "{#}";

        /// <summary>
        /// Represents the Pound (hashtag) key.
        /// Both HASHTAG and POUND supported to accommodate generational differences :)
        /// </summary>
        public static readonly string POUND = "{#}";

        /// <summary>
        /// Represents the Plus key.
        /// </summary>
        public static readonly string PLUS = "{+}";

        public static readonly string CARET_UP = "{^}";
        public static readonly string SELECT_ALL_TEXT = "^a";

        public static readonly string START_CURLY_BRACKET = "{{}";
        public static readonly string END_CURLY_BRACKET = "{}}";
        public static readonly string SPACE = "{SPACE}";
        public static readonly string ALT = "{ALT}";
        public static readonly string BACKSPACE = "{BACKSPACE}";
        public static readonly string DELETE = "{DELETE}";
        public static readonly string UP = "{UP}";
        public static readonly string DOWN = "{DOWN}";
        public static readonly string LEFT = "{LEFT}";
        public static readonly string RIGHT = "{RIGHT}";
        public static readonly string HOME = "{HOME}";
        public static readonly string END = "{END}";
        public static readonly string ESCAPE = "{ESCAPE}";
        public static readonly string INSERT = "{INSERT}";
        public static readonly string PGUP = "{PGUP}";
        public static readonly string PGDN = "{PGDN}";
        public static readonly string F1 = "{F1}";
        public static readonly string F2 = "{F2}";
        public static readonly string F3 = "{F3}";
        public static readonly string F4 = "{F4}";
        public static readonly string F5 = "{F5}";
        public static readonly string F6 = "{F6}";
        public static readonly string F7 = "{F7}";
        public static readonly string F8 = "{F8}";
        public static readonly string F9 = "{F9}";
        public static readonly string F10 = "{F10}";
        public static readonly string F11 = "{F11}";
        public static readonly string F12 = "{F12}";
        public static readonly string TAB = "{TAB}";
        public static readonly string PRINTSCREEN = "{PRINTSCREEN}";
        public static readonly string LWIN = "{LWIN}";
        public static readonly string RWIN = "{RWIN}";
        public static readonly string NUMLOCK = "{NUMLOCK}";
        public static readonly string CAPSLOCK = "{CAPSLOCK}";
        public static readonly string SCROLLLOCK = "{SCROLLLOCK}";
        public static readonly string BREAK = "{BREAK}";
        public static readonly string PAUSE = "{PAUSE}";
        public static readonly string NUMPAD0 = "{NUMPAD0}";
        public static readonly string NUMPAD1 = "{NUMPAD1}";
        public static readonly string NUMPAD2 = "{NUMPAD2}";
        public static readonly string NUMPAD3 = "{NUMPAD3}";
        public static readonly string NUMPAD4 = "{NUMPAD4}";
        public static readonly string NUMPAD5 = "{NUMPAD5}";
        public static readonly string NUMPAD6 = "{NUMPAD6}";
        public static readonly string NUMPAD7 = "{NUMPAD7}";
        public static readonly string NUMPAD8 = "{NUMPAD8}";
        public static readonly string NUMPAD9 = "{NUMPAD9}";
        public static readonly string NUMPADMULT = "{NUMPADMULT}";
        public static readonly string NUMPADADD = "{NUMPADADD}";
        public static readonly string NUMPADSUB = "{NUMPADSUB}";
        public static readonly string NUMPADDIV = "{NUMPADDIV}";
        public static readonly string NUMPADDOT = "{NUMPADDOT}";
        public static readonly string NUMPADENTER = "{NUMPADENTER}";
        public static readonly string APPSKEY = "{APPSKEY}";
        public static readonly string LALT = "{LALT}";
        public static readonly string RALT = "{RALT}";
        public static readonly string LCTRL = "{LCTRL}";
        public static readonly string RCTRL = "{RCTRL}";
        public static readonly string LSHIFT = "{LSHIFT}";
        public static readonly string RSHIFT = "{RSHIFT}";
        public static readonly string SLEEP = "{SLEEP}";
        public static readonly string ALTDOWN = "{ALTDOWN}";
        public static readonly string SHIFTDOWN = "{SHIFTDOWN}";
        public static readonly string CTRLDOWN = "{CTRLDOWN}";
        public static readonly string LWINDOWN = "{LWINDOWN}";
        public static readonly string RWINDOWN = "{RWINDOWN}";
        public static readonly string ASC_NNNN = "{ASC nnnn}";
        public static readonly string BROWSER_BACK = "{BROWSER_BACK}";
        public static readonly string BROWSER_FORWARD = "{BROWSER_FORWARD}";
        public static readonly string BROWSER_REFRESH = "{BROWSER_REFRESH}";
        public static readonly string BROWSER_STOP = "{BROWSER_STOP}";
        public static readonly string BROWSER_SEARCH = "{BROWSER_SEARCH}";
        public static readonly string BROWSER_FAVORITES = "{BROWSER_FAVORITES}";
        public static readonly string BROWSER_HOME = "{BROWSER_HOME}";
        public static readonly string VOLUME_MUTE = "{VOLUME_MUTE}";
        public static readonly string VOLUME_DOWN = "{VOLUME_DOWN}";
        public static readonly string VOLUME_UP = "{VOLUME_UP}";
        public static readonly string MEDIA_NEXT = "{MEDIA_NEXT}";
        public static readonly string MEDIA_PREV = "{MEDIA_PREV}";
        public static readonly string MEDIA_STOP = "{MEDIA_STOP}";
        public static readonly string MEDIA_PLAY_PAUSE = "{MEDIA_PLAY_PAUSE}";
        public static readonly string LAUNCH_MAIL = "{LAUNCH_MAIL}";
        public static readonly string LAUNCH_MEDIA = "{LAUNCH_MEDIA}";
        public static readonly string LAUNCH_APP1 = "{LAUNCH_APP1}";
        public static readonly string LAUNCH_APP2 = "{LAUNCH_APP2}";
        public static readonly string OEM_102 = "{OEM_102}";

    }
}
