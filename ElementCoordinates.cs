using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public class ElementCoordinates : ICoordinates
    {
        private JavaElement _element;

        public ElementCoordinates(JavaElement element)
        {
            this._element = element;
        }

        public Point LocationOnScreen
        {
            get { return this._element.Location; }
        }
        public Point LocationInViewport
        {
            get { return this._element.Location; }
        }
    }
}
