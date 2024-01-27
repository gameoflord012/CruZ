//using CruZ.Editor.Bindings;
//using System;
//using System.Collections.Generic;
//using System.Reflection;

//namespace CruZ.Components
//{
//    public partial class Transform : IProvideBinding
//    {
//        //public IEnumerable<KeyValuePair<string, Type>> GetBindingProperties()
//        //{
//        //    yield return new("Position", typeof(Vector3));
//        //}

//        public object GetRegistedChangeEvent(string propertyName, Type ty, Action<object> changeEvent)
//        {
//            EventHandler<TransformEventArgs> listener = (sender, args) => changeEvent.Invoke(args);

//            if(propertyName == "Position")
//            {
//                OnPositionChanged += listener;
//            }

//            if(propertyName == "Scale")
//            {
//                OnScaleChanged += listener;
//            }

//            return listener;
//        }

//        public void RemoveRegistedChangeEvent(string propertyName, Type propertyType, object listener)
//        {
//            if(propertyName == "Position")
//            {
//                OnPositionChanged -= (EventHandler<TransformEventArgs>)listener;
//            }

//            if(propertyName == "Scale")
//            {
//                OnScaleChanged -= (EventHandler<TransformEventArgs>)listener;
//            }
//        }
//    }
//}