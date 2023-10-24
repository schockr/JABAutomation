using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JABAutomation
{
    /// <summary>
    /// Utility to convert to and from TreeNode and XDocument type objects
    /// </summary>
    public class TreeToXMLConverter
    {
        /// <summary>
        /// Converts from a <see cref="TreeNode"/> object to an <see cref="XDocument"/> object.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static XDocument CreateXmlDocumentFromTree(TreeNode root)
        {
            XElement element = CreateXElement(root.AccessibleContextInfo.role, GetAttributes(root));
            element.Add(CreateXmlElement(root));
            XDocument xdoc = new XDocument(element);
            return xdoc;

        }

        public static TreeNode GetTreeNodeFromXElement(XElement element, TreeNode root)
        {
            if (element == null || root == null || element.Attribute("elementId") == null)
                throw new ElementNotFoundException("The element could not be found.");

            TreeNode treeNode = LocateTreeNodeByElementId(root, element);
            if (treeNode == null)
                throw new ElementNotFoundException("");

            return treeNode;
        }
        public static ReadOnlyCollection<TreeNode> GetTreeNodesFromXElements(IEnumerable<XElement> elements, TreeNode root)
        {
            if (elements == null || root == null)
                throw new ElementNotFoundException("The element could not be found.");


            IList<TreeNode> treeNodes = new List<TreeNode>();
            foreach (XElement element in elements)
            {
                TreeNode treeNode = LocateTreeNodeByElementId(root, element);
                if (treeNode != null)
                    treeNodes.Add(treeNode);
            }

            if (treeNodes == null)
                throw new ElementNotFoundException("No elements found.");

            return treeNodes.ToList().AsReadOnly();
        }


        private static TreeNode LocateTreeNodeByElementId(TreeNode root, XElement element)
        {
            return TreeUtils.Flatten(root).FirstOrDefault(node => node.ElementId == element.Attribute("elementId").Value);
        }

        private static List<XElement> CreateXmlElement(TreeNode treeViewNodes)
        {
            var document = new XDocument();
            var elements = new List<XElement>();
            foreach (TreeNode treeViewNode in treeViewNodes.Children)
            {
                XElement element = CreateXElement(treeViewNode.AccessibleContextInfo.role, GetAttributes(treeViewNode));

                if (treeViewNode.Children.Count() < 1)
                    element.Value = treeViewNode.AccessibleContextInfo.role;
                else
                    element.Add(CreateXmlElement(treeViewNode));
                elements.Add(element);
            }
            return elements;
        }

        /// <summary>
        /// Accesses TreeNode properties for storage in dictionary object.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Dictionary containing TreeNode attributes.</returns>
        private static Dictionary<string,string> GetAttributes(TreeNode node)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("name", node.AccessibleContextInfo.name);
            attributes.Add("description", node.AccessibleContextInfo.description);
            attributes.Add("role", node.AccessibleContextInfo.role);
            attributes.Add("role_en_US", node.AccessibleContextInfo.role_en_US);
            attributes.Add("x", node.AccessibleContextInfo.x.ToString());
            attributes.Add("y", node.AccessibleContextInfo.y.ToString());
            attributes.Add("indexInParent", node.AccessibleContextInfo.indexInParent.ToString());
            attributes.Add("childrenCount", node.AccessibleContextInfo.childrenCount.ToString());
            attributes.Add("width", node.AccessibleContextInfo.width.ToString());
            attributes.Add("height", node.AccessibleContextInfo.height.ToString());
            attributes.Add("accessibleComponent", node.AccessibleContextInfo.accessibleComponent.ToString());
            attributes.Add("accessibleAction", node.AccessibleContextInfo.accessibleAction.ToString());
            attributes.Add("accessibleText", node.AccessibleContextInfo.accessibleText.ToString());
            attributes.Add("states", node.AccessibleContextInfo.states);
            attributes.Add("states_en_US", node.AccessibleContextInfo.states_en_US);
            attributes.Add("depth", node.Depth.ToString());
            attributes.Add("handle", node.JavaObjectHandle.Handle.ToString());
            attributes.Add("handle_legacy", node.JavaObjectHandle.HandleLegacy.ToString());
            attributes.Add("jvmId", node.JavaObjectHandle.JvmId.ToString());
            attributes.Add("isNull", node.JavaObjectHandle.IsNull.ToString());
            attributes.Add("isClosed", node.JavaObjectHandle.IsClosed.ToString());
            attributes.Add("elementId", node.ElementId);
            return attributes;
        }

        /// <summary>
        /// Generates an XElement with no whitespace.
        /// </summary>
        /// <param name="elName">Element name.</param>
        /// <returns>A raw XElement with no additional attributes.</returns>
        private static XElement CreateXElement(string elName)
        {
            elName = Regex.Replace(elName, @"\s+", "");

            return new XElement(elName);
        }

        /// <summary>
        /// Generates an XElement with attributes.
        /// </summary>
        /// <param name="elName">Element name.</param>
        /// <param name="attributes">A dictionary containing attribute key-value pairs.</param>
        /// <returns>A XElement object with the proper attributes set.</returns>
        private static XElement CreateXElement(string elName, Dictionary<string,string> attributes)
        {
            elName = String.IsNullOrEmpty(elName) ? "empty" : elName;

            XElement el = CreateXElement(elName);

            foreach (KeyValuePair<string,string> attr in attributes)
            {
                XAttribute attribute = new XAttribute(attr.Key, attr.Value == null ? "" : attr.Value);
                el.Add(attribute);
            }

            return el;
        }



    }
}
