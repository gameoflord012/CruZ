//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace CruZ.Tools.ResourceImporter
//{
//    /// <summary>
//    /// Format of .dotimporter files as follow <br/>
//    /// Line 1: Guid <br/>
//    /// Line 2..n: Resource refernces <br/>
//    /// </summary>
//    public partial class DotImportObject
//    {
//        public Guid Guid { get => _guid; set => _guid = value; }
        
//        public void Read(string dotImportPath)
//        {
//            _dotImportPath = dotImportPath;

//            using(StreamReader reader = new StreamReader(dotImportPath))
//            {
//                var guidString = reader.ReadLine();
//                _guid = Guid.Parse(guidString);

//                string line;
//                while(string.IsNullOrEmpty(line = reader.ReadLine()))
//                {
//                    AddResourceRef(ResourceRef.Prase(line));
//                }
//            }
//        }

//        public void Save()
//        {
//            if(string.IsNullOrEmpty(_dotImportPath)) throw new InvalidOperationException("Read first");

//            using(StreamWriter writer = new StreamWriter(_dotImportPath))
//            {
//                writer.WriteLine(Guid.ToString());
//                foreach (var resRef in _resourceRefs)
//                {
//                    writer.WriteLine(resRef.ToString());
//                }
//            }
//        }

//        public void AddResourceRef(Guid guid, string label)
//        {
//            AddResourceRef(new ResourceRef(guid, label));
//        }

//        private void AddResourceRef(ResourceRef resourceRef)
//        {
//            if (_resourceRefs.Contains(resourceRef)) return;
//            _resourceRefs.Add(resourceRef);
//        }

//        Guid _guid;
//        List<ResourceRef> _resourceRefs = new List<ResourceRef>();
//        string _dotImportPath;
//    }
//}
