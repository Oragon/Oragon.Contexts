using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Context.Tests.Schema
{
    [System.Serializable]
    public class NewException : Exception
    {
        public NewException() { }
        public NewException(string message) : base(message) { }
        public NewException(string message, Exception inner) : base(message, inner) { }
        protected NewException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
