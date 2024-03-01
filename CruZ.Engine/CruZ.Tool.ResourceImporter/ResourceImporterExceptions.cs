using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CruZ.Tools.ResourceImporter
{

	[Serializable]
	public class ResourceGuidNotFoundException : Exception
	{
		public ResourceGuidNotFoundException() { }
		public ResourceGuidNotFoundException(string message) : base(message) { }
		public ResourceGuidNotFoundException(string message, Exception inner) : base(message, inner) { }
		protected ResourceGuidNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}


	[Serializable]
	public class ResourcePathNotFoundException : Exception
	{
		public ResourcePathNotFoundException() { }
		public ResourcePathNotFoundException(string message) : base(message) { }
		public ResourcePathNotFoundException(string message, Exception inner) : base(message, inner) { }
		protected ResourcePathNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
