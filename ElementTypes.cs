using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public class ElementType
    {
        private ElementType(string value) { Value = value; }

        public string Value { get;private set; }

        public static ElementType LABEL { get { return new ElementType("label"); } }
        public static ElementType ROOT_PANE { get { return new ElementType("root pane"); } }
        public static ElementType LAYERED_PANE { get { return new ElementType("layered pane"); } }
        public static ElementType PANEL { get { return new ElementType("panel"); } }
        public static ElementType RADIO_BUTTON { get { return new ElementType("radio button"); } }
        public static ElementType TOGGLE_BUTTON { get { return new ElementType("toggle button"); } }
        public static ElementType PUSH_BUTTON { get { return new ElementType("push button"); } }
        public static ElementType TEXT { get { return new ElementType("text"); } }
        public static ElementType VIEWPORT { get { return new ElementType("viewport"); } }
        public static ElementType SCROLL_BAR { get { return new ElementType("scroll bar"); } }
        public static ElementType TREE { get { return new ElementType("tree"); } }
        public static ElementType SCROLL_PANE { get { return new ElementType("scroll pane"); } }
        public static ElementType COMBO_BOX { get { return new ElementType("combo box"); } }
        public static ElementType POPUP_MENU { get { return new ElementType("popup menu"); } }
        public static ElementType LIST { get { return new ElementType("list"); } }
        public static ElementType SPLIT_PANE { get { return new ElementType("split pane"); } }
        public static ElementType FRAME { get { return new ElementType("frame"); } }
        public static ElementType CHECK_BOX { get { return new ElementType("check box"); } }

        public override string ToString()
        {
            return Value;
        }
    }
}
