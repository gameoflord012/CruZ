//using Newtonsoft.Json;
//using System.Collections.Generic;

//namespace CruZ.Tools.ResourceImporter
//{
//    public class ResourceImporterObject
//    {
//        [JsonProperty(PropertyName = "import-patterns")]
//        public List<string> ImportPatterns = new List<string>();

//        /// <summary>
//        /// Key: guid
//        /// value: relative path
//        /// </summary>
//        [JsonProperty(PropertyName = "build-result")]
//        public Dictionary<string, string> BuildResult = new Dictionary<string, string>();

//        [JsonIgnore]
//        public string ImporterFilePath { get; set; }

//        [JsonIgnore]
//        public string BuildLog { get; internal set; }
//    }
//}