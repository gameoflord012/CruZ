using System;

namespace CruZ.Exception;

[global::System.SerializableAttribute]
public class LoadResourceFailedException : global::System.Exception
{
	public LoadResourceFailedException() { }
	public LoadResourceFailedException(string message) : base(message) { }
	public LoadResourceFailedException(string message, global::System.Exception inner) : base(message, inner) { }
	protected LoadResourceFailedException(
	  global::System.Runtime.Serialization.SerializationInfo info,
	  global::System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[global::System.SerializableAttribute]
public class MissingContextException : global::System.Exception
{
    public MissingContextException(Type missingType)
        : this(string.Format("Context for {0} type is not provided", missingType)) { }

    public MissingContextException(string message) : base(message) { }
    public MissingContextException(string message, global::System.Exception inner) : base(message, inner) { }

    protected MissingContextException(
      global::System.Runtime.Serialization.SerializationInfo info,
      global::System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[global::System.SerializableAttribute]
public class SystemUninitailizeException : global::System.Exception
{
    public SystemUninitailizeException() { }
    public SystemUninitailizeException(string message) : base(message) { }
    public SystemUninitailizeException(string message, global::System.Exception inner) : base(message, inner) { }
    protected SystemUninitailizeException(
      global::System.Runtime.Serialization.SerializationInfo info,
      global::System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[global::System.SerializableAttribute]
public class SceneAssetNotFoundException : global::System.Exception
{
    public SceneAssetNotFoundException() { }
    public SceneAssetNotFoundException(string message) : base(message) { }
    public SceneAssetNotFoundException(string message, global::System.Exception inner) : base(message, inner) { }
    protected SceneAssetNotFoundException(
      global::System.Runtime.Serialization.SerializationInfo info,
      global::System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}


[Serializable]
public class RuntimeSceneLoadException : global::System.Exception
{
    public RuntimeSceneLoadException() { }
    public RuntimeSceneLoadException(string message) : base(message) { }
    public RuntimeSceneLoadException(string message, global::System.Exception inner) : base(message, inner) { }
    protected RuntimeSceneLoadException(
      global::System.Runtime.Serialization.SerializationInfo info,
      global::System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[Serializable]
public class InvalidGuidValueException : global::System.Exception
{
    public InvalidGuidValueException() { }
    public InvalidGuidValueException(string message) : base(message) { }
    public InvalidGuidValueException(string message, global::System.Exception inner) : base(message, inner) { }
    protected InvalidGuidValueException(
      global::System.Runtime.Serialization.SerializationInfo info,
      global::System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}


[Serializable]
public class InvalidGuidException : global::System.Exception
{
    public InvalidGuidException() { }
    public InvalidGuidException(string message) : base(message) { }
    public InvalidGuidException(string message, global::System.Exception inner) : base(message, inner) { }
    protected InvalidGuidException(
      global::System.Runtime.Serialization.SerializationInfo info,
      global::System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}