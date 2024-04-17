using System.Text.Json.Serialization;

namespace CruZ.Framework.Serialization
{
    /// <summary>
    /// Implementation is from https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/preserve-references
    /// </summary>
    internal class ResetReferenceHandler : ReferenceHandler
    {
        public ResetReferenceHandler() => Reset();
        private ReferenceResolver? _rootedResolver;
        public override ReferenceResolver CreateResolver() => _rootedResolver!;
        public void Reset() => _rootedResolver = new BasicReferenceResolver();
    }
}
