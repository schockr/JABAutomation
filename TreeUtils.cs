using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public class TreeUtils
    {
        public static IEnumerable<TreeNode> Flatten(TreeNode root)
        {
            return Traverse(root);
        }


        /// <summary>
        /// Generates a flattened collection of <see cref="TreeNode"/> objects
        /// using a stack to find descendant nodes of the input parameter.
        /// </summary>
        /// <param name="root">The full <see cref="TreeNode"/> object starting from the root.</param>
        /// <returns>An IEnumerable<<see cref="TreeNode"/>> object.</returns>
        private static IEnumerable<TreeNode> Traverse(TreeNode root)
        {
            var stack = new Stack<TreeNode>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                foreach (var child in current.Children)
                    stack.Push(child);
            }
        }
    }
}
