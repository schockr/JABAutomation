using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    [Serializable]
    public class WindowNotFoundException : JavaDriverException
    {
        public WindowNotFoundException() : base() { }

        public WindowNotFoundException(string message) : base(message) { }

        public WindowNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public WindowNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
