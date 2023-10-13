using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public class LabelTextElementFinder : IFindElementStrategy
    {
        private bool _partialFinder { get; set; } = false;
        public LabelTextElementFinder(bool partialFinder)
        {
            this._partialFinder = partialFinder;
        }

        public LabelTextElementFinder() { }

        public TreeNode FindElement(TreeNode root, string labelText)
        {
            TreeNode node;
            if (_partialFinder)
            {
                node = TreeUtils.Flatten(root).FirstOrDefault(item =>
                    StringUtils.ContainsIgnoreCase(item.GetAccessibleTextSentence(), labelText));
            }
            else
            {
                node = TreeUtils.Flatten(root).FirstOrDefault(item =>
                    StringUtils.EqualsIgnoreCase(item.GetAccessibleTextSentence(), labelText));
            }

            if (node == null)
                throw new ElementNotFoundException("Element with label text " + labelText + " could not be found.");

            return node;
        }

        public ReadOnlyCollection<TreeNode> FindElements(TreeNode root, string labelText)
        {
            IEnumerable<TreeNode> nodes;
            if (_partialFinder)
            {
                nodes = TreeUtils.Flatten(root).Where(item =>
                    StringUtils.ContainsIgnoreCase(item.GetAccessibleTextSentence(), labelText));
            }
            else
            {
                nodes = TreeUtils.Flatten(root).Where(item =>
                    StringUtils.EqualsIgnoreCase(item.GetAccessibleTextSentence(), labelText));
            }

            if (nodes == null || !nodes.Any())
                throw new ElementNotFoundException("Element with label text " + labelText + " could not be found.");

            return nodes.ToList().AsReadOnly();

        }

    }
}
