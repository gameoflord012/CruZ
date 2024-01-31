namespace CruZ.Exception
{

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
}