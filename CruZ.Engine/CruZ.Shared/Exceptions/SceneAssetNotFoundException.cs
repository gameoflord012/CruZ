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