using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public class ElementFinderByName : IFindElementStrategy
    {
        private string? _findTypeFilter { get; set; }

        public ElementFinderByName() { }

        public ElementFinderByName(ElementType findTypeFilter)
        {
            this._findTypeFilter = findTypeFilter.Value;
        }

        public TreeNode FindElement(TreeNode root, string name)
        {
            TreeNode? node;
            if (!string.IsNullOrEmpty(_findTypeFilter))
            {
                node = FindElementWithFilter(root, name);
            }
            else
            {
                node = FindElementNoFilter(root, name);
            }

            if (node == null)
                throw new ElementNotFoundException("Element with name " + name + " could not be found.");

            return node;
        }

        public ReadOnlyCollection<TreeNode> FindElements(TreeNode root, string name)
        {
            IEnumerable<TreeNode>? nodes;
            if (!string.IsNullOrEmpty(_findTypeFilter))
            {
                nodes = FindElementsWithFilter(root, name);
            }
            else
            {
                nodes = FindElementsNoFilter(root, name);
            }

            if (nodes == null || !nodes.Any())
                throw new ElementNotFoundException("Element with name " + name + " could not be found.");

            return nodes.ToList().AsReadOnly();
        }

        private TreeNode? FindElementNoFilter(TreeNode root, string name)
        {
            TreeNode? node = TreeUtils.Flatten(root).FirstOrDefault(item => StringUtils.EqualsIgnoreCase(item.AccessibleContextInfo.name, name));
            return node;
        }

        private TreeNode? FindElementWithFilter(TreeNode root, string name)
        {
            TreeNode? node = TreeUtils.Flatten(root).FirstOrDefault(item => item.AccessibleContextInfo.role == this._findTypeFilter
                  && StringUtils.EqualsIgnoreCase(item.AccessibleContextInfo.name, name));
            return node;
        }
        private IEnumerable<TreeNode>? FindElementsNoFilter(TreeNode root, string name)
        {
            IEnumerable<TreeNode> nodes = TreeUtils.Flatten(root).Where(item => StringUtils.EqualsIgnoreCase(item.AccessibleContextInfo.name, name));
            return nodes;
        }

        private IEnumerable<TreeNode>? FindElementsWithFilter(TreeNode root, string name)
        {
            IEnumerable<TreeNode> nodes = TreeUtils.Flatten(root).Where(item => item.AccessibleContextInfo.role == this._findTypeFilter
                && StringUtils.EqualsIgnoreCase(item.AccessibleContextInfo.name, name));
            return nodes;
        }


    }

}
