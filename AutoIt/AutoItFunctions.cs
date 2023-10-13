using AutoIt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JABAutomation.AutoIt
{
    public class AutoItFunctions
    {

        public static void SendTextByCoordinates(string text, int x, int y)
        {
            // Clear text
            Clear(x, y);

            // Send text
            AutoItX.Send(text + Keys.TAB);

        }

        public static void Clear(int x, int y)
        {
            // Click by coordinates instead
            AutoItX.MouseClick("LEFT", x, y);
            
            // select and delete all text
            AutoItX.Send(Keys.SELECT_ALL_TEXT + Keys.BACKSPACE);
        }

        public static void DoubleClick(int x, int y)
        {
            try
            {
                AutoItX.MouseClick("LEFT", x, y, 2);
            }
            catch (Exception e)
            {
                throw new ElementNotInteractableException("Element could not be clicked.", e);
            }
        }

        public static void ClickAtCoordinates(int x, int y)
        {
            try
            {
                AutoItX.MouseClick("LEFT", x, y);
            }
            catch (Exception e)
            {
                throw new ElementNotInteractableException("Element could not be clicked.", e);
            }
        }


    }
}
