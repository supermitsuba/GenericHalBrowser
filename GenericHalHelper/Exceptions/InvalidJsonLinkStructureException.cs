using System;
using System.Runtime.Serialization;

namespace GenericHalHelper.Exceptions
{
    class InvalidJsonLinkStructureException : Exception
    {
        public InvalidJsonLinkStructureException()
            : base()
        {

        }

        public InvalidJsonLinkStructureException(string msg)
            : base(msg)
        {

        }

        public InvalidJsonLinkStructureException(string msg, Exception innerException)
            : base(msg, innerException)
        {

        }

        protected InvalidJsonLinkStructureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

    }
}
