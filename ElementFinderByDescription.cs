using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public class ElementFinderByDescription : IFindElementStrategy
    {
        private string _findTypeFilter { get; set; }

        public ElementFinderByDescription() { }

        public ElementFinderByDescription(ElementType findTypeFilter)
        {
            this._findTypeFilter = findTypeFilter.Value;
        }

        public TreeNode FindElement(TreeNode root, string desc)
        {
            TreeNode node;
            if (!string.IsNullOrEmpty(_findTypeFilter))
            {
                node = FindElementWithFilter(root, desc);
            }
            else
            {
                node = FindElementNoFilter(root, desc);
            }

            if (node == null)
                throw new ElementNotFoundException("Element with desc " + desc + " could not be found.");

            return node;
        }

        public ReadOnlyCollection<TreeNode> FindElements(TreeNode root, string desc)
        {
            IEnumerable<TreeNode> nodes;
            if (!string.IsNullOrEmpty(_findTypeFilter))
            {
                nodes = FindElementsWithFilter(root, desc);
            }
            else
            {
                nodes = FindElementsNoFilter(root, desc);
            }

            if (nodes == null || !nodes.Any())
                throw new ElementNotFoundException("Element with desc " + desc + " could not be found.");

            return nodes.ToList().AsReadOnly();
        }

        private TreeNode FindElementNoFilter(TreeNode root, string desc)
        {
            TreeNode node = TreeUtils.Flatten(root).FirstOrDefault(item => StringUtils.EqualsIgnoreCase(item.AccessibleContextInfo.description, desc));
            return node;
        }

        private TreeNode FindElementWithFilter(TreeNode root, string desc)
        {
            TreeNode node = TreeUtils.Flatten(root).FirstOrDefault(item => item.AccessibleContextInfo.role == this._findTypeFilter
                  && StringUtils.EqualsIgnoreCase(item.AccessibleContextInfo.description, desc));
            return node;
        }
        private IEnumerable<TreeNode> FindElementsNoFilter(TreeNode root, string desc)
        {
            IEnumerable<TreeNode> nodes = TreeUtils.Flatten(root).Where(item => StringUtils.EqualsIgnoreCase(item.AccessibleContextInfo.description, desc));
            return nodes;
        }

        private IEnumerable<TreeNode> FindElementsWithFilter(TreeNode root, string desc)
        {
            IEnumerable<TreeNode> nodes = TreeUtils.Flatten(root).Where(item => item.AccessibleContextInfo.role == this._findTypeFilter
                && StringUtils.EqualsIgnoreCase(item.AccessibleContextInfo.description, desc));
            return nodes;
        }


    }

}
