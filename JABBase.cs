using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using WindowsAccessBridgeInterop;

namespace JABAutomation
{
    public class JABBase : IDisposable, IJavaDriver
    {
        private readonly AccessBridge _accessBridge;
        private readonly AccessBridgeFunctions _accessBridgeFunctions;
        private int _jvmId;
        private JavaObjectHandle _rootJoHandle;
        private AccessibleContextInfo _accessibleContextInfo;
        private TreeNode _jvmNodeTree;
        private bool _disposed;


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

        public IJavaElement FindElementByPath(string path)
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

        public IJavaElement FindElementByLabelText(string labelText)
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

        public IJavaElement FindElementByDesc(string elDesc)
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
        }

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
