using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public interface ISearchContext
    {

        IJavaElement FindElement(By by);

        ReadOnlyCollection<IJavaElement> FindElements(By by);

    }
}
