//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CruZ.Editor.Utility
//{
//    class BindingObject
//    {
//        public event Action<string, object>? OnBindingValueChanged;

//        public void NotifyChanged(string propertyName, object value)
//        {
//            _values[propertyName] = value;
//            OnBindingValueChanged?.Invoke(propertyName, value);
//        }

//        Dictionary<string, object> _values = [];
//    }
//}
