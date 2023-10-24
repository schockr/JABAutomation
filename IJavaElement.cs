using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public interface IJavaElement
    {
        JavaObjectHandle JavaObjectHandle { get; }
        Point Location { get; }
        Size Size { get; }
        TreeNode Element { get; }
        string Role { get; }
        ICoordinates Coordinates { get; }
        JavaTable TableInfo { get; }
        void Click();
        void DoubleClick();
        void SendKeys(string text);
        void Clear();
    }
}
