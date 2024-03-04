using System;

namespace CruZ.Exception;

[System.Serializable]
public class LoadResourceFailedException : System.Exception
{
	public LoadResourceFailedException() { }
	public LoadResourceFailedException(string message) : base(message) { }
	public LoadResourceFailedException(string message, System.Exception inner) : base(message, inner) { }
	protected LoadResourceFailedException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

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

[System.Serializable]
public class SystemUninitailizeException : System.Exception
{
    public SystemUninitailizeException() { }
    public SystemUninitailizeException(string message) : base(message) { }
    public SystemUninitailizeException(string message, System.Exception inner) : base(message, inner) { }
    protected SystemUninitailizeException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[System.Serializable]
public class SceneAssetNotFoundException : System.Exception
{
    public SceneAssetNotFoundException() { }
    public SceneAssetNotFoundException(string message) : base(message) { }
    public SceneAssetNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    protected SceneAssetNotFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}


[Serializable]
public class RuntimeSceneLoadException : System.Exception
{
    public RuntimeSceneLoadException() { }
    public RuntimeSceneLoadException(string message) : base(message) { }
    public RuntimeSceneLoadException(string message, System.Exception inner) : base(message, inner) { }
    protected RuntimeSceneLoadException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[Serializable]
public class InvalidGuidValueException : System.Exception
{
    public InvalidGuidValueException() { }
    public InvalidGuidValueException(string message) : base(message) { }
    public InvalidGuidValueException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidGuidValueException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}


[Serializable]
public class InvalidGuidException : System.Exception
{
    public InvalidGuidException() { }
    public InvalidGuidException(string message) : base(message) { }
    public InvalidGuidException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidGuidException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}