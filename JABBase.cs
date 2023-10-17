using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using WindowsAccessBridgeInterop;

namespace JABAutomation
{
    public class JABBase : IDisposable, IJavaDriver, ISearchContext
    {
        private readonly AccessBridge _accessBridge;
        private readonly AccessBridgeFunctions _accessBridgeFunctions;
        private int _jvmId;
        private JavaObjectHandle _rootJoHandle;
        private AccessibleContextInfo _accessibleContextInfo;
        private TreeNode _jvmNodeTree;
        private bool _disposed;

        private int _waitTimeMs = 500;
        private int _maxFindAttempts = 60;

        public string Name
        {
            get
            {
                return _accessibleContextInfo.name;
            }
        }

        public AccessBridgeFunctions AccessBridgeFunctions
        {
            get
            {
                return _accessBridgeFunctions;
            }
        }

        public int JvmId
        {
            get
            {
                return this._jvmId;
            }
        }

        public TreeNode JvmNodeTree
        {
            get
            {
                if (_jvmNodeTree == null)
                    return BuildTreeFromRoot();

                return _jvmNodeTree; 
            }
            set { _jvmNodeTree = value; }
        }

        public JABBase(AccessBridge accessBridge, IntPtr hWnd)
        {
            this._accessBridge = accessBridge;
            this._accessBridgeFunctions = accessBridge.Functions;
            SetJvmId(hWnd);
            _accessBridgeFunctions.GetAccessibleContextFromHWND(hWnd, out int jvmId, out JavaObjectHandle ac);
            this._jvmId = jvmId;
            this._rootJoHandle = ac;
            _accessBridgeFunctions.GetAccessibleContextInfo(_jvmId, _rootJoHandle, out this._accessibleContextInfo);

        }

        private void SetJvmId(IntPtr hWnd)
        {
            if (_accessBridgeFunctions.GetAccessibleContextFromHWND(hWnd, out int jvmId, out JavaObjectHandle ac))
            {
                this._jvmId = jvmId;
                this._rootJoHandle = ac;
                return;
            }
            throw new WindowNotFoundException("No java window was found for the window handle: "+hWnd);
        }


        public ReadOnlyCollection<IntPtr> WindowHandles
        {
            get
            {
                WindowHandle winHandle = new WindowHandle();
                return winHandle.GetWindowHandles().AsReadOnly();
            }
        }

        public IJavaElement FindElement(By by)
        {
            if (by == null)
            {
                throw new ArgumentNullException(nameof(@by), "by cannot be null");
            }

            TreeNode el = null;
            //TODO: Long chain of if statements is a code smell. Break this up later & use a factory
            if (by.Mechanism == SelectorMechanisms.XPath)
                el = FindElementByXPath(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.PushButtonName)
                el = FindElementByPushButton(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.LabelText)
                el = FindElementByLabelText(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.PartialLabelText)
                el = FindElementByPartialLabelText(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.ComboboxName)
                el = FindElementByCombobox(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.Name)
                el = FindElementByName(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.NameAndRole)
                el = FindElementByNameAndRole(by.Criteria, by.Role);
            else if (by.Mechanism == SelectorMechanisms.Description)
                el = FindElementByDescription(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.DescriptionAndRole)
                el = FindElementByDescAndRole(by.Criteria, by.Role);
            else if (by.Mechanism == SelectorMechanisms.TextboxName)
                el = FindElementByTextboxName(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.LabelName)
                el = FindElementByLabelName(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.RadioButtonName)
                el = FindElementByRadioButtonName(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.ToggleButtonName)
                el = FindElementByToggleButtonName(by.Criteria);
            else
                el = null;


            if (el != null)
            {
                IJavaElement javaEl = new JavaElement(this, el);
                return javaEl;
            }

     
            throw new ElementNotFoundException("Element not found at defined mechanism and criteria: " + by.Mechanism + ", " + by.Criteria);

        }

        public IJavaElement FindElementWait(By by)
        {
            if (by == null)
            {
                throw new ArgumentNullException(nameof(@by), "by cannot be null");
            }

            int maxAttempts = _maxFindAttempts;

            for (int attempts = 1; attempts <= maxAttempts; attempts++)
            {
                try
                {
                    RefreshTree();
                    IJavaElement el = FindElement(by);

                    return el;
                }
                catch (Exception e)
                {
                    Thread.Sleep(_waitTimeMs); // Wait before attempting again
                }
            }
            throw new ElementNotFoundException("The element (criteria: " + by.Mechanism + ", " + by.Criteria + ") could not be found after " + maxAttempts + " attempts.");

        }

        public ReadOnlyCollection<IJavaElement> FindElements(By by)
        {
            if (by == null)
            {
                throw new ArgumentNullException(nameof(@by), "by cannot be null");
            }

            ReadOnlyCollection<TreeNode> els = null;

            //TODO: Long chain of if statements is a code smell. Break this up later & use a factory
            if (by.Mechanism == SelectorMechanisms.XPath)
                els = FindElementsByXPath(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.PushButtonName)
                els = FindElementsByPushButton(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.LabelText)
                els = FindElementsByLabelText(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.PartialLabelText)
                els = FindElementsByPartialLabelText(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.ComboboxName)
                els = FindElementsByCombobox(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.Name)
                els = FindElementsByName(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.NameAndRole)
                els = FindElementsByNameAndRole(by.Criteria, by.Role);
            else if (by.Mechanism == SelectorMechanisms.Description)
                els = FindElementsByDescription(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.DescriptionAndRole)
                els = FindElementsByDescAndRole(by.Criteria, by.Role);
            else if (by.Mechanism == SelectorMechanisms.TextboxName)
                els = FindElementsByTextboxName(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.LabelName)
                els = FindElementsByLabelName(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.RadioButtonName)
                els = FindElementsByRadioButtonName(by.Criteria);
            else if (by.Mechanism == SelectorMechanisms.ToggleButtonName)
                els = FindElementsByToggleButtonName(by.Criteria);
            else
                els = null;


            if (els != null)
            {
                return ConvertTreeNodesToJavaElements(els);
            }
            throw new ElementNotFoundException("Element not found at defined mechanism and criteria: " + by.Mechanism + ", " + by.Criteria);
        }
        
        private TreeNode FindElementByXPath(string path)
        {
            IFindElementStrategy strategy = new PathElementFinder();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByPath(path);
        }

        private ReadOnlyCollection<TreeNode> FindElementsByXPath(string path)
        {
            IFindElementStrategy strategy = new PathElementFinder();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);
            return elementFinder.FindElementsByPath(path);
        }

        private TreeNode FindElementByPushButton(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.PUSH_BUTTON);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByPushButton(criteria);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByPushButton(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.PUSH_BUTTON);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByPushButton(criteria);

        }

        private TreeNode FindElementByLabelText(string criteria)
        {
            IFindElementStrategy strategy = new LabelTextElementFinder();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByLabelText(criteria);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByLabelText(string criteria)
        {
            IFindElementStrategy strategy = new LabelTextElementFinder();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByLabelText(criteria);
        }

        private TreeNode FindElementByPartialLabelText(string criteria)
        {
            IFindElementStrategy strategy = new LabelTextElementFinder(true);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByLabelText(criteria);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByPartialLabelText(string criteria)
        {
            IFindElementStrategy strategy = new LabelTextElementFinder(true);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByLabelText(criteria);
        }

        private TreeNode FindElementByCombobox(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.COMBO_BOX);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByCombobox(criteria);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByCombobox(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.COMBO_BOX);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByCombobox(criteria);

        }

        private TreeNode FindElementByName(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByName(criteria);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByName(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByName(criteria);

        }

        private TreeNode FindElementByNameAndRole(string name, ElementType elType)
        {
            IFindElementStrategy strategy = new ElementFinderByName(elType);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByName(name);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByNameAndRole(string name, ElementType elType)
        {
            IFindElementStrategy strategy = new ElementFinderByName(elType);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByName(name);

        }

        private TreeNode FindElementByDescription(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByDescription();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByDescription(criteria);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByDescription(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByDescription();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByDescription(criteria);

        }
        private TreeNode FindElementByDescAndRole(string name, ElementType elType)
        {
            IFindElementStrategy strategy = new ElementFinderByDescription(elType);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByDescription(name);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByDescAndRole(string name, ElementType elType)
        {
            IFindElementStrategy strategy = new ElementFinderByDescription(elType);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByDescription(name);

        }
        private TreeNode FindElementByTextboxName(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.TEXT);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByName(criteria);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByTextboxName(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.TEXT);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByName(criteria);

        }

        private TreeNode FindElementByLabelName(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.LABEL);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByName(criteria);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByLabelName(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.LABEL);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByName(criteria);

        }
        private TreeNode FindElementByRadioButtonName(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.RADIO_BUTTON);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByName(criteria);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByRadioButtonName(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.RADIO_BUTTON);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByName(criteria);

        }
        private TreeNode FindElementByToggleButtonName(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.TOGGLE_BUTTON);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementByName(criteria);

        }

        private ReadOnlyCollection<TreeNode> FindElementsByToggleButtonName(string criteria)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.TOGGLE_BUTTON);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            return elementFinder.FindElementsByName(criteria);

        }

        /* public IJavaElement FindElementByPath(string path)
         {
             IFindElementStrategy strategy = new PathElementFinder();
             ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

             TreeNode el = elementFinder.FindElementByPath(path);

             if (el != null)
             {
                 IJavaElement javaEl = new JavaElement(this, el);
                 return javaEl;
             }
             throw new ElementNotFoundException("Element not found at path: "+ path);
         }

         public ReadOnlyCollection<IJavaElement> FindElementsByPath(string path)
         {
             IFindElementStrategy strategy = new PathElementFinder();
             ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

             ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByPath(path);

             if (els != null)
             {
                 return ConvertTreeNodesToJavaElements(els);
             }
             throw new ElementNotFoundException("Element not found at path: " + path);
         }
         
        public IJavaElement FindElementByPushButton(string buttonName)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.PUSH_BUTTON);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            TreeNode el = elementFinder.FindElementByPushButton(buttonName);

            if (el != null)
            {
                IJavaElement javaEl = new JavaElement(this, el);
                return javaEl;
            }
            throw new ElementNotFoundException("Element not found at button name: " + buttonName);
        }

        public ReadOnlyCollection<IJavaElement> FindElementsByPushButton(string buttonName)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.PUSH_BUTTON);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByPushButton(buttonName);

            if (els != null)
            {
                return ConvertTreeNodesToJavaElements(els);
            }
            throw new ElementNotFoundException("Element not found at button name: " + buttonName);
        }
        */
        /*public IJavaElement FindElementByLabelText(string labelText)
        {
            IFindElementStrategy strategy = new LabelTextElementFinder();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            TreeNode el = elementFinder.FindElementByLabelText(labelText);

            if (el != null)
            {
                IJavaElement javaEl = new JavaElement(this, el);
                return javaEl;
            }
            throw new ElementNotFoundException("Element not found with label text: " + labelText);
        }

        public ReadOnlyCollection<IJavaElement> FindElementsByLabelText(string labelText)
        {
            IFindElementStrategy strategy = new LabelTextElementFinder();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByLabelText(labelText);

            if (els != null)
            {
                return ConvertTreeNodesToJavaElements(els);
            }
            throw new ElementNotFoundException("Element not found with label text: " + labelText);
        }

        
        public IJavaElement FindElementByPartialLabelText(string labelText)
        {
            IFindElementStrategy strategy = new LabelTextElementFinder(true);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            TreeNode el = elementFinder.FindElementByLabelText(labelText);

            if (el != null)
            {
                IJavaElement javaEl = new JavaElement(this, el);
                return javaEl;
            }
            throw new ElementNotFoundException("Element not found with label text: " + labelText);
        }

        public ReadOnlyCollection<IJavaElement> FindElementsByPartialLabelText(string labelText)
        {
            IFindElementStrategy strategy = new LabelTextElementFinder(true);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByLabelText(labelText);

            if (els != null)
            {
                return ConvertTreeNodesToJavaElements(els);
            }
            throw new ElementNotFoundException("Element not found with label text: " + labelText);
        }

        public IJavaElement FindElementByCombobox(string combobox)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.COMBO_BOX);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            TreeNode el = elementFinder.FindElementByCombobox(combobox);

            if (el != null)
            {
                IJavaElement javaEl = new JavaElement(this, el);
                return javaEl;
            }
            throw new ElementNotFoundException("Element not found at combobox name: " + combobox);
        }

        public ReadOnlyCollection<IJavaElement> FindElementsByCombobox(string combobox)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.COMBO_BOX);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByCombobox(combobox);

            if (els != null)
            {
                return ConvertTreeNodesToJavaElements(els);
            }
            throw new ElementNotFoundException("Element not found at combobox name: " + combobox);
        }


        public IJavaElement FindElementByName(string elName)
        {
            IFindElementStrategy strategy = new ElementFinderByName();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            TreeNode el = elementFinder.FindElementByName(elName);

            if (el != null)
            {
                IJavaElement javaEl = new JavaElement(this, el);
                return javaEl;
            }
            throw new ElementNotFoundException("Element not found at element name: " + elName);
        }

        public ReadOnlyCollection<IJavaElement> FindElementsByName(string elName)
        {
            IFindElementStrategy strategy = new ElementFinderByName();
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByName(elName);

            if (els != null)
            {
                return ConvertTreeNodesToJavaElements(els);
            }
            throw new ElementNotFoundException("Element not found at element name: " + elName);
        }

        public IJavaElement FindElementByName(string elName, ElementType elType)
        {
            IFindElementStrategy strategy = new ElementFinderByName(elType);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            TreeNode el = elementFinder.FindElementByName(elName);

            if (el != null)
            {
                IJavaElement javaEl = new JavaElement(this, el);
                return javaEl;
            }
            throw new ElementNotFoundException("Element not found at element name: " + elName);
        }

        public ReadOnlyCollection<IJavaElement> FindElementsByName(string elName, ElementType elType)
        {
            IFindElementStrategy strategy = new ElementFinderByName(elType);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByName(elName);

            if (els != null)
            {
                return ConvertTreeNodesToJavaElements(els);
            }
            throw new ElementNotFoundException("Element not found at element name: " + elName);
        }
        */
        /* public IJavaElement FindElementByDesc(string elDesc)
         {
             IFindElementStrategy strategy = new ElementFinderByDescription();
             ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

             TreeNode el = elementFinder.FindElementByDescription(elDesc);

             if (el != null)
             {
                 IJavaElement javaEl = new JavaElement(this, el);
                 return javaEl;
             }
             throw new ElementNotFoundException("Element not found at element desc: " + elDesc);
         }

         public ReadOnlyCollection<IJavaElement> FindElementsByDesc(string elDesc)
         {
             IFindElementStrategy strategy = new ElementFinderByDescription();
             ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

             ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByDescription(elDesc);

             if (els != null)
             {
                 return ConvertTreeNodesToJavaElements(els);
             }
             throw new ElementNotFoundException("Element not found at element desc: " + elDesc);
         }

        public IJavaElement FindElementByDesc(string elDesc, ElementType elType)
        {
            IFindElementStrategy strategy = new ElementFinderByDescription(elType);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            TreeNode el = elementFinder.FindElementByDescription(elDesc);

            if (el != null)
            {
                IJavaElement javaEl = new JavaElement(this, el);
                return javaEl;
            }
            throw new ElementNotFoundException("Element not found at element desc: " + elDesc);
        }

        public ReadOnlyCollection<IJavaElement> FindElementsByDesc(string elDesc, ElementType elType)
        {
            IFindElementStrategy strategy = new ElementFinderByDescription(elType);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByDescription(elDesc);

            if (els != null)
            {
                return ConvertTreeNodesToJavaElements(els);
            }
            throw new ElementNotFoundException("Element not found at element desc: " + elDesc);
        }

        public IJavaElement FindElementByTextboxName(string elName)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.TEXT);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            TreeNode el = elementFinder.FindElementByName(elName);

            if (el != null)
            {
                IJavaElement javaEl = new JavaElement(this, el);
                return javaEl;
            }
            throw new ElementNotFoundException("Element not found at textbox name: " + elName);
        }

        public ReadOnlyCollection<IJavaElement> FindElementsByTextboxName(string elName)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.TEXT);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByName(elName);

            if (els != null)
            {
                return ConvertTreeNodesToJavaElements(els);
            }
            throw new ElementNotFoundException("Element not found at textbox name: " + elName);
        }

        public IJavaElement FindElementByLabelName(string elName)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.LABEL);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            TreeNode el = elementFinder.FindElementByName(elName);

            if (el != null)
            {
                IJavaElement javaEl = new JavaElement(this, el);
                return javaEl;
            }
            throw new ElementNotFoundException("Element not found at textbox name: " + elName);
        }

        public ReadOnlyCollection<IJavaElement> FindElementsByLabelName(string elName)
        {
            IFindElementStrategy strategy = new ElementFinderByName(ElementType.LABEL);
            ElementFinder elementFinder = new ElementFinder(JvmNodeTree, strategy);

            ReadOnlyCollection<TreeNode> els = elementFinder.FindElementsByName(elName);

            if (els != null)
            {
                return ConvertTreeNodesToJavaElements(els);
            }
            throw new ElementNotFoundException("Element not found at textbox name: " + elName);
        }*/

        private ReadOnlyCollection<IJavaElement> ConvertTreeNodesToJavaElements(ReadOnlyCollection<TreeNode> els)
        {
            IList<IJavaElement> javaEls = new List<IJavaElement>();

            foreach (TreeNode el in els)
                javaEls.Add(new JavaElement(this, el));

            return new ReadOnlyCollection<IJavaElement>(javaEls);
        }


        public TreeNode BuildTreeFromRoot()
        {
            return TreeBuilder.BuildTree(_accessibleContextInfo, 0, _accessBridge, _rootJoHandle);
        }

        public TreeNode RefreshTree()
        {
            JvmNodeTree = BuildTreeFromRoot();
            return JvmNodeTree;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            // DisposeTreeNodeList(_view.AccessibilityTree.Nodes);
            _accessBridge.Dispose();
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
