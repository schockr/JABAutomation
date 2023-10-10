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
        Point Location { get; }
        Size Size { get; }
        void Click();
        void DoubleClick();
        void SendKeys(string text);
        void Clear();
    }
}
