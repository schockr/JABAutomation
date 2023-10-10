using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public interface ICoordinates
    {
        Point LocationOnScreen { get; }
        Point LocationInViewport { get; }
    }
}
