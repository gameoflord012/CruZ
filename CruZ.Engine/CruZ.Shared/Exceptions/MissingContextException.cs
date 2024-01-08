using System;

namespace CruZ.Exception
{
    [System.Serializable]
    public class MissingContextException : System.Exception
    {
        public MissingContextException(Type missingType) 
            : this(string.Format("Context for {0} type is not provided", missingType)) { }

        public MissingContextException(string message) : base(message) { }
        public MissingContextException(string message, System.Exception inner) : base(message, inner) { }

        protected MissingContextException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}