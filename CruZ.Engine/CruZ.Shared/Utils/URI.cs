//using Microsoft.Xna.Framework.Content.Pipeline.Builder.Convertors;
//using System.IO;

//namespace CruZ.Resource
//{
//    public class URI
//    {
//        public URI(string uri)
//        {
//            _uri = uri;
//            ValidateURI(_uri);
//        }

//        public string GetFullResPath(string sourcePrefix)
//        {
//            return Path.Combine(
//                Path.GetFullResPath(sourcePrefix), _uri);
//        }

//        public static void ValidateURI(string uri)
//        {
//            bool isValid = !string.IsNullOrEmpty(uri);
//            isValid |= !Path.IsPathRooted(uri);

//            if (!isValid)
//            {
//                throw new(string.Format("Resource URI {0} is invalid", uri));
//            }
//        }

//        public static implicit operator URI(string uri)
//        {
//            return new(uri);
//        }

//        public override string ToString()
//        {
//            return _uri;
//        }

//        private string _uri = "";
//    }
//}