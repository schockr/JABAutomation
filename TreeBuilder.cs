using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace JABAutomation
{
    public class TreeBuilder
    {
        private static PropertyOptions _propertyOptions = new PropertyOptionsSetting().GetDefaultPropertyOptions();
        public static TreeNode BuildTree(AccessibleContextInfo acInfo, int depth, WindowsAccessBridgeInterop.AccessBridge accessBridge, JavaObjectHandle ac)
        {
            int JvmId = ac.JvmId;
            TreeNode parentNode = new TreeNode(acInfo, ac);

            AccessibleContextNode acNode = new AccessibleContextNode(accessBridge, ac);
            parentNode.SetAcNode(acNode);

            parentNode.SetProperties(acNode.GetProperties(_propertyOptions));
            parentNode.SetElementId(GenerateGuid());


            if (acInfo.childrenCount == 0)
            {
                // base case
                return parentNode;
            }

            depth += 1;


            for (int i = 0; i < acInfo.childrenCount; i++)
            {
                int vmid = ac.JvmId;
                JavaObjectHandle childAc = accessBridge.Functions.GetAccessibleChildFromContext(vmid, ac, i);
                AccessibleContextInfo childInfo = new AccessibleContextInfo();
                accessBridge.Functions.GetAccessibleContextInfo(vmid, childAc, out childInfo);

                //recurse(childInfo, accessBridge, childAc, vmid, list);
                
                var childNode = BuildTree(childInfo, depth, accessBridge, childAc);
                if (childNode != null)
                {
                    parentNode.AddChild(childNode, depth);
                }
            }
            return parentNode;
        }



        private static string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
