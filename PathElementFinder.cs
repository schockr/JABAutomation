using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace JABAutomation
{
    public class PathElementFinder : IFindElementStrategy
    {
        public TreeNode FindElement(TreeNode root, string path)
        {
            if (root == null || path == null)
                throw new ArgumentNullException("The treenode or path provided were null.");

            return FindElement(TreeToXMLConverter.CreateXmlDocumentFromTree(root), path, root);
        }

        public TreeNode FindElement(XDocument xdoc, string path, TreeNode root)
        {
           
            XElement? el = xdoc.XPathSelectElement(path, GetXmlNamespaceManager());

            if (el != null)
                return TreeToXMLConverter.GetTreeNodeFromXElement(el, root);
            else
            {
                throw new ElementNotFoundException("Element not found at path: " + path);
            }
        }

        private XmlNamespaceManager GetXmlNamespaceManager()
        {
            XmlNamespaceManager xnm = new XmlNamespaceManager(new NameTable());
            xnm.AddNamespace("x", "http://demo.com/2011/demo/schema");
            return xnm;
        }


        public ReadOnlyCollection<TreeNode> FindElements(TreeNode root, string path)
        {
            if (root == null || path == null)
                throw new ArgumentNullException("The root treenode or path provided were null.");

            return FindElements(TreeToXMLConverter.CreateXmlDocumentFromTree(root), path, root);
        }

        public ReadOnlyCollection<TreeNode> FindElements(XDocument xdoc, string path, TreeNode root)
        {
            IEnumerable<XElement>? els = xdoc.XPathSelectElements(path, GetXmlNamespaceManager());

            if (els != null)
                return TreeToXMLConverter.GetTreeNodesFromXElements(els, root);
            else
            {
                throw new ElementNotFoundException("Elements not found at path: " + path);
            }
        }

    }
}
