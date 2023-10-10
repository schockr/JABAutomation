using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace JABAutomation
{
    public class PropertyOptionsSetting
    {
        private const PropertyOptions DefaultPropertyOptions = PropertyOptions.AccessibleContextInfo |
          PropertyOptions.AccessibleIcons |
          PropertyOptions.AccessibleKeyBindings |
          PropertyOptions.AccessibleRelationSet |
          PropertyOptions.ParentContext |
          PropertyOptions.Children |
          PropertyOptions.ObjectDepth |
          PropertyOptions.TopLevelWindowInfo |
          PropertyOptions.ActiveDescendent |
          PropertyOptions.AccessibleText |
          PropertyOptions.AccessibleHyperText |
          PropertyOptions.AccessibleValue |
          PropertyOptions.AccessibleSelection |
          PropertyOptions.AccessibleTable |
          PropertyOptions.AccessibleTableCells |
          PropertyOptions.AccessibleTableCellsSelect |
          PropertyOptions.AccessibleActions;


        public PropertyOptions GetDefaultPropertyOptions()
        {
            return DefaultPropertyOptions;
        }


    }

}

