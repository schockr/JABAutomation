using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using AutoIt;
using static System.Net.Mime.MediaTypeNames;
using JABAutomation.AutoIt;

namespace JABAutomation
{
    public class JavaElement : IJavaElement
    {
        private readonly JABBase jabDriver;
        private TreeNode element;
        private int jvmId;
        private JavaObjectHandle joh;
        private JavaTable tableInfo;

        public JavaElement(JABBase jabDriver, TreeNode element)
        {
            this.jabDriver = jabDriver;
            this.element = element;
            this.jvmId = element.JavaObjectHandle.JvmId;
            this.joh = element.JavaObjectHandle;
            this.tableInfo = new JavaTable(this);
        }
        
        public virtual JavaObjectHandle JavaObjectHandle
        {
            get { return joh; }
        }
        
        public virtual Point Location
        {
            get
            {
                int x = element.AccessibleContextInfo.x;
                int y = element.AccessibleContextInfo.y;
                return new Point(x, y);
            }
        }

        public virtual Size Size
        {
            get
            {
                int w = element.AccessibleContextInfo.width;
                int h = element.AccessibleContextInfo.height;
                return new Size(w, h);
            }
        }

        public virtual string Role
        {
            get
            {
                return element.AccessibleContextInfo.role;
            }
        }

        public virtual ICoordinates Coordinates
        {
            get { return new ElementCoordinates(this); }
        }

        public virtual TreeNode Element
        {
            get
            {
                return this.element;
            }
        }

        public virtual JavaTable TableInfo {
            get
            {                
                return this.tableInfo;
            }
        }


        public virtual void Click()
        {
            // Retrieve actions made interactable/accessible by jvm
            AccessibleActions actions = GetAccessibleActionsProperty();

            // Verify the click action is accessible
            if (actions.actionInfo.Where(a=>a.name == "click").Count() >= 1)
            {
                AccessibleActionsToDo actionsToDo = new AccessibleActionsToDo();
                actionsToDo.actions = new AccessibleActionInfo[actions.actionInfo.Length];
                actionsToDo.actions[0].name = "click";
                actionsToDo.actionsCount = 1;
                jabDriver.AccessBridgeFunctions.DoAccessibleActions(jabDriver.JvmId, element.JavaObjectHandle, ref actionsToDo, out int failure);
                return;
            }
            else
            {
                try
                {
                    AutoItFunctions.ClickAtCoordinates(Location.X, Location.Y);
                }
                catch (Exception e)
                {
                    // if this happens, user should handle the exception
                    throw new ElementNotInteractableException("No click action is available for this element: " + Role, e);

                }
            }

        }

        
        public virtual void DoubleClick()
        {
            // Double click unnecessary at jvm level. If needed, perform with autoit by coordinates
            AutoItFunctions.DoubleClick(Location.X, Location.Y);
        }

        public virtual AccessibleActions GetAccessibleActionsProperty()
        {
            AccessibleActions actions = new AccessibleActions();
            jabDriver.AccessBridgeFunctions.GetAccessibleActions(jabDriver.JvmId, element.JavaObjectHandle, out actions);
            return actions;

        }

        public virtual void SendKeys(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text), "text cannot be null");
            }

            int jvmid = this.element.JavaObjectHandle.JvmId;
            JavaObjectHandle el = this.element.JavaObjectHandle;

            if (!jabDriver.AccessBridgeFunctions.SetTextContents(jvmid, el, text))
            {
                try
                {
                    //TODO LOG that element not interactable via JAB
                 
                    AutoItFunctions.SendTextByCoordinates(text, this.element.AccessibleContextInfo.x, this.element.AccessibleContextInfo.y);
                    
                    // TODO verify that text updated (refresh tree & read element accessible text)

                }
                catch (Exception e)
                {
                    throw new ElementNotInteractableException("Text could not be entered for element.", e);
                }
            }
        }

        public virtual void Clear()
        {
            jabDriver.AccessBridgeFunctions.GetAccessibleActions(jvmId, joh, out AccessibleActions actions);

            AccessibleActionsToDo actionsToDo = new AccessibleActionsToDo();
            actionsToDo.actions = actions.actionInfo;
            actionsToDo.actions[0].name = "caret-begin";
            actionsToDo.actions[1].name = "select-all";
            actionsToDo.actions[2].name = "delete-next";
            actionsToDo.actionsCount = 3;

            jabDriver.AccessBridgeFunctions.DoAccessibleActions(jvmId, joh, ref actionsToDo, out int failure);

            if (failure >= 1)
            {
                // Clear with autoit via direct screen interaction
                AutoItFunctions.Clear(Location.X, Location.Y);
            }

            // TODO add verification that text is cleared by refreshing element and viewing Accessible Text


        }

        public virtual void ClickTableRowByCellName(string cellName, int colIndex = -1, int rowIndex = -1)
        {
            try
            {
                Point point = TableInfo.GetTableRowCoordinates(cellName, colIndex, rowIndex);
                if (point.X != -1 && point.Y !=  -1)
                {
                    AutoItFunctions.ClickAtCoordinates(point.X, point.Y);
                }
                else
                {
                    throw new ElementNotFoundException("The table cell with name could not be found: "+ cellName);
                }
            }
            catch (Exception e)
            {
                // if this happens, user should handle the exception
                throw new ElementNotInteractableException("No click action is available for this element: " + Role, e);
            }
        }

    }
}
