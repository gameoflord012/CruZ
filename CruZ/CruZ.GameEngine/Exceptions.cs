#pragma warning disable SYSLIB0051 // Type or member is obsolete

using System;

namespace CruZ.GameEngine;

[Serializable]
public class LoadResourceFailedException : Exception
{
    public LoadResourceFailedException() { }
    public LoadResourceFailedException(string message) : base(message) { }
    public LoadResourceFailedException(string message, Exception inner) : base(message, inner) { }
    protected LoadResourceFailedException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[Serializable]
public class MissingContextException : Exception
{
    public MissingContextException(Type missingType)
        : this(string.Format("Context for {0} type is not provided", missingType)) { }

    public MissingContextException(string message) : base(message) { }
    public MissingContextException(string message, Exception inner) : base(message, inner) { }

    protected MissingContextException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[Serializable]
public class SystemUninitailizeException : Exception
{
    public SystemUninitailizeException() { }
    public SystemUninitailizeException(string message) : base(message) { }
    public SystemUninitailizeException(string message, Exception inner) : base(message, inner) { }
    protected SystemUninitailizeException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}


[Serializable]
public class SceneException : Exception
{
    public SceneException() { }
    public SceneException(string message) : base(message) { }
    public SceneException(string message, Exception inner) : base(message, inner) { }
    protected SceneException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[Serializable]
public class InvalidGuidValueException : Exception
{
    public InvalidGuidValueException() { }
    public InvalidGuidValueException(string message) : base(message) { }
    public InvalidGuidValueException(string message, Exception inner) : base(message, inner) { }
    protected InvalidGuidValueException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}


[Serializable]
public class InvalidGuidException : Exception
{
    public InvalidGuidException() { }
    public InvalidGuidException(string message) : base(message) { }
    public InvalidGuidException(string message, Exception inner) : base(message, inner) { }
    protected InvalidGuidException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
