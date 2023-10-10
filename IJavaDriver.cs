using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public interface IJavaDriver : IDisposable
    {
        /// <summary>
        /// Gets the name of the current app frame.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the current JVM Id.
        /// </summary>
        int JvmId { get; }


    }
}
