using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public interface IFindElementStrategy
    {
        TreeNode FindElement(TreeNode root, string finderCritera);

        ReadOnlyCollection<TreeNode> FindElements(TreeNode root, string finderCriteria);
    }
}
