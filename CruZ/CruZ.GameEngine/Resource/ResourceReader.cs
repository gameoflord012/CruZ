//using System;
//using System.Collections.Generic;

//namespace CruZ.GameEngine.Resource
//{
//    internal class ResourceReaderCollection
//    {
//        public void AddReader<T>(T reader) where T : notnull
//        {
//            if(!_readers.TryAdd(typeof(T), reader))
//            {
//                throw new ArgumentException();
//            }
//        }

//        public T GetReader<T>()
//        {
//            return (T)_readers[typeof(T)];
//        }

//        Dictionary<Type, object> _readers = [];
//    }
//}
