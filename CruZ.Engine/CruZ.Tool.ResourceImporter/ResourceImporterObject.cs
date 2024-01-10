using Newtonsoft.Json;

namespace CruZ.Tool.ResourceImporter
{
    public class ResourceImporterObject
    {
        [JsonProperty(PropertyName = "import-patterns")]
        public List<string> ImportPatterns = [];

        [JsonProperty(PropertyName = "build-result")]
        public Dictionary<string, string> BuildResult = [];
    }
}