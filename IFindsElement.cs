using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public interface IFindsElement
    {
        TreeNode FindElementByPath(string path);

        ReadOnlyCollection<TreeNode> FindElementsByPath(string path);
        
        TreeNode FindElementByPushButton(string path);

        ReadOnlyCollection<TreeNode> FindElementsByPushButton(string path);
        
        TreeNode FindElementByLabelText(string path);

        ReadOnlyCollection<TreeNode> FindElementsByLabelText(string path);
       
        TreeNode FindElementByCombobox(string path);

        ReadOnlyCollection<TreeNode> FindElementsByCombobox(string path);

        TreeNode FindElementByName(string path);

        ReadOnlyCollection<TreeNode> FindElementsByName(string path);

        TreeNode FindElementByDescription(string desc);

        ReadOnlyCollection<TreeNode> FindElementsByDescription(string desc);

        TreeNode FindElementByTextboxName(string path);

        ReadOnlyCollection<TreeNode> FindElementsByTextboxName(string path);
        
        TreeNode FindElementByLabelName(string path);

        ReadOnlyCollection<TreeNode> FindElementsByLabelName(string path);
        
        TreeNode FindElementByRadioButton(string path);

        ReadOnlyCollection<TreeNode> FindElementsByRadioButton(string path);
       
        TreeNode FindElementByToggleButton(string path);

        ReadOnlyCollection<TreeNode> FindElementsByToggleButton(string path);
        
        TreeNode FindElementByMenuItem(string path);

        ReadOnlyCollection<TreeNode> FindElementsByMenuItem(string path);
       
    }
}
