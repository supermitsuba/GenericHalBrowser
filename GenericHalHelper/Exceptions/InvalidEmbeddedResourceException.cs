using System;
using System.Runtime.Serialization;

namespace GenericHalHelper.Exceptions
{
    public class InvalidEmbeddedResourceException : Exception
    {
        public InvalidEmbeddedResourceException()
            : base()
        {

        }

        public InvalidEmbeddedResourceException(string msg)
            : base(msg)
        {

        }

        public InvalidEmbeddedResourceException(string msg, Exception innerException)
            : base(msg, innerException)
        {

        }
        
        protected InvalidEmbeddedResourceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

    }
}
