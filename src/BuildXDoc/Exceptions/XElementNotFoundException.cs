using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BuildXDoc.Exceptions
{
    public class XElementNotFoundException : Exception
    {
        public XElementNotFoundException()
        {
        }

        public XElementNotFoundException(string message) : base(message)
        {
        }

        public XElementNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XElementNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
