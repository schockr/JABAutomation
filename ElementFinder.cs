using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JABAutomation
{
    public class ElementFinder : IFindsElement
    {
        private TreeNode _root;
        private IFindElementStrategy _strategy;

        public ElementFinder(TreeNode root, IFindElementStrategy strategy)
        {
            this._root = root;
            this._strategy = strategy;
        }

        public TreeNode FindElementByPath(string path)
        {
            if (String.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path), "path cannot be null");

            return _strategy.FindElement(_root, path);
        }
        public ReadOnlyCollection<TreeNode> FindElementsByPath(string path)
        {
            if (String.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path), "path cannot be null");

            return _strategy.FindElements(_root, path);
        }

        public TreeNode FindElementByPushButton(string buttonName)
        {
            if (String.IsNullOrEmpty(buttonName)) throw new ArgumentNullException(nameof(buttonName), "button name cannot be null");

            return _strategy.FindElement(_root, buttonName);

        }
        public ReadOnlyCollection<TreeNode> FindElementsByPushButton(string buttonName)
        {
            if (String.IsNullOrEmpty(buttonName)) throw new ArgumentNullException(nameof(buttonName), "button name cannot be null");

            return _strategy.FindElements(_root, buttonName);
        }
        
        public TreeNode FindElementByLabelText(string labelText)
        {
            if (String.IsNullOrEmpty(labelText)) throw new ArgumentNullException(nameof(labelText), "label text cannot be null");

            return _strategy.FindElement(_root, labelText);
        }
        public ReadOnlyCollection<TreeNode> FindElementsByLabelText(string labelText)
        {
            if (String.IsNullOrEmpty(labelText)) throw new ArgumentNullException(nameof(labelText), "label text cannot be null");

            return _strategy.FindElements(_root, labelText);

        }
        
        public TreeNode FindElementByCombobox(string comboBox)
        {
            if (String.IsNullOrEmpty(comboBox)) throw new ArgumentNullException(nameof(comboBox), "combobox name cannot be null");

            return _strategy.FindElement(_root, comboBox);
        }
        public ReadOnlyCollection<TreeNode> FindElementsByCombobox(string comboBox)
        {
            if (String.IsNullOrEmpty(comboBox)) throw new ArgumentNullException(nameof(comboBox), "combobox cannot be null");

            return _strategy.FindElements(_root, comboBox);

        }

        public TreeNode FindElementByName(string elName)
        {
            if (String.IsNullOrEmpty(elName)) throw new ArgumentNullException(nameof(elName), "element name cannot be null");

            return _strategy.FindElement(_root, elName);
        }
        public ReadOnlyCollection<TreeNode> FindElementsByName(string elName)
        {
            if (String.IsNullOrEmpty(elName)) throw new ArgumentNullException(nameof(elName), "element name cannot be null");

            return _strategy.FindElements(_root, elName);

        }

        public TreeNode FindElementByDescription(string elDesc)
        {
            if (String.IsNullOrEmpty(elDesc)) throw new ArgumentNullException(nameof(elDesc), "element desc cannot be null");

            return _strategy.FindElement(_root, elDesc);
        }
        public ReadOnlyCollection<TreeNode> FindElementsByDescription(string elDesc)
        {
            if (String.IsNullOrEmpty(elDesc)) throw new ArgumentNullException(nameof(elDesc), "element desc cannot be null");

            return _strategy.FindElements(_root, elDesc);

        }

        public TreeNode FindElementByTextboxName(string txtName)
        {
            if (String.IsNullOrEmpty(txtName)) throw new ArgumentNullException(nameof(txtName), "textbox name cannot be null");

            return _strategy.FindElement(_root, txtName);
        }
        public ReadOnlyCollection<TreeNode> FindElementsByTextboxName(string txtName)
        {
            if (String.IsNullOrEmpty(txtName)) throw new ArgumentNullException(nameof(txtName), "textbox name cannot be null");

            return _strategy.FindElements(_root, txtName);

        }
        public TreeNode FindElementByLabelName(string lblName)
        {
            if (String.IsNullOrEmpty(lblName)) throw new ArgumentNullException(nameof(lblName), "label name cannot be null");

            return _strategy.FindElement(_root, lblName);
        }
        public ReadOnlyCollection<TreeNode> FindElementsByLabelName(string lblName)
        {
            if (String.IsNullOrEmpty(lblName)) throw new ArgumentNullException(nameof(lblName), "label name cannot be null");

            return _strategy.FindElements(_root, lblName);

        }

        public TreeNode FindElementByRadioButton(string radioBtnName)
        {
            if (String.IsNullOrEmpty(radioBtnName)) throw new ArgumentNullException(nameof(radioBtnName), "radio button name cannot be null");

            return _strategy.FindElement(_root, radioBtnName);
        }
        public ReadOnlyCollection<TreeNode> FindElementsByRadioButton(string radioBtnName)
        {
            if (String.IsNullOrEmpty(radioBtnName)) throw new ArgumentNullException(nameof(radioBtnName), "radio button name cannot be null");

            return _strategy.FindElements(_root, radioBtnName);

        }


        public TreeNode FindElementByToggleButton(string btnName)
        {
            if (String.IsNullOrEmpty(btnName)) throw new ArgumentNullException(nameof(btnName), "toggle button name cannot be null");

            return _strategy.FindElement(_root, btnName);
        }
        public ReadOnlyCollection<TreeNode> FindElementsByToggleButton(string btnName)
        {
            if (String.IsNullOrEmpty(btnName)) throw new ArgumentNullException(nameof(btnName), "toggle button name cannot be null");

            return _strategy.FindElements(_root, btnName);

        }

        public TreeNode FindElementByMenuItem(string itemName)
        {
            if (String.IsNullOrEmpty(itemName)) throw new ArgumentNullException(nameof(itemName), "menu item name cannot be null");

            return _strategy.FindElement(_root, itemName);
        }
        public ReadOnlyCollection<TreeNode> FindElementsByMenuItem(string itemName)
        {
            if (String.IsNullOrEmpty(itemName)) throw new ArgumentNullException(nameof(itemName), "menu item cannot be null");

            return _strategy.FindElements(_root, itemName);

        }
    }
}
