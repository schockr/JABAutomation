using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public interface IElementLocator
    {
        TreeNode FindElement(TreeNode root, string criteria);


        ReadOnlyCollection<TreeNode> FindElements(TreeNode root, string criteria);

    }
}
