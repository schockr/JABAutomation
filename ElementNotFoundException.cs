using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    [Serializable]
    public class ElementNotFoundException : JavaDriverException
    {
        public ElementNotFoundException() : base() { }

        public ElementNotFoundException(string message) : base(message) { }

        public ElementNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public ElementNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
