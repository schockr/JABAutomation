using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace JABAutomation
{
    public class TreeNode
    {
        private JavaObjectHandle Handle { get; set; }
        private AccessibleContextNode AcNode { get; set; } 
        private AccessibleContextInfo _accessibleContextInfo { get; set; }
        private Path<AccessibleNode> _nodePath { get; set; }
        private PropertyList properties { get; set; }
        private string elementId { get; set; }

        public List<TreeNode> Children = new List<TreeNode>();

        public int Depth { get; private set; }

        public TreeNode(AccessibleContextInfo accessibleContextInfo, JavaObjectHandle handle)
        {
            _accessibleContextInfo = accessibleContextInfo;
            Handle = handle;
        }

        public AccessibleContextInfo AccessibleContextInfo { get {  return _accessibleContextInfo; } }
        public JavaObjectHandle JavaObjectHandle { get { return Handle; } }

        public void SetElementId(string id)
        {
            this.elementId = id;
        }
        public string ElementId
        {
            get
            {
                return this.elementId;
            }
        }

        public void SetProperties(PropertyList props)
        {
            this.properties = props;
        }

        public PropertyList GetProperties() { return this.properties; }


        public void SetAccessibleTextSentence()
        {
            var propertyToUpdate = properties
                    .OfType<PropertyGroup>()
                    .Where(group => StringUtils.EqualsIgnoreCase(group.Name, "accessible text"))
                    .SelectMany(group => group.Children)
                    .OfType<PropertyGroup>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "Text attributes at point (0, 0)"))
                    .SelectMany(subGroup => subGroup.Children)
                    .OfType<PropertyNode>()
                    .FirstOrDefault(prop => StringUtils.EqualsIgnoreCase(prop.Name, "sentence"))
                        ;





            if (propertyToUpdate != null)
            {
                // Create a new PropertyNode with the updated value
                var updatedPropertyNode = new PropertyNode(propertyToUpdate.Name, "test");

                // Find the parent group using LINQ
                var parentGroup = properties
                        .OfType<PropertyGroup>()
                        .Where(group => StringUtils.EqualsIgnoreCase(group.Name, "accessible text"))
                        .SelectMany(group => group.Children)
                        .OfType<PropertyGroup>()
                        .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "Text attributes at point (0, 0)"))
                        .FirstOrDefault(subGroup => subGroup.Children.Contains(propertyToUpdate));

                if (parentGroup != null)
                {
                    int index = parentGroup.Children.IndexOf(propertyToUpdate);

                    if (index >= 0)
                    {
                        parentGroup.Children[index] = updatedPropertyNode;
                    }
                }

                //   .OfType<PropertyNode>()
                //  .FirstOrDefault(prop => StringUtils.EqualsIgnoreCase(prop.Name, "sentence"));
                // test = updatedPropertyNode;

                //parentGroup = 

                //  if (parentGroup != null)
                //{
                //  int index = parentGroup.Children.IndexOf(propertyToUpdate);

                //if (index >= 0)
                //{
                //   parentGroup.Children[index] = updatedPropertyNode;
                // }
                //}
            }
        }


        public string GetAccessibleTextSentence()
        {
            string result = properties
                    .OfType<PropertyGroup>()
                    .Where(group => StringUtils.EqualsIgnoreCase(group.Name, "accessible text"))
                    .SelectMany(group => group.Children)
                    .OfType<PropertyGroup>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "Text attributes at point (0, 0)"))
                    .SelectMany(subGroup => subGroup.Children)
                    .OfType<PropertyNode>()
                    .FirstOrDefault(prop => StringUtils.EqualsIgnoreCase(prop.Name, "sentence"))
                    ?.Value
                    ?.ToString() ?? string.Empty;

            return result;


            StringBuilder sbText = new StringBuilder();
            int caretIndex = 0;
            while (true)
            {
                
            }
        }

        public void SetAcNode(AccessibleContextNode AcNode)
        {
            this.AcNode = AcNode;
            this._nodePath = AcNode.BuildNodePath();
        }
        public AccessibleContextNode GetAcNode()
        {
            return this.AcNode;
        }

        public void AddChild(TreeNode child, int depth)
        {
            child.Depth = depth;
            Children.Add(child);
        }
    }
}
