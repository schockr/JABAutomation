using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    [Serializable]
    public class JavaDriverException : Exception
    {
        public JavaDriverException() :base() { }

        public JavaDriverException(string message) : base(message) { }

        public JavaDriverException(string message, Exception innerException) : base(message, innerException) { }

        protected JavaDriverException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
